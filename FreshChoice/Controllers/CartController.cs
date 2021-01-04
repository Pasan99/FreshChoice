using FreshChoice.Models;
using FreshChoice.Utilities;
using FreshChoice.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreshChoice.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            int userId = 3;
            return View(GetCartViewModel(userId));
        }
        public JsonResult AddItem(int Id)
        {
            int userId = 3;
            try
            {
                using (FreshChoiceEntities db = new FreshChoiceEntities())
                {
                    if (Id > 0)
                    {
                        CartHelper.GetInstance(userId).AddItemToCart(Id);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            
            return Json(GetCartViewModel(userId));
        }
        public JsonResult RemoveItem(int Id)
        {
            int userId = 3;
            try
            {
                using (FreshChoiceEntities db = new FreshChoiceEntities())
                {
                    if (Id > 0)
                    {
                        CartHelper.GetInstance(userId).RemoveItemFromCart(Id);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return Json(GetCartViewModel(userId));
        }
        public ActionResult PlaceOrder(bool IsPickup)
        {
            int userId = 3;
            string deliveryType = IsPickup ? DeliveryTypes.PICKUP : DeliveryTypes.HOME_DELIVERY;
            return View(CartHelper.GetInstance(userId).PlaceOrder(deliveryType));
        }
        private MainCartViewModel GetCartViewModel(int UserId)
        {
            MainCartViewModel viewModel = new MainCartViewModel();
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                viewModel.CartItems = CartHelper.GetInstance(UserId).GetOrderCartItems();

                double total = 0;
                foreach(var item in viewModel.CartItems)
                {
                    total += (item.ItemPrice * item.Quantity);
                }
                viewModel.SubTotal = total;
                viewModel.DeliveryFee = CartHelper.GetInstance(UserId).GetDeliveryFees();
                viewModel.Total = viewModel.SubTotal + viewModel.DeliveryFee;
                viewModel.DistanceInKiloMeters = CartHelper.GetInstance(UserId).GetDeliveryInKilometers();
                Address address = db.Addresses.Where(w => w.UserId == UserId).FirstOrDefault();
                User user = db.Users.Where(w => w.UserId == UserId).FirstOrDefault();
                if (address != null)
                {
                    viewModel.Address = address.AddressDescription;
                }
                viewModel.UserName = user.UserName;
                viewModel.UserPhone = user.UserContact;
            }
            return viewModel;
        }
    }
}