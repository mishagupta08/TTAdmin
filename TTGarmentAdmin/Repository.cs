using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TTGarmentAdmin.Models;
using System.IO.Compression;
using System.IO;

namespace TTGarmentAdmin
{
    public class Repository
    {
        private string ApiUrl = "http://ttapi.bisplindia.in/api/Home/";

        //private string ApiUrl = "http://localhost:59515/api/Home/";

        private string LoginAdminAction = "LoginAdminUser";

        private string ManageRetailerListAction = "ManageRetailer/List";

        private string ManageProductsListAction = "ManageProducts/FullList";

        private string ManageProductAddAction = "ManageProducts/Add";

        private string ManageProductEditAction = "ManageProducts/Edit";

        private string GetProductByIdAction = "ManageProducts/ById";

        private string ManagePointsLedgerAction = "ManagePoints/List";

        private string ManagePointsLedgerFilterAction = "ManagePointsWithFilter";

        private string ManageRetailerWithFilterAction = "ManageRetailerWithFilter";

        private string ManageRetailerByIdAction = "ManageRetailer/ById";

        private string ManageProductsStatusByIdAction = "ManageProducts/UpdateStatus";

        private string ManagePromotonListAction = "ManagePromotion/FullList";

        private string GetPromotionByIdAction = "ManagePromotion/ById";

        private string AddPromotionAction = "ManagePromotion/Add";

        private string EditPromotionAction = "ManagePromotion/Edit";

        private string ManagePromotionStatusByIdAction = "ManagePromotion/UpdateStatus";

        private string ManageRetailerEditAction = "ManageRetailer/Edit";

        private string SaveImageAction = "SaveImage";

        private string UpdateStatusAction = "UpdateStatus";

        private string EncryptBarcodeAction = "EncryptBarcodes";

        private string GetDistributerCityListAction = "GetDistributerCityList";

        private string GetDistributerListAction = "GetDistributerByCityName/";

        private string StateListAction = "StateList";

        private string GetDashboardCountsAction = "GetDashboardCounts";

        private string CityListAction = "CityList/";

        private string ManageOrderListAction = "ManageOrder/List";

        private string ManageMediaAddAction = "ManageMediaCategory/Add";

        private string ManageMediaEditAction = "ManageMediaCategory/Edit";

        private string ManageUploadedMediaAddAction = "ManageUploadedMedia/Add";

        private string ManageUploadedMediaEditAction = "ManageUploadedMedia/Edit";

        private string ManageMediaDeleteAction = "ManageMediaCategory/Delete";

        private string ManageMediaListAction = "ManageMediaCategory/FullList";

        private string ManageMediaActiveListAction = "ManageMediaCategory/List";

        private string ManageUploadedMediaListAction = "ManageUploadedMedia/FullList";

        private string ManageMediaByIdAction = "ManageMediaCategory/ById";

        private string ManageMediaStatusByIdAction = "ManageMediaCategory/UpdateStatus";

        private string ManageUploadedMediaByIdAction = "ManageUploadedMedia/ById";

        private string ManageUploadedMediaStatusAction = "ManageUploadedMedia/UpdateStatus";

        private string ListRetailerByPromotionAction = "ManagePromotionEntry/ListRetailerByPromotion";

        private string UpdatePromotionEntriesAction = "ManagePromotionEntry/";

        private string ManageRetailerImagesAction = "ManagePromotionEntry/RetailerImages";

        private string UpdateRetailerImageStausAction = "ManagePromotionEntry/UpdateRetailerImageStatus";

        private string ManageNotificationManagerAction = "ManageNotification";

        private string ManageBannerAction = "ManageBanner/";

        private string ManageMessageDetailAction = "ManageMessages/";

        private string ManageFestivePointAction = "ManageFetivePoints/";

        private string ManageAppVersionAction = "ManageAppVersion/";

        public async Task<AdminMaster> AdminLogin(LoginViewModel loginModel)
        {
            var loginDetail = "{\"Username\":\"UsernameValue\",\"Password\":\"PasswordValue\"}";
            loginDetail = loginDetail.Replace("UsernameValue", loginModel.username).Replace("PasswordValue", loginModel.password);

            var result = await CallPostFunction(loginDetail, LoginAdminAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var memberDetail = JsonConvert.DeserializeObject<AdminMaster>(result.ResponseValue);
                return memberDetail;
            }
        }

