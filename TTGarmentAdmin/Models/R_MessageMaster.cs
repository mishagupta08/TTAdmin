using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public partial class R_MessageMaster
    {
        public string Id { get; set; }
        public Nullable<System.DateTime> MessagePublishDate { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<System.DateTime> PublishFrom { get; set; }
        public Nullable<System.DateTime> PublishTo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string DateString { get; set; }
        public string PublishFromDate { get; set; }
        public string PublishToDate { get; set; }
    }
}