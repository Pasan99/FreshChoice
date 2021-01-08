using FreshChoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.ViewModels.Admin
{
    public class AllOrderItem
    {
        public int OrderId { get; set; }
        public string BillNo { get; set; }
        public string ConfirmationNo { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime NextDeadline { get; set; }
        public string OrderStatus { get; set; }
        public int OrderStatusId { get; set; }
        public Models.Cart Cart { get; set; }
        public double OrderAmount { get; set; }
        public List<OrderCartItem> Items { get; set; }
        //public AllOrderItem(int OrderId, string BillNo, string ConfirmationNo, DateTime LastUpdate, DateTime NextDeadline, string OrderStatus, Cart Cart, double OrderAmount)
        //{
        //    this.OrderId = OrderId;
        //    this.BillNo = BillNo;
        //    this.ConfirmationNo = ConfirmationNo;
        //    this.LastUpdate = LastUpdate;
        //    this.NextDeadline = NextDeadline;
        //    this.OrderStatus = OrderStatus;
        //    this.Cart = Cart;
        //    this.OrderAmount = OrderAmount;
        //}

    }
    public class OrderCartItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string ItemImageUrl { get; set; }
        public string ItemDescription { get; set; }
        public string ItemBrand { get; set; }
        public string ItemCategory { get; set; }
        //public OrderCartItem(int ItemId, int Quantity, string ItemName, double ItemPrice, string ItemImageUrl)
        //{
        //    this.ItemId = ItemId;
        //    this.Quantity = Quantity;
        //    this.ItemName = ItemName;
        //    this.ItemPrice = ItemPrice;
        //    this.ItemImageUrl = ItemImageUrl;
        //}
    }
}