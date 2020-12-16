//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FreshChoice.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int OrderId { get; set; }
        public string OrderDescription { get; set; }
        public string OrderBillNo { get; set; }
        public string OrderConfirmationNo { get; set; }
        public System.DateTime OrderUpdatedDate { get; set; }
        public System.DateTime OrderNextDeadline { get; set; }
        public Nullable<int> DeliveryId { get; set; }
        public int OrderStatusId { get; set; }
        public int CartId { get; set; }
        public double OrderAmount { get; set; }
    
        public virtual Cart Cart { get; set; }
        public virtual Cart Cart1 { get; set; }
        public virtual OrderStatu OrderStatu { get; set; }
    }
}
