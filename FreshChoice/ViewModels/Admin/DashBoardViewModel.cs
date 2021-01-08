using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Admin
{
    public class DashBoardViewModel
    {
        public List<CategorySalesItem> CategorySalesItems { get; set; }
        public List<double> DataJson { get; set; }
        public double TotalSales { get; set; }
    }
}