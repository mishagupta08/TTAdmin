using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_NotificationResult
    {
        public string Id { get; set; }
        public string NotificationId { get; set; }
        public decimal FailMessageCount { get; set; }
        public decimal SuccessMessageCount { get; set; }
        public decimal NotificationIdCount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }
}