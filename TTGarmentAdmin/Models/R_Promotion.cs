using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_Promotion
    {
        public string Id { get; set; }
        public string Heading { get; set; }
        public string HeadingText { get; set; }
        public Nullable<int> ImageCount { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Image { get; set; }
        public string ApprovedCount { get; set; }
        public Nullable<int> TotalEntries { get; set; }
        public Nullable<int> Points { get; set; }
    }
}