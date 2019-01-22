using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class R_UploadedMedia
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string DateString { get; set; }
        public string Url { get; set; }
        public string UploadedBy { get; set; }
        public string FileName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string DisplayName { get; set; }
    }
}