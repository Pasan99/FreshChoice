using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Home
{
    public class IndexItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string ItemImageUrl { get; set; }
        public string ItemDescription { get; set; }
        public string ItemBrand { get; set; }
        public string ItemCategory { get; set; }
        public int InCartCount { get; set; }
    }
}