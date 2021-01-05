using CustomAuthorizationFilter.Infrastructure;
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
    [CustomAuthenticationFilter]
    public class CartController : Controller
    {
        // GET: Cart
        [CustomAuthorize("User", "Admin")]
        public ActionResult Index()
        {
            int userId = int.Parse(Convert.ToString(Session["UserId"]));
            return View(GetCartViewModel(userId));
        }
        [CustomAuthorize("User", "Admin")]
        public JsonResult AddItem(int Id)
        {
            int userId = int.Parse(Convert.ToString(Session["UserId"]));
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
        [CustomAuthorize("User", "Admin")]
        public JsonResult RemoveItem(int Id)
        {
            int userId = int.Parse(Convert.ToString(Session["UserId"]));
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
        [CustomAuthorize("User", "Admin")]
        public ActionResult PlaceOrder(bool IsPickup)
        {
            int userId = int.Parse(Convert.ToString(Session["UserId"]));
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
                if(viewModel.CartItems != null)
                {
                    foreach (var item in viewModel.CartItems)
                    {
                        total += (item.ItemPrice * item.Quantity);
                    }
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