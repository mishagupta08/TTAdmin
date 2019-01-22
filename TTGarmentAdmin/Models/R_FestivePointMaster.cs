using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_FestivePointMaster
    {
        public string Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FromPoint { get; set; }
        public int ToPoint { get; set; }
        public bool IsActive { get; set; }
        public bool IsRegistration { get; set; }
        public string DateString { get; set; }
        public string FromDateString { get; set; }
        public string ToDateString { get; set; }
        public DateTime AddedDate { get; set; }
    }
}