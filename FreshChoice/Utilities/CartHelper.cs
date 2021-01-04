using FreshChoice.Models;
using FreshChoice.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace FreshChoice.Utilities
{
    public class CartHelper
    {
        private int currentUserId;
        private static CartHelper _instance;
        private static Cart currentCart;
        // Hours
        private const int ORDER_PLACED_DEADLINE = 8;
        private CartHelper(int UserId) {
            currentUserId = UserId;
        }
        public static CartHelper GetInstance(int UserId)
        {
            if(_instance == null)
            {
                _instance = new CartHelper(UserId);
            }

            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                currentCart = db.Carts.Where(w => w.UserId == UserId && w.CartIsActive).FirstOrDefault();
            }

            return _instance;
        }
        public List<CartItem> GetCartItems()
        {
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                if (currentCart == null)
                {
                    return null;
                }
                if (currentCart != null)
                {
                    return db.CartItems.Where(w => w.CartId == currentCart.CartId).ToList();
                }
            }
            return null;
        }
        public List<OrderCartItem> GetOrderCartItems()
        {
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                List<CartItem> cartItems = GetCartItems();

                if (cartItems != null)
                {
                    List<OrderCartItem> orderCartItems = new List<OrderCartItem>();
                    foreach(var item in cartItems)
                    {
                        OrderCartItem orderCartItem = new OrderCartItem();
                        Item selectedItem = db.Items.Where(w => w.ItemId == item.ItemId).FirstOrDefault();
                        Image selectedItemImage = db.Images.Where(w => w.ItemId == item.ItemId).FirstOrDefault();
                        orderCartItem.ItemId = item.ItemId;
                        orderCartItem.Quantity = item.CartItemQnt;
                        orderCartItem.ItemName = selectedItem.ItemName;
                        orderCartItem.ItemPrice = selectedItem.ItemPrice;
                        orderCartItem.ItemDescription = selectedItem.ItemDescription;
                        if (selectedItemImage != null)
                        {
                            orderCartItem.ItemImageUrl = selectedItemImage.ImageUrl;
                        }
                        orderCartItems.Add(orderCartItem);
                    }
                    return orderCartItems;
                }
            }
            return null;
        }
        public List<OrderCartItem> AddItemToCart(int ItemId, int Quantity = 1)
        {
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                Item item = db.Items.Where(w => w.ItemId == ItemId).FirstOrDefault();
                if (item != null && item.ItemAvailableQnt >= Quantity)
                {
                    bool isNewCart = false;
                    if (currentCart == null)
                    {
                        currentCart = new Cart();
                        currentCart.CartIsActive = true;
                        currentCart.UserId = currentUserId;
                        db.Carts.Add(currentCart);
                        db.SaveChanges();
                        isNewCart = true;
                    }

                    CartItem cartItem = null;
                    if (!isNewCart)
                    {
                        // check this item is already exist in cart
                        cartItem = db.CartItems
                            .Where(w => w.CartId == currentCart.CartId && w.ItemId == ItemId)
                            .FirstOrDefault();
                    }
                    if (cartItem != null)
                    {
                        cartItem.CartItemQnt += Quantity;
                        cartItem.CartItemTotalPrice = cartItem.CartItemQnt * item.ItemPrice;
                    }
                    else
                    {
                        // New
                        cartItem = new CartItem();
                        cartItem.ItemId = ItemId;
                        cartItem.CartId = currentCart.CartId;
                        cartItem.CartItemTotalPrice = item.ItemPrice * Quantity;
                        cartItem.CartItemQnt += Quantity;
                        db.CartItems.Add(cartItem);
                    }
                    db.SaveChanges();
                    item.ItemAvailableQnt -= Quantity;
                    db.SaveChanges();
                }
                else
                {
                    return null;
                }

                // add it to cart

                // return new cart items
                return GetOrderCartItems();
            }
        }
        public List<OrderCartItem> RemoveItemFromCart(int ItemId, int Quantity = 1)
        {
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                Item item = db.Items.Where(w => w.ItemId == ItemId).FirstOrDefault();
                if (item != null && item.ItemAvailableQnt >= Quantity)
                {
                    if (currentCart == null)
                    {
                        return null;
                    }

                    CartItem cartItem = db.CartItems
                            .Where(w => w.CartId == currentCart.CartId && w.ItemId == ItemId)
                            .FirstOrDefault();
                    if (cartItem != null)
                    {
                        if ((cartItem.CartItemQnt - Quantity) > 0)
                        {
                            // Normal reduce
                            cartItem.CartItemQnt -= Quantity;
                            cartItem.CartItemTotalPrice = cartItem.CartItemQnt * item.ItemPrice;
                        }
                        else if ((cartItem.CartItemQnt - Quantity) == 0)
                        {
                            // Remove item from cart
                            db.CartItems.Remove(cartItem);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                    db.SaveChanges();
                    item.ItemAvailableQnt += Quantity;
                    db.SaveChanges();
                }
                else
                {
                    return null;
                }

                // return new cart items
                return GetOrderCartItems();
            }
        }
        public bool DeleteCart()
        {
            if (currentCart == null)
            {
                return false;
            }
            using (FreshChoiceEntities db = new FreshChoiceEntities()) {
                currentCart.CartIsActive = false;
                Cart cart = db.Carts.Where(w => w.CartId == currentCart.CartId).FirstOrDefault();
                db.SaveChanges();
                return true;
            }
        }
        public Order PlaceOrder(string DeliveryType, bool UseWalletCash = false)
        {
            if (currentCart == null)
            {
                return null;
            }
            List<CartItem> cartItems = GetCartItems();
            if (cartItems == null || cartItems.Count == 0)
            {
                return null;
            }
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                if (DeliveryType == DeliveryTypes.HOME_DELIVERY)
                {
                    Address address = db.Addresses.Where(w => w.UserId == currentUserId).FirstOrDefault();
                    if (address == null)
                    {
                        return null;
                    }
                }
                OrderStatu orderStatus = db.OrderStatus.Where(w => w.OrderStatusOrder == OrderProcessingStatus.PLACED).FirstOrDefault();
                Delivery delivery = new Delivery();
                delivery.DeliveryType = DeliveryType;
                db.Deliveries.Add(delivery);
                db.SaveChanges();

                Order order = new Order();
                order.OrderDescription = "New Order";
                order.OrderBillNo = GenerateBillNo(DeliveryType);
                order.OrderConfirmationNo = GenerateConfirmationNo(DeliveryType);
                order.OrderUpdatedDate = DateTime.Now;
                order.OrderNextDeadline = DateTime.Now.AddHours(ORDER_PLACED_DEADLINE);
                order.DeliveryId = delivery.DeliveryId;
                order.OrderStatusId = orderStatus.OrderStatusId;
                order.CartId = currentCart.CartId;

                // calculate cart amount
                double total = 0;
                foreach(var cartItem in cartItems)
                {
                    total += cartItem.CartItemTotalPrice;
                }
                // Add delivery Fees
                double deliveryFee = 0;
                if (DeliveryType == DeliveryTypes.HOME_DELIVERY)
                {
                    Address address = db.Addresses.Where(w => w.UserId == currentUserId).FirstOrDefault();
                    if (address != null)
                    {
                        var userCoordinate = new GeoCoordinate(address.AddressLatitude, address.AddressLongitude);
                        var shopCoordinate = new GeoCoordinate(StoreLocation.STORE_LATITUDE, StoreLocation.STORE_LONGITUDE);

                        var distanceInKM = userCoordinate.GetDistanceTo(shopCoordinate) / 1000;
                        deliveryFee = distanceInKM * 50;
                        delivery.DeliveryFee = deliveryFee;
                    }
                }

                order.OrderAmount = total + deliveryFee;
                order.UserId = currentUserId;

                // if using wallet cash
                if (UseWalletCash)
                {
                    // add amount to user wallet
                    Wallet wallet = db.Wallets.Where(w => w.UserId == currentUserId).FirstOrDefault();

                    if (wallet == null || wallet.WalletTotal < order.OrderAmount)
                    {
                        return null;
                    }

                    // create transaction
                    Transaction transaction = new Transaction();
                    transaction.TransactionDate = DateTime.Now;
                    transaction.TransactionDescription = "cancel_order_" + order.OrderId;
                    transaction.TransactionAmount = 0 - order.OrderAmount;
                    transaction.WalletId = wallet.WalletId;
                    db.Transactions.Add(transaction);
                    wallet.WalletTotal -= order.OrderAmount;
                    db.SaveChanges();
                }

                db.Orders.Add(order);
                db.SaveChanges();
                DeleteCart();
                return order;

            }
            
        }
        public double GetDeliveryFees()
        {
            double deliveryFee = 0;
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                Address address = db.Addresses.Where(w => w.UserId == currentUserId).FirstOrDefault();
                if (address != null)
                {
                    var userCoordinate = new GeoCoordinate(address.AddressLatitude, address.AddressLongitude);
                    var shopCoordinate = new GeoCoordinate(StoreLocation.STORE_LATITUDE, StoreLocation.STORE_LONGITUDE);

                    var distanceInKM = userCoordinate.GetDistanceTo(shopCoordinate) / 1000;
                    deliveryFee = distanceInKM * 50;
                }
            }
            return deliveryFee;
        }
        public double GetDeliveryInKilometers()
        {
            double distance = 0;
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                Address address = db.Addresses.Where(w => w.UserId == currentUserId).FirstOrDefault();
                if (address != null)
                {
                    var userCoordinate = new GeoCoordinate(address.AddressLatitude, address.AddressLongitude);
                    var shopCoordinate = new GeoCoordinate(StoreLocation.STORE_LATITUDE, StoreLocation.STORE_LONGITUDE);

                    distance = userCoordinate.GetDistanceTo(shopCoordinate) / 1000;
                }
            }
            return distance;
        }
        public List<AllOrderItem> GetAllOrderItems()
        {
            List<AllOrderItem> orders = new List<AllOrderItem>();
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                var newOrders = db.Orders
                    .Join(
                        db.OrderStatus,
                        o => o.OrderStatusId,
                        os => os.OrderStatusId,
                        (o, os) => new { order = o, orderStatus = os }
                    )
                    .Join(
                        db.Carts,
                        ows => ows.order.CartId,
                        c => c.CartId,
                        (ows, c) => new { order = ows.order, orderStatus = ows.orderStatus, cart = c }
                    )
                    .Where(w => w.order.UserId == currentUserId)
                    .OrderByDescending(o => o.order.OrderUpdatedDate)
                    .ToList();

                foreach (var order in newOrders)
                {
                    AllOrderItem orderItem = new AllOrderItem();
                    orderItem.OrderId = order.order.OrderId;
                    orderItem.BillNo = order.order.OrderBillNo;
                    orderItem.ConfirmationNo = order.order.OrderConfirmationNo;
                    orderItem.LastUpdate = order.order.OrderUpdatedDate;
                    orderItem.NextDeadline = order.order.OrderNextDeadline;
                    orderItem.OrderStatus = order.orderStatus.OrderStatusDescription;
                    orderItem.OrderAmount = order.order.OrderAmount;

                    var items = db.CartItems
                        .Where(w => w.CartId == order.order.Cart.CartId)
                        .Join(db.Items,
                            ci => ci.ItemId,
                            i => i.ItemId,
                            (ci, i) => new { cartItem = ci, item = i })
                        .ToList();

                    orderItem.Items = new List<OrderCartItem>();
                    foreach (var item in items)
                    {
                        OrderCartItem cartItem = new OrderCartItem();
                        cartItem.ItemId = item.item.ItemId;
                        cartItem.Quantity = item.cartItem.CartItemQnt;
                        cartItem.ItemName = item.item.ItemName;
                        cartItem.ItemPrice = item.item.ItemPrice;

                        var image = db.Images.Where(w => w.ItemId == item.item.ItemId).FirstOrDefault();
                        if (image != null)
                        {
                            cartItem.ItemImageUrl = image.ImageUrl;
                        }
                        orderItem.Items.Add(cartItem);
                    }

                    orders.Add(orderItem);
                }
            }
            return orders;
        }
        public bool CancelOrder(int OrderId)
        {
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                Order order = db.Orders.Where(w => w.OrderId == OrderId).FirstOrDefault();
                if (order != null)
                {
                    // delete order
                    Cart cart = db.Carts.Where(w => w.CartId == order.CartId).FirstOrDefault();
                    // add amount to user wallet
                    Wallet wallet = db.Wallets.Where(w => w.UserId == currentUserId).FirstOrDefault();
                    if (wallet == null)
                    {
                        // create wallet
                        wallet = new Wallet();
                        wallet.WalletDescription = currentUserId + "_wallet";
                        wallet.WalletTotal = 0;
                        db.Wallets.Add(wallet);
                        db.SaveChanges();

                    }
                    // create transaction
                    Transaction transaction = new Transaction();
                    transaction.TransactionDate = DateTime.Now;
                    transaction.TransactionDescription = "cancel_order_" + order.OrderId;
                    transaction.TransactionAmount = order.OrderAmount;
                    transaction.WalletId = wallet.WalletId;
                    db.Transactions.Add(transaction);
                    wallet.WalletTotal += order.OrderAmount;
                    db.SaveChanges();

                    db.Orders.Remove(order);
                    db.SaveChanges();

                    // add items to list again
                    if (cart != null)
                    {
                        var items = db.CartItems
                            .Where(w => w.CartId == cart.CartId)
                            .Join(
                                db.Items,
                                ci => ci.ItemId,
                                i => i.ItemId,
                                (ci, i) => new { Item = i, CartItem = ci })
                            .ToList();
                        // update available quantity
                        foreach(var item in items)
                        {
                            Item item1 = db.Items.Where(w => w.ItemId == item.Item.ItemId).FirstOrDefault();
                            item1.ItemAvailableQnt += item.CartItem.CartItemQnt;
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        private string GenerateBillNo(string DeliveryType)
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000000).ToString("D8");
            return DeliveryType + "_U_" + currentUserId + "_" + r;
        }
        private string GenerateConfirmationNo(string DeliveryType)
        {
            Random generator = new Random();
            String r = generator.Next(0, 10000).ToString("D4");
            return DeliveryType + "_" + r + DateTime.Now.Second.ToString();
        }
    }

}