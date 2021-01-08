using CustomAuthorizationFilter.Infrastructure;
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
    [CustomAuthenticationFilter]
    public class HomeController : Controller
    {
        [CustomAuthorize("User", "Admin")]
        public ActionResult Index()
        {
            int userId = int.Parse(Convert.ToString(Session["UserId"]));
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
                    orderItem.Quantity = item.ItemAvailableQnt;

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
        [CustomAuthorize("User", "Admin")]
        public ActionResult Products(int CategoryId = 0, String q = null, int BrandId = 0)
        {
            int userId = int.Parse(Convert.ToString(Session["UserId"]));
            List<OrderCartItem> cartItems = CartHelper.GetInstance(userId).GetOrderCartItems();
            IndexViewModel viewModel = new IndexViewModel();
            if (cartItems != null && cartItems.Count > 0)
            {
                viewModel.InCartCount = cartItems.Count;
            }
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                List<Item> items = items = db.Items.ToList();
                if (CategoryId != 0)
                {
                    items = items.Where(w=> w.Brand.CategoryId == CategoryId).ToList();
                }
                if (q != null)
                {
                    items = items.Where(w => w.ItemName.ToLower().StartsWith(q.ToLower())).ToList();
                }
                if (BrandId != 0)
                {
                    items = items.Where(w => w.Brand.BrandId == BrandId).ToList();
                }
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
                    orderItem.Quantity = item.ItemAvailableQnt;

                    // Checking whether the item is in cart or not
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
        [CustomAuthorize("User", "Admin")]
        public ActionResult Categories()
        {
            CategoryViewModel viewModel = new CategoryViewModel();
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                viewModel.Categories = db.Categories.ToList();
                viewModel.Brands = db.Brands.ToList();
            }

            return View(viewModel);
        }
        [CustomAuthorize("User", "Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [CustomAuthorize("User", "Admin")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [CustomAuthorize("User", "Admin")]
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetCategories()
        {
            List<Category> categories;
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                categories = db.Categories.ToList();
            }
            return Json(new { categories = categories, JsonRequestBehavior.AllowGet });
        }

    }
}