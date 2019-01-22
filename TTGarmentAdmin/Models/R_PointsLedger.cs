using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_PointsLedger
    {
        public string Id { get; set; }
        public Nullable<decimal> BarcodeSno { get; set; }
        public string Barcode { get; set; }
        public string FirmName { get; set; }
        public string RetailerId { get; set; }
        public string EarnSpentDateString { get; set; }
        public Nullable<System.DateTime> EarnSpentDate { get; set; }
        public string LocationX { get; set; }
        public string LocationY { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string LocationLink { get; set; }
        public Nullable<decimal> DabitPoints { get; set; }
        public Nullable<decimal> CreditPoints { get; set; }
    }
}