using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public partial class R_NotificationManager
    {
        public string Id { get; set; }
        public string Notification { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string ImageUrl { get; set; }
        public bool IsReplyable { get; set; }
        public bool Boradcast { get; set; }
        public Nullable<int> StateCode { get; set; }
        public Nullable<int> CityCode { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Header { get; set; }
        public string DateString { get; set; }
        public string Exception { get; set; }

        public decimal? FailMessageCount { get; set; }
        public decimal? SuccessMessageCount { get; set; }
        public decimal? NotificationIdCount { get; set; }
        public string ResultDate { get; set; }
    }
}