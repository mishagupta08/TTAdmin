using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTGarmentAdmin.Models
{
    /// <summary>
    /// hold dashboard model
    /// </summary>
    public class DashboardModel
    {
        public IList<RetailerMaster> RetailerList { get; set; }

        public IList<R_OrderMaster> OrderList { get; set; }

        public IList<string> StatusList { get; set; }

        public IList<R_PointsLedger> PointsLedger { get; set; }

        public RetailerMaster RetailerDetail { get; set; }

        public DashboardCounts DashboardCountsDetail { get; set; }

        public List<string> DistributerCityList { get; set; }

        public List<R_DistributerMaster> DistributerList { get; set; }

        public List<R_StateMaster> StateList { get; set; }

        public List<R_CityMaster> CityList { get; set; }

        public List<R_ProductMaster> ProductList { get; set; }

        public List<R_UploadedMedia> MediaList { get; set; }

        public List<R_Promotion> PromotionList { get; set; }

        public R_ProductMaster ProductDetail { get; set; }

        public R_Promotion PromotionDetail { get; set; }

        public R_OrderMaster OrderDetail { get; set; }

        public IList<R_MediaCategory> MediaCategoryList { get; set; }

        public IList<PromotionEntryDetail> RetailerListByPromoId { get; set; }

        public IList<R_PromotionEntries> RetailerUploadedImagesList { get; set; }

        public IList<R_BannerMaster> BannerList { get; set; }

        public R_BannerMaster BannerDetail { get; set; }

        public R_MediaCategory MediaCategory { get; set; }

        public R_UploadedMedia MediaDetail { get; set; }

        public R_MessageMaster MessageDetail { get; set; }

        public IList<R_MessageMaster> MessagesList { get; set; }

        public IList<R_NotificationManager> NotificationList { get; set; }

        public R_NotificationManager NotificationDetail { get; set; }

        public R_AppVersion VersionDetail { get; set; }

        public IList<R_AppVersion> VersionList { get; set; }

        public R_FestivePointMaster FestivePointDetail { get; set; }

        public IList<R_FestivePointMaster> FestivePointList { get; set; }

        public string Error { get; set; }

        public Filters FilterDetail { get; set; }

        public string TableString { get; set; }

        public IList<SelectListItem> BlockOption { get; set; }

        public void AssignStatusList()
        {
            this.StatusList = new List<string>();
            this.StatusList.Add(Status.Pending.ToString());
            this.StatusList.Add(Status.Confirm.ToString());
            this.StatusList.Add(Status.Delivered.ToString());
            this.StatusList.Add(Status.Rejected.ToString());
            this.StatusList.Add(Status.OnHold.ToString());
        }

        public void AssignBlockOptionList()
        {
            this.BlockOption = new List<SelectListItem>();
            this.BlockOption.Add(new SelectListItem {Text="Blocked",Value="Y"});
            this.BlockOption.Add(new SelectListItem { Text = "Active", Value = "N" });
        }
    }
}