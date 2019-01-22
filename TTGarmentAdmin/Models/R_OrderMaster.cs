using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_OrderMaster
    {
        public string ProductName { get; set; }
        public string Retailer { get; set; }
        public string Address { get; set; }
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string RetailerId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<long> PointsUsed { get; set; }
        public string OrderStatus { get; set; }
        public string OrderNo { get; set; }
        public string DateString { get; set; }
        public Nullable<decimal> Quantity { get; set; }       
        public string City { get; set; }
        public string State { get; set; }
    }
}