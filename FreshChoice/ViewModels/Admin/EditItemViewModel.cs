using FreshChoice.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Admin
{
    public class EditItemViewModel
    {
        public Item Item { get; set; }
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Image> Images { get; set; }
        public string CategoriesJson { get; set; }
        public string BrandsJson { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemAvailableQnt { get; set; }
        public float ItemPrice { get; set; }
        public int ItemBrandId { get; set; }
        public List<HttpPostedFile> ItemImages { get; set; }
        public HttpPostedFile ItemImage { get; set; }
    }
}