        public async Task<List<R_ProductMaster>> GetProductList()
        {
            var result = await CallPostFunction(string.Empty, ManageProductsListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var memberDetail = JsonConvert.DeserializeObject<List<R_ProductMaster>>(result.ResponseValue);
                return memberDetail;
            }
        }

        public async Task<List<R_Promotion>> GetPromotionList()
        {
            var result = await CallPostFunction(string.Empty, ManagePromotonListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var promoList = JsonConvert.DeserializeObject<List<R_Promotion>>(result.ResponseValue);
                return promoList;
            }
        }

        public async Task<List<R_OrderMaster>> GetOrderList()
        {
            var result = await CallPostFunction(string.Empty, ManageOrderListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var orders = JsonConvert.DeserializeObject<List<R_OrderMaster>>(result.ResponseValue);
                return orders;
            }
        }

        public async Task<List<RetailerMaster>> GetRegisteredRetailer()
        {
            var result = await CallPostFunction(string.Empty, ManageRetailerListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                result.ResponseValue = Decompress(result.ResponseValue);
                var memberDetail = JsonConvert.DeserializeObject<List<RetailerMaster>>(result.ResponseValue);
                return memberDetail;
            }
        }

        public async Task<List<R_StateMaster>> GetStateList()
        {
            var result = await CallPostFunction(string.Empty, StateListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var states = JsonConvert.DeserializeObject<List<R_StateMaster>>(result.ResponseValue);
                return states;
            }
        }

        public async Task<List<R_CityMaster>> GetCityList(string cityId)
        {
            var result = await CallPostFunction(string.Empty, CityListAction + cityId);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var states = JsonConvert.DeserializeObject<List<R_CityMaster>>(result.ResponseValue);
                return states;
            }
        }

        public async Task<List<R_PointsLedger>> GetPointsLedger()
        {
            var result = await CallPostFunction(string.Empty, ManagePointsLedgerAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var memberDetail = JsonConvert.DeserializeObject<List<R_PointsLedger>>(result.ResponseValue);
                return memberDetail;
            }
        }

        public async Task<List<RetailerMaster>> GetRetailerWithFilters(Filters filter)
        {
            var detail = JsonConvert.SerializeObject(filter);
            var result = await CallPostFunction(detail, ManageRetailerWithFilterAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var memberDetail = JsonConvert.DeserializeObject<List<RetailerMaster>>(result.ResponseValue);
                return memberDetail;
            }
        }

        public async Task<List<R_PointsLedger>> GetPointsLedgerWithFilters(Filters filter)
        {
            var detail = JsonConvert.SerializeObject(filter);
            var result = await CallPostFunction(detail, ManagePointsLedgerFilterAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var memberDetail = JsonConvert.DeserializeObject<List<R_PointsLedger>>(result.ResponseValue);
                return memberDetail;
            }
        }

        public async Task<string> EncryptBarcode()
        {
            var result = await CallGetFunction(EncryptBarcodeAction);
            if (result == null)
            {
                return "Something went wrong.";
            }
            else
            {
                return result.ResponseValue;
            }
        }

        public async Task<DashboardCounts> GetDashboardCount()
        {
            var result = await CallGetFunction(GetDashboardCountsAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                var count = JsonConvert.DeserializeObject<DashboardCounts>(result.ResponseValue);
                return count;
            }
        }

        public async Task<R_Promotion> PromotionByIdView(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);

