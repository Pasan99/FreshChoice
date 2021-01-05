using FreshChoice.Models;
using FreshChoice.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Home
{
    public class IndexViewModel
    {
        public List<IndexItem> Items { get; set; }
        public int InCartCount { get; set; }
    }
}