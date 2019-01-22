using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_BannerMaster
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string DateString { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}