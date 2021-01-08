using CustomAuthorizationFilter.Infrastructure;
using FreshChoice.Models;
using FreshChoice.Utilities;
using FreshChoice.ViewModels.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreshChoice.Controllers
{
    [CustomAuthenticationFilter]
    public class AdminController : Controller
    {
        [CustomAuthorize("Admin")]
        public ActionResult Index()
        {
            DashBoardViewModel viewModel = new DashBoardViewModel();
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                var items = db.Orders
                    .Join(
                    db.Carts,
                    o => o.CartId,
                    c => c.CartId,
                    (o, c) => new { Order = o, Cart = c })
                    .Join(
                    db.CartItems,
                    oc => oc.Cart.CartId,
                    ci => ci.CartId,
                    (oc, ci) => new {OrderCart = oc, CartItem = ci}
                    )
                    .Join(
                    db.Items,
                    occi => occi.CartItem.ItemId,
                    i => i.ItemId,
                    (occi, i) => new { Item = i, CartItem = occi.CartItem}
                    ).ToList();
                viewModel.CategorySalesItems = new List<CategorySalesItem>();
                viewModel.TotalSales = 0;
                foreach(var item in items)
                {
                    CategorySalesItem categorySalesItem = new CategorySalesItem();
                    categorySalesItem.CategoryName = item.Item.Brand.Category.CategoryName;
                    categorySalesItem.TotalSalesValue = item.CartItem.CartItemTotalPrice;
                    viewModel.TotalSales += categorySalesItem.TotalSalesValue;
                    viewModel.CategorySalesItems.Add(categorySalesItem);
                }

                var cats = viewModel.CategorySalesItems.Select(s => s.CategoryName).Distinct().ToList();
                viewModel.DataJson = new List<double>();
                foreach (var cat in cats)
                {
                    double total = 0;
                    var catItems = viewModel.CategorySalesItems.Where(w => w.CategoryName == cat).ToList();
                    foreach(var i in catItems)
                    {
                        total += i.TotalSalesValue;
                    }
                    viewModel.DataJson.Add(total);
                }
            }
            return View(viewModel);
        }
        [CustomAuthorize("Admin", "Sales")]
        public ActionResult Items()
        {
            List<Item> items = new List<Item>();
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                items = db.Items.Where(w=> w.ItemIsDeleted == null || w.ItemIsDeleted != true).ToList();
            }
            return View(items);
        }

        [CustomAuthorize("Admin", "Sales")]
        public ActionResult ManageCategories()
        {
            ManageCategoriesViewModel viewModel = new ManageCategoriesViewModel();
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                viewModel.Categories = db.Categories.ToList();
                viewModel.Brands = db.Brands.ToList();
            }
            return View(viewModel);
        }

        [CustomAuthorize("Admin", "Sales")]
        public ActionResult SaveBrand(int? CategoryId, string BrandName, int? BrandId)
        {
            bool result = false;

            if ((BrandId == null && CategoryId == null) || BrandName.Length < 1)
            {
                return Json(result);
            }
            try
            {
                using(FreshChoiceEntities db = new FreshChoiceEntities())
                {
                    Brand brand = new Brand();
                    if (BrandId == null)
                    {
                        brand.CategoryId = (int) CategoryId;
                        brand.BrandName = BrandName;
                        db.Brands.Add(brand);
                    }
                    else
                    {
                        brand = db.Brands.Where(w => w.BrandId == BrandId).FirstOrDefault();
                        brand.BrandName = BrandName;
                    }
                    
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception) { }
            return Json(result);
        }
        [CustomAuthorize("Admin", "Sales")]
        public ActionResult SaveCategory(int CategoryId, string CategoryName)
        {
            bool result = false;

            try
            {
                using (FreshChoiceEntities db = new FreshChoiceEntities())
                {
                    Category category = new Category();
                    if (CategoryId == 0)
                    {
                        category.CategoryName = CategoryName;
                        db.Categories.Add(category);
                    }
                    else
                    {
                        category = db.Categories.Where(w => w.CategoryId == CategoryId).FirstOrDefault();
                        category.CategoryName = CategoryName;
                    }

                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception) { }
            return Json(result);
        }
        [CustomAuthorize("Admin", "Sales")]
        public ActionResult EditItem(int? Id)
        {
            EditItemViewModel viewModel = new EditItemViewModel();

            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                if (Id == null)
                {
                    viewModel.Item = new Item();
                }
                else
                {
                    viewModel.Item = db.Items.Where(w => w.ItemId == (int)Id).FirstOrDefault();
                }
                viewModel.Categories = db.Categories.ToList();
                viewModel.Brands = db.Brands.ToList();

                viewModel.Images = db.Images.Where(w => w.ItemId == viewModel.Item.ItemId).ToList();

                var catJson = viewModel.Categories
                    .Select(s => new { s.CategoryName, s.CategoryId })
                    .ToList();
                var brandJson = viewModel.Brands
                    .Select(s => new { s.BrandName, s.CategoryId, s.BrandId })
                    .ToList();

                viewModel.CategoriesJson = JsonConvert.SerializeObject(catJson);
                viewModel.BrandsJson = JsonConvert.SerializeObject(brandJson);
            }
            return View(viewModel);
        }

        [CustomAuthorize("Admin", "Sales")]
        public ActionResult SaveItem(EditItemViewModel viewModel)
        {
            bool result = false;
            
                try
                {
                    using (FreshChoiceEntities db = new FreshChoiceEntities())
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        List<Image> images = new List<Image>();

                        // create Item
                        Item item = new Item();
                        if (viewModel.ItemId != 0)
                        {
                            item = db.Items.Where(w => w.ItemId == viewModel.ItemId).FirstOrDefault();
                        }

                        item.ItemName = viewModel.ItemName;
                        item.ItemDescription = viewModel.ItemDescription;
                        item.ItemAvailableQnt = viewModel.ItemAvailableQnt;
                        if (viewModel.ItemId == 0)
                        {
                            item.ItemQnt = viewModel.ItemAvailableQnt;
                        }
                        else
                        {
                            item.ItemQnt = (item.ItemQnt + (viewModel.ItemAvailableQnt - item.ItemAvailableQnt));
                        }
                        item.ItemPrice = viewModel.ItemPrice;
                        item.ItemIsDeleted = false;
                        item.BrandId = viewModel.ItemBrandId;
                        if (viewModel.ItemId == 0)
                        {
                            db.Items.Add(item);
                        }
                        db.SaveChanges();

                        int imageCount = db.Images.Where(w => w.ItemId == item.ItemId).Count();
                        if (Request.Files.Count > 0)
                        {
                            for (int i = 0; i < files.Count; i++)
                            {

                                HttpPostedFileBase file = files[i];
                                string fname;

                                // Checking for Internet Explorer  
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = file.FileName;
                                }

                                string[] find = fname.Split('.');
                                string extention = find[find.Length - 1];
                                fname = item.ItemName + "_" + (i+ imageCount) + "." + extention;

                                // Get the complete folder path and store the file inside it.  
                                var finalName = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                                file.SaveAs(finalName);

                                // Create image
                                Image image = new Image();
                                image.ImageDescription = "";
                                image.ImageUrl = "/Uploads/" + fname;
                                image.ItemId = item.ItemId;
                                db.Images.Add(image);
                                db.SaveChanges();

                            }
                        }
                        result = true;
                    }
                }
                catch (Exception)
                {
                }
            
            return Json(result);
        }
        [CustomAuthorize("Admin", "Sales")]
        public ActionResult DeleteItem(int Id)
        {
            bool result = false;
            try
            {
                using(FreshChoiceEntities db = new FreshChoiceEntities())
                {
                    Item item = db.Items.Where(w => w.ItemId == Id).FirstOrDefault();
                    if (item != null)
                    {
                        item.ItemIsDeleted = true;
                        db.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return Json(result);
        }
        [CustomAuthorize("Admin")]
        public ActionResult Orders()
        {
            List<AllOrderItem> orders = new List<AllOrderItem>();
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                var newOrders = db.Orders
                    .Join(
                        db.OrderStatus,
                        o => o.OrderStatusId,
                        os => os.OrderStatusId,
                        (o, os) => new {order = o, orderStatus = os}
                    )
                    .Join(
                        db.Carts,
                        ows => ows.order.CartId,
                        c => c.CartId,
                        (ows, c) => new { order = ows.order, orderStatus = ows.orderStatus, cart = c }
                    )
                    .OrderByDescending(o => o.order.OrderUpdatedDate)
                    .ToList();

                foreach(var order in newOrders)
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
                            (ci, i) => new {cartItem = ci, item = i})
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
            return View(orders);
        }
        [CustomAuthorize("Admin", "Sales")]
        public ActionResult Sales()
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
                    .Where(w => w.orderStatus.OrderStatusOrder == OrderProcessingStatus.ONSALES 
                            || w.orderStatus.OrderStatusOrder == OrderProcessingStatus.PLACED)
                    .OrderBy(o=> o.order.OrderUpdatedDate)
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
            return View(orders);
        }
        [CustomAuthorize("Admin", "Delivery")]
        public ActionResult Delivery()
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
                    .Where(w => w.orderStatus.OrderStatusOrder == OrderProcessingStatus.COMPLETED)
                    .OrderBy(o => o.order.OrderUpdatedDate)
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
            return View(orders);
        }
        [CustomAuthorize("Admin", "Order Manager")]
        public ActionResult Counter()
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
                    .Where(w => w.orderStatus.OrderStatusOrder == OrderProcessingStatus.READY)
                    .OrderBy(o => o.order.OrderUpdatedDate)
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
            return View(orders);
        }
        [CustomAuthorize("Admin", "Order Manager", "Sales", "Delivery")]
        public ActionResult UpdateOrderStatus(int Id, int StatusOrder, DateTime NextDeadline)
        {
            try
            {
                using(FreshChoiceEntities db = new FreshChoiceEntities())
                {
                    Order order = db.Orders.Where(w => w.OrderId == Id).FirstOrDefault();
                    if (order != null)
                    {
                        if (StatusOrder == OrderProcessingStatus.COMPLETED)
                        {
                            // assign delivery person
                            var user = db.Users
                                .Join(
                                    db.Roles,
                                    u => u.RoleId,
                                    r => r.RoleId,
                                    (u, r) => new { User = u, Role = r })
                                .Where(w => w.Role.RoleId == RoleTypes.DELIVERY)
                                .FirstOrDefault();
                            if (user != null)
                            {
                                Delivery delivery = db.Deliveries
                                    .Where(w => w.DeliveryId == order.DeliveryId)
                                    .FirstOrDefault();
                                delivery.UserId = user.User.UserId;
                                db.SaveChanges();
                            }
                            else
                            {
                                return Json(false);
                            }
                        }

                        order.OrderStatusId = db
                            .OrderStatus.Where(w => w.OrderStatusOrder == StatusOrder)
                            .Select(s => s.OrderStatusId).FirstOrDefault();
                        order.OrderNextDeadline = NextDeadline;
                        order.OrderUpdatedDate = DateTime.Now;
                        db.SaveChanges();

                        return Json(true);
                    }
                }
            }
            catch (Exception)
            {

            }
            return Json(false);
        }
        [CustomAuthorize("Admin", "Sales","User")]
        public ActionResult CancelOrder(int Id)
        {
            if (Id == 0)
            {
                return Json(false);
            }
            try
            {
                int userId = int.Parse(Convert.ToString(Session["UserId"]));
                CartHelper.GetInstance(userId).CancelOrder(Id);
                return Json(true);
            }
            catch (Exception) { }
            return Json(false);
        }
    }
}