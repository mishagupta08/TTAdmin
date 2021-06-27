using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_ProductMaster
    {
        public string Action { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Nullable<long> Points { get; set; }
        public Nullable<long> Quantity { get; set; }
        public string ProductCode { get; set; }
        public string Size { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}