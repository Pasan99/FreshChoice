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
    
    public partial class CartItem
    {
        public int CartItemId { get; set; }
        public int CartItemQnt { get; set; }
        public double CartItemTotalPrice { get; set; }
        public int ItemId { get; set; }
        public int CartId { get; set; }
    
        public virtual Cart Cart { get; set; }
        public virtual Item Item { get; set; }
    }
}
