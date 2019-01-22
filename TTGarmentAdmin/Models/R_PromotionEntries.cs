using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_PromotionEntries
    {
        public string Id { get; set; }
        public string PromotionId { get; set; }
        public string RetailerId { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<System.DateTime> UpoadedDate { get; set; }
        public Nullable<bool> IsValid { get; set; }
    }
}