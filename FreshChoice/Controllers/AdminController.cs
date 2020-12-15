using FreshChoice.Models;
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
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Items()
        {
            List<Item> items = new List<Item>();
            using(FreshChoiceEntities db = new FreshChoiceEntities())
            {
                items = db.Items.Where(w=> w.ItemIsDeleted == null || w.ItemIsDeleted != true).ToList();
            }
            return View(items);
        }
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
    }
}