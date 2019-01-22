using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_AppVersion
    {
        public string Id { get; set; }
        public Nullable<decimal> Version { get; set; }
        public string DateString { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }
}