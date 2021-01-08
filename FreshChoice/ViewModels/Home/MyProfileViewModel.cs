using FreshChoice.Models;
using FreshChoice.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Home
{
    public class MyProfileViewModel
    {
        public List<AllOrderItem> Orders { get; set; }
        public List<Transaction> Transactions { get; set; }
        public Wallet Wallet { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
    }
}