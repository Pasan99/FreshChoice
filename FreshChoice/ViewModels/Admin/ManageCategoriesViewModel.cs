using FreshChoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Admin
{
    public class ManageCategoriesViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
    }
}