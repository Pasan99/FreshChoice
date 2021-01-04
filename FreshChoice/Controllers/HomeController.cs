using FreshChoice.Models;
using FreshChoice.Utilities;
using FreshChoice.ViewModels.Admin;
using FreshChoice.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreshChoice.Controllers
{
    // Pasan Branch
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int userId = 3;
            List<OrderCartItem> cartItems = CartHelper.GetInstance(userId).GetOrderCartItems();
            IndexViewModel viewModel = new IndexViewModel();
            if (cartItems != null && cartItems.Count > 0)
            {
                viewModel.InCartCount = cartItems.Count;
            }
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                var items = db.Items.ToList();
                viewModel.Items = new List<IndexItem>();
                foreach(var item in items)
                {
                    IndexItem orderItem = new IndexItem();
                    orderItem.ItemName = item.ItemName;
                    orderItem.ItemDescription = item.ItemDescription;
                    orderItem.ItemId = item.ItemId;
                    orderItem.ItemPrice = item.ItemPrice;
                    orderItem.ItemBrand = item.Brand.BrandName;
                    orderItem.ItemCategory = item.Brand.Category.CategoryName;
                    orderItem.ItemImageUrl = item.Images.First().ImageUrl;

                    if (cartItems != null && cartItems.Count > 0)
                    {
                        var existing = cartItems.Where(w => w.ItemId == orderItem.ItemId).FirstOrDefault();
                        if(existing != null)
                        {
                            orderItem.InCartCount = existing.Quantity;
                        }
                    }

                    viewModel.Items.Add(orderItem);
                }
            }
            return View(viewModel);
        }

        public ActionResult Products()
        {
            int userId = 3;
            List<OrderCartItem> cartItems = CartHelper.GetInstance(userId).GetOrderCartItems();
            IndexViewModel viewModel = new IndexViewModel();
            if (cartItems != null && cartItems.Count > 0)
            {
                viewModel.InCartCount = cartItems.Count;
            }
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                var items = db.Items.ToList();
                viewModel.Items = new List<IndexItem>();
                foreach (var item in items)
                {
                    IndexItem orderItem = new IndexItem();
                    orderItem.ItemName = item.ItemName;
                    orderItem.ItemDescription = item.ItemDescription;
                    orderItem.ItemId = item.ItemId;
                    orderItem.ItemPrice = item.ItemPrice;
                    orderItem.ItemBrand = item.Brand.BrandName;
                    orderItem.ItemCategory = item.Brand.Category.CategoryName;
                    orderItem.ItemImageUrl = item.Images.First().ImageUrl;

                    if (cartItems != null && cartItems.Count > 0)
                    {
                        var existing = cartItems.Where(w => w.ItemId == orderItem.ItemId).FirstOrDefault();
                        if (existing != null)
                        {
                            orderItem.InCartCount = existing.Quantity;
                        }
                    }

                    viewModel.Items.Add(orderItem);
                }
            }
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Login()
        {
            ViewBag.Message = "Login .";

            return View();
        }
    }
}