            var result = await CallPostFunction(detail, GetPromotionByIdAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                var product = JsonConvert.DeserializeObject<R_Promotion>(result.ResponseValue);
                return product;
            }
        }

        public async Task<R_ProductMaster> ProductByIdView(string Id)
        {
            var prod = new R_ProductMaster();
            prod.Id = Id;
            var list = new List<R_ProductMaster>();
            list.Add(prod);
            var detail = JsonConvert.SerializeObject(list);

            var result = await CallPostFunction(detail, GetProductByIdAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                var product = JsonConvert.DeserializeObject<R_ProductMaster>(result.ResponseValue);
                return product;
            }
        }

        public async Task<RetailerMaster> GetRetailerDetail(string Id)
        {
            var detail = "{\"ID\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);
            var result = await CallPostFunction(detail, ManageRetailerByIdAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                var memberDetail = JsonConvert.DeserializeObject<RetailerMaster>(result.ResponseValue);
                return memberDetail;
            }
        }

        public async Task<string> SaveRetailerDetail(RetailerMaster retailerDetail)
        {
            string parameter = JsonConvert.SerializeObject(retailerDetail);
            var result = await CallPostFunction(parameter, ManageRetailerEditAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result.ResponseValue;
            }
        }

        public async Task<List<string>> GetDistributerCityList()
        {
            var result = await CallGetFunction(GetDistributerCityListAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                var cityList = JsonConvert.DeserializeObject<List<string>>(result.ResponseValue);
                return cityList;
            }
        }

        public async Task<List<string>> GetDistributerList(string cityName)
        {
            var result = await CallGetFunction(GetDistributerListAction + cityName);
            if (result == null)
            {
                return null;
            }
            else
            {
                var cityList = JsonConvert.DeserializeObject<List<string>>(result.ResponseValue);
                return cityList;
            }
        }

        public async Task<Response> AddBulkProduct(List<R_ProductMaster> products)
        {
            var productData = JsonConvert.SerializeObject(products);
            var result = await CallPostFunction(productData, ManageProductAddAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<Response> SaveImage(ProfilePhoto imageDetail)
        {
            var detail = JsonConvert.SerializeObject(imageDetail);
            var result = await CallPostFunction(detail, SaveImageAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<Response> UpdateStatus(R_OrderMaster statusDetail)
        {
            var detail = JsonConvert.SerializeObject(statusDetail);
            var result = await CallPostFunction(detail, UpdateStatusAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<Response> UpdatePromotionStatus(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);


            var result = await CallPostFunction(detail, ManagePromotionStatusByIdAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<Response> UpdateProductStatus(List<R_ProductMaster> products)
        {
            var productData = JsonConvert.SerializeObject(products);
            var result = await CallPostFunction(productData, ManageProductsStatusByIdAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<Response> SavePromotionDetail(R_Promotion promotion)
        {
            var productData = JsonConvert.SerializeObject(promotion);
            string action = string.IsNullOrEmpty(promotion.Id) ? AddPromotionAction : EditPromotionAction;
            var result = await CallPostFunction(productData, action);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<Response> EditProduct(List<R_ProductMaster> products)
        {
            var productData = JsonConvert.SerializeObject(products);
            var result = await CallPostFunction(productData, ManageProductEditAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /**********Manage notification functions start**********/

        public async Task<Response> AddAndSendNotification(R_NotificationManager detail)
        {
            var notificationData = JsonConvert.SerializeObject(detail);
            string action = ManageNotificationManagerAction + "/Add";
            var result = await CallPostFunction(notificationData, action);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<R_NotificationManager>> ListNotification()
        {
            var result = await CallPostFunction(string.Empty, ManageNotificationManagerAction + "/List");
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var notificationList = JsonConvert.DeserializeObject<List<R_NotificationManager>>(result.ResponseValue);
                return notificationList;
            }
        }

        public async Task<Response> DeleteNotification(string Id)
        {
            string action = ManageNotificationManagerAction + "/Delete";
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);

            var result = await CallPostFunction(detail, action);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /**********Manage notification functions End**********/

        /******Media Category Functions* Start*****/

        public async Task<Response> AddEditMediaCategoryDetail(R_MediaCategory category)
        {
            var categoryData = JsonConvert.SerializeObject(category);
            string action = string.IsNullOrEmpty(category.Id) ? ManageMediaAddAction : ManageMediaEditAction;
            var result = await CallPostFunction(categoryData, action);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<R_MediaCategory>> ListMediaCategory()
        {
            var result = await CallPostFunction(string.Empty, ManageMediaListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var categoryList = JsonConvert.DeserializeObject<List<R_MediaCategory>>(result.ResponseValue);
                return categoryList;
            }
        }

        public async Task<List<R_MediaCategory>> ActiveListMediaCategory()
        {
            var result = await CallPostFunction(string.Empty, ManageMediaActiveListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var categoryList = JsonConvert.DeserializeObject<List<R_MediaCategory>>(result.ResponseValue);
                return categoryList;
            }
        }

        public async Task<R_MediaCategory> GetMediaCategoryById(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);
            var result = await CallPostFunction(detail, ManageMediaByIdAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                var media = JsonConvert.DeserializeObject<R_MediaCategory>(result.ResponseValue);
                return media;
            }
        }

        public async Task<Response> UpdateMediaCategoryStatus(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);


            var result = await CallPostFunction(detail, ManageMediaStatusByIdAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /******Media Category Functions* End*****/


        /******upload media gallary Functions* Start*****/

        public async Task<Response> AddEditGallaryMediaDetail(R_UploadedMedia gallaryDetail)
        {
            var gallaryData = JsonConvert.SerializeObject(gallaryDetail);
            string action = string.IsNullOrEmpty(gallaryDetail.Id) ? ManageUploadedMediaAddAction : ManageUploadedMediaEditAction;
            var result = await CallPostFunction(gallaryData, action);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<R_UploadedMedia>> ListUploadedMedia()
        {
            var result = await CallPostFunction(string.Empty, ManageUploadedMediaListAction);
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var categoryList = JsonConvert.DeserializeObject<List<R_UploadedMedia>>(result.ResponseValue);
                return categoryList;
            }
        }

        public async Task<R_UploadedMedia> GetUploadedMediaById(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);
            var result = await CallPostFunction(detail, ManageUploadedMediaByIdAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                var media = JsonConvert.DeserializeObject<R_UploadedMedia>(result.ResponseValue);
                return media;
            }
        }

        public async Task<Response> UpdateUploadedMediaStatus(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);


            var result = await CallPostFunction(detail, ManageUploadedMediaStatusAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /******upload media gallary Functions* End*****/

        /******Manage Promotion Entry Functions* Start*****/

        public async Task<List<PromotionEntryDetail>> ListRetailerListByPromotionId(string Id)
        {
            var detail = "{\"PromoId\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);
            var result = await CallPostFunction(detail, ListRetailerByPromotionAction);
            if (result == null || result.Status == false)
            {
                return null;
            }
            else
            {
                var media = JsonConvert.DeserializeObject<List<PromotionEntryDetail>>(result.ResponseValue);
                return media;
            }
        }

        public async Task<Response> UpdatePromotionEntryStatus(string promoId, string retailerId, bool action)
        {
            var det = new PromotionEntryDetail();
            det.PromoId = promoId;
            det.RetailerId = retailerId;
            var detail = JsonConvert.SerializeObject(det);

            var result = await CallPostFunction(detail, UpdatePromotionEntriesAction + (action == true ? "Approve" : "Reject"));
            return result;
        }

        public async Task<List<R_PromotionEntries>> ListRetailerImages(string promoId, string retailerId)
        {
            var det = new PromotionEntryDetail();
            det.PromoId = promoId;
            det.RetailerId = retailerId;
            var detail = JsonConvert.SerializeObject(det);
            var result = await CallPostFunction(detail, ManageRetailerImagesAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                var media = JsonConvert.DeserializeObject<List<R_PromotionEntries>>(result.ResponseValue);
                return media;
            }
        }

        public async Task<Response> UpdateRetailerImageStatus(string Id, bool status)
        {
            var data = new PromotionEntryDetail();
            data.PromoId = Id;
            data.IsApproved = status;
            var detail = JsonConvert.SerializeObject(data);
            var result = await CallPostFunction(detail, UpdateRetailerImageStausAction);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /******Manage Promotion Entry Functions* End*****/

        /******Banner Functions* Start*****/

        public async Task<Response> AddEditBannerDetail(R_BannerMaster bannerDetail)
        {
            var bannerData = JsonConvert.SerializeObject(bannerDetail);
            string action = ManageBannerAction + (string.IsNullOrEmpty(bannerDetail.Id) ? "Add" : "Edit");
            var result = await CallPostFunction(bannerData, action);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<R_BannerMaster>> ListBanners()
        {
            var result = await CallPostFunction(string.Empty, ManageBannerAction + "FullList");
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var bannerList = JsonConvert.DeserializeObject<List<R_BannerMaster>>(result.ResponseValue);
                return bannerList;
            }
        }

        public async Task<R_BannerMaster> GetBannerById(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);
            var result = await CallPostFunction(detail, ManageBannerAction + "ById");
            if (result == null)
            {
                return null;
            }
            else
            {
                var banner = JsonConvert.DeserializeObject<R_BannerMaster>(result.ResponseValue);
                return banner;
            }
        }

        public async Task<Response> UpdateBannerStatus(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);


            var result = await CallPostFunction(detail, ManageBannerAction + "UpdateStatus");
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /******Banner Functions* End*****/

        /******Message Detail Functions* Start*****/

        public async Task<Response> AddEditMessageDetail(R_MessageMaster messageDetail)
        {
            var data = JsonConvert.SerializeObject(messageDetail);
            string action = ManageMessageDetailAction + (string.IsNullOrEmpty(messageDetail.Id) ? "Add" : "Edit");
            var result = await CallPostFunction(data, action);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<R_MessageMaster>> ListMessages()
        {
            var result = await CallPostFunction(string.Empty, ManageMessageDetailAction + "FullList");
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var list = JsonConvert.DeserializeObject<List<R_MessageMaster>>(result.ResponseValue);
                return list;
            }
        }

        public async Task<R_MessageMaster> GetMessageById(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);
            var result = await CallPostFunction(detail, ManageMessageDetailAction + "ById");
            if (result == null)
            {
                return null;
            }
            else
            {
                var banner = JsonConvert.DeserializeObject<R_MessageMaster>(result.ResponseValue);
                return banner;
            }
        }

        public async Task<Response> UpdateMessageStatus(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);


            var result = await CallPostFunction(detail, ManageMessageDetailAction + "UpdateStatus");
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /******Message Detail Functions* End*****/

        /******Festive points Functions* Start*****/

        public async Task<Response> AddEditFestivePointDetail(R_FestivePointMaster pointDetail)
        {
            var pointData = JsonConvert.SerializeObject(pointDetail);
            string action = ManageFestivePointAction + (string.IsNullOrEmpty(pointDetail.Id) ? "Add" : "Edit");
            var result = await CallPostFunction(pointData, action);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<R_FestivePointMaster>> ListFestivePoints()
        {
            var result = await CallPostFunction(string.Empty, ManageFestivePointAction + "List");
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var festivePointList = JsonConvert.DeserializeObject<List<R_FestivePointMaster>>(result.ResponseValue);
                return festivePointList;
            }
        }

        public async Task<R_FestivePointMaster> GetFestivePointById(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);
            var result = await CallPostFunction(detail, ManageFestivePointAction + "ById");
            if (result == null)
            {
                return null;
            }
            else
            {
                var data = JsonConvert.DeserializeObject<R_FestivePointMaster>(result.ResponseValue);
                return data;
            }
        }

        public async Task<Response> UpdateFestivePointStatus(string Id)
        {
            var detail = "{\"Id\":\"IDValue\"}";
            detail = detail.Replace("IDValue", Id);


            var result = await CallPostFunction(detail, ManageFestivePointAction + "UpdateStatus");
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /******Festive Points Functions* End*****/

        /******App Version Functions* Start*****/

        public async Task<Response> AddAppVersionDetail(R_AppVersion versionDetail)
        {
            var appVersion = JsonConvert.SerializeObject(versionDetail);
            var result = await CallPostFunction(appVersion, ManageAppVersionAction + "Add");
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<R_AppVersion>> ListAppVersions()
        {
            var result = await CallPostFunction(string.Empty, ManageAppVersionAction + "List");
            if (result == null || !result.Status)
            {
                return null;
            }
            else
            {
                var bannerList = JsonConvert.DeserializeObject<List<R_AppVersion>>(result.ResponseValue);
                return bannerList;
            }
        }

        /******App Version Functions* End*****/

        private async Task<Response> CallPostFunction(string detail, string action)
        {
            using (var httpClient = new HttpClient())
            {
                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var httpContent = new StringContent(detail, Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await httpClient.PostAsync(ApiUrl + action, httpContent);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Response>(responseContent);

                    return result;
                }
            }

            return null;
        }

        private async Task<Response> CallGetFunction(string action)
        {
            using (var httpClient = new HttpClient())
            {
                // Do the actual request and await the response
                var httpResponse = await httpClient.GetAsync(ApiUrl + action);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Response>(responseContent);

                    return result;
                }
            }

            return null;
        }

        public static string Decompress(string s)
        {
            var bytes = Convert.FromBase64String(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }
    }
}