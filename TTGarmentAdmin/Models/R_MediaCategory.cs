using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_MediaCategory
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}