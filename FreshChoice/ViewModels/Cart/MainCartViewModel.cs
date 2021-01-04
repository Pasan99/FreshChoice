using FreshChoice.Models;
using FreshChoice.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Cart
{
    public class MainCartViewModel
    {
        public List<OrderCartItem> CartItems { get; set; }
        public double SubTotal { get; set; }
        public double DeliveryFee { get; set; }
        public double DistanceInKiloMeters { get; set; }
        public double Total { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
    }
}