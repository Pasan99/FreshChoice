using FreshChoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Home
{
    public class CategoryViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
    }
}