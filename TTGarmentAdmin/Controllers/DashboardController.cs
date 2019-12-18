using GemBox.Spreadsheet;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TTGarmentAdmin.Models;
using TTGarmentAdmin.Properties;

namespace TTGarmentAdmin.Controllers
{
    public class DashboardController : Controller
    {
        private Repository repository;

        private DashboardModel model;

        // GET: Dashboard
        public async Task<ActionResult> Index()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.DashboardCountsDetail = await this.repository.GetDashboardCount();
            if (this.model.DashboardCountsDetail == null)
            {
                this.model.DashboardCountsDetail = new DashboardCounts();
            }

            return View(this.model);
        }

        public async Task<ActionResult> GetRegisteredRetailer(string filterType, string filterValue, string fromDate, string toDate)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.FilterDetail = new Filters();
            this.model.FilterDetail.AssignRetailerFilter();
            this.model.FilterDetail.SelectedFilterName = filterType;
            this.model.FilterDetail.FilterValue = filterValue;
            this.model.FilterDetail.FromDate = fromDate;
            this.model.FilterDetail.ToDate = toDate;

            //if (!string.IsNullOrEmpty(filterValue))
            //{
            //    filterValue = filterValue.Trim().ToLower();
            //}

            if (!(string.IsNullOrEmpty(filterType) && string.IsNullOrEmpty(filterValue) && string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate)))
            {
                this.model.RetailerList = await this.repository.GetRetailerWithFilters(this.model.FilterDetail);
                //if (filterType == "Id")
                //{
                //    this.model.RetailerList = this.model.RetailerList.Where(r => r.ID.ToLower().Contains(filterValue)).ToList();
                //}
                //if (filterType == "Date")
                //{
                //    this.model.RetailerList = this.model.RetailerList.Where(r => (!string.IsNullOrEmpty(r.DateString) && r.DateString.ToLower().Contains(filterValue))).ToList();
                //}
                //if (filterType == "Firm Name")
                //{
                //    this.model.RetailerList = this.model.RetailerList.Where(r => (!string.IsNullOrEmpty(r.FirmName) && r.FirmName.ToLower().Contains(filterValue))).ToList();
                //}
                //if (filterType == "Mobile Number")
                //{
                //    this.model.RetailerList = this.model.RetailerList.Where(r => (!string.IsNullOrEmpty(r.Mobile) && r.Mobile.Contains(filterValue))).ToList();
                //}
                //if (filterType == "City")
                //{
                //    this.model.RetailerList = this.model.RetailerList.Where(r => (!string.IsNullOrEmpty(r.CityName) && r.CityName.ToLower().Contains(filterValue))).ToList();
                //}
                //if (filterType == "State")
                //{
                //    this.model.RetailerList = this.model.RetailerList.Where(r => (!string.IsNullOrEmpty(r.StateName) && r.StateName.ToLower().Contains(filterValue))).ToList();
                //}

                if (this.model.RetailerList != null)
                {
                    foreach (var retailer in this.model.RetailerList)
                    {
                        /// retailer.RegistrationDate = Convert.ToDateTime(retailer.DateString);
                        var gpsy = retailer.AddressGpsY;
                        if (!string.IsNullOrEmpty(retailer.AddressGpsX) || !string.IsNullOrEmpty(retailer.AddressGpsY))
                        {
                            retailer.LocationLink = "https://www.google.com/maps/place/" + retailer.AddressGpsX + "," + retailer.AddressGpsY;
                        }

                        if (!string.IsNullOrEmpty(retailer.ShopGpsX) || !string.IsNullOrEmpty(retailer.ShopGpsY))
                        {
                            retailer.DeviceLocationLink += "https://www.google.com/maps/place/" + retailer.ShopGpsX + "," + retailer.ShopGpsY;
                        }


                        retailer.DetailedDistributer = "<div>Distributer Name : " + retailer.DistributerName + "</div><div> DISTRIBUTER CITY : " + retailer.DistributerCity + "</div><div> DISTRIBUTER Mobile : " + retailer.DistributerMobileNo + "</div>";
                        retailer.DetailedAddress = "<div>" + retailer.Address + "</div><div> PIN : " + retailer.PinCode + "</div>";
                        retailer.CredentialDetail = "<div>Mobile : " + retailer.Mobile + "</div><div> Password : " + retailer.Password + "</div>";
                        retailer.PersonalDetail = "<div>Email : " + retailer.Email + "</div><div> CITY : " + retailer.CityName + "</div><div> State : " + retailer.StateName + "</div>";
                        retailer.GPSDetail = "<div>Shop Gps : " + retailer.AddressGpsX + " , " + retailer.AddressGpsY + "</div><div> Registration Gps : " + retailer.ShopGpsX + " , " + retailer.ShopGpsY + "</div><div> Distance : " + retailer.Distance + " Km</div>";
                        if (!string.IsNullOrEmpty(retailer.DateString))
                        {
                            retailer.RegistrationDate = Convert.ToDateTime(retailer.DateString);
                        }
                    }

                    CreateRetailerTableString();
                }
            }

            return View("RegisteredRetailer", this.model);
        }

        public ActionResult LogOut()
        {
            Session["LoginUserName"] = null;
            return RedirectToAction("Index", "Login");
        }

        public async Task<ActionResult> GetOrders(string filterType, string filterValue, string fromDate, string toDate)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.OrderList = await this.repository.GetOrderList();

            this.model.FilterDetail = new Filters();
            this.model.FilterDetail.AssignOrderListFilter();
            this.model.FilterDetail.SelectedFilterName = filterType;
            this.model.FilterDetail.FilterValue = filterValue;
            this.model.FilterDetail.FromDate = fromDate;
            this.model.FilterDetail.ToDate = toDate;
            if (!string.IsNullOrEmpty(filterValue))
            {
                filterValue = filterValue.Trim().ToLower();
            }

            if (this.model.OrderList != null)
            {
                if (filterType == "Id")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => r.Id.ToLower().Contains(filterValue)).ToList();
                }
                //if (filterType == "Date")
                //{
                    
                //}
                if (filterType == "Order No")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => (!string.IsNullOrEmpty(r.OrderNo) && r.OrderNo.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Product Name")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => (!string.IsNullOrEmpty(r.ProductName) && r.ProductName.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Product Id")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => (!string.IsNullOrEmpty(r.ProductId) && r.ProductId.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Retailer Id")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => (!string.IsNullOrEmpty(r.RetailerId) && r.RetailerId.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Retailer Name")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => (!string.IsNullOrEmpty(r.Retailer) && r.Retailer.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Address")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => (!string.IsNullOrEmpty(r.Address) && r.Address.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Order Status")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => (!string.IsNullOrEmpty(r.OrderStatus) && r.OrderStatus.ToLower().Contains(filterValue.ToLower()))).ToList();
                }
                if (filterType == "State")
                {
                    this.model.OrderList = this.model.OrderList.Where(r => (!string.IsNullOrEmpty(r.State) && r.State.ToLower().Contains(filterValue.ToLower()))).ToList();
                }


                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    try
                    {
                        DateTime d1 = DateTime.ParseExact(fromDate, "MM/dd/yyyy", CultureInfo.CurrentCulture);
                        DateTime d2 = DateTime.ParseExact(toDate, "MM/dd/yyyy", CultureInfo.CurrentCulture);

                        this.model.OrderList = this.model.OrderList.Where(r => r.Date.Value.Date >= d1.Date && r.Date.Value.Date <= d2.Date).ToList();
                    }
                    catch (Exception ex)
                    {
                        this.model.OrderList = null;
                    }
                   
                }

                else if (!string.IsNullOrEmpty(fromDate))
                {
                    try
                    {
                        DateTime d1 = DateTime.ParseExact(fromDate, "MM/dd/yyyy", CultureInfo.CurrentCulture);
                        this.model.OrderList = this.model.OrderList.Where(r => r.Date.Value.Date >= d1.Date).ToList();
                    }
                    catch (Exception ex)
                    {
                        this.model.OrderList = null;
                    }
                }

                else if (!string.IsNullOrEmpty(toDate))
                {
                   try
                   {
                        DateTime d2 = DateTime.ParseExact(toDate, "MM/dd/yyyy", CultureInfo.CurrentCulture);
                        this.model.OrderList = this.model.OrderList.Where(r => r.Date.Value.Date <= d2.Date).ToList();
                    }
                    catch (Exception ex)
                    {
                        this.model.OrderList = null;
                    }
                }

            }
            this.model.OrderDetail = new R_OrderMaster();
            this.model.AssignStatusList();
            return View("OrderView", this.model);
        }

        public async Task<ActionResult> UpdateFestivePointStatusDetail(string Id)
        {
            this.repository = new Repository();
            var result = await this.repository.UpdateFestivePointStatus(Id);
            return Json(result);
        }

        public async Task<ActionResult> UpdateMessageStatusDetail(string Id)
        {
            this.repository = new Repository();
            var result = await this.repository.UpdateMessageStatus(Id);
            return Json(result);
        }

        public async Task<ActionResult> UpdateMediaCategoryStatus(string Id)
        {
            this.repository = new Repository();
            var result = await this.repository.UpdateMediaCategoryStatus(Id);
            return Json(result);
        }

        public async Task<ActionResult> UpdateUploadedMediaStatus(string Id)
        {
            this.repository = new Repository();
            var result = await this.repository.UpdateUploadedMediaStatus(Id);
            return Json(result);
        }

        public async Task<ActionResult> UpdateBannerStatusDetail(string Id)
        {
            this.repository = new Repository();
            var result = await this.repository.UpdateBannerStatus(Id);
            return Json(result);
        }

        public async Task<ActionResult> UpdateRetailerImageStatus(string Id, bool status)
        {
            this.repository = new Repository();
            var result = await this.repository.UpdateRetailerImageStatus(Id, status);
            return Json(result);
        }

        public async Task<ActionResult> DeleteNotification(string Id)
        {
            this.repository = new Repository();
            var result = await this.repository.DeleteNotification(Id);
            return Json(result);
        }


        public async Task<ActionResult> UpdateProductStatus(string productId)
        {
            this.repository = new Repository();
            var list = new List<R_ProductMaster>();

            list.Add(new R_ProductMaster
            {
                Id = productId
            });

            var result = await this.repository.UpdateProductStatus(list);

            return Json(result);
        }

        public async Task<ActionResult> UpdatePromotionStatus(string Id)
        {
            this.repository = new Repository();
            var result = await this.repository.UpdatePromotionStatus(Id);

            return Json(result);
        }

        public async Task<ActionResult> UpdateStatus(DashboardModel statusModel)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();

            var result = await this.repository.UpdateStatus(statusModel.OrderDetail);
            if (result == null || string.IsNullOrEmpty(result.ResponseValue))
            {
                return Json("Error : Something went wrong. Please try again later.");
            }
            else if (!result.Status)
            {
                return Json("Error : " + result.ResponseValue);
            }
            else
            {
                this.model.OrderList = await this.repository.GetOrderList();
            }

            return View("_orderPartialView", this.model);
        }

        public async Task<ActionResult> GetProductView(string filterType, string filterValue)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.ProductList = await this.repository.GetProductList();

            this.model.FilterDetail = new Filters();
            this.model.FilterDetail.AssignProductFilter();
            this.model.FilterDetail.SelectedFilterName = filterType;
            this.model.FilterDetail.FilterValue = filterValue;

            if (!string.IsNullOrEmpty(filterValue))
            {
                filterValue = filterValue.Trim().ToLower();
            }

            if (this.model.ProductList != null && this.model.ProductList.Count > 0)
            {
                if (filterType == "Id")
                {
                    this.model.ProductList = this.model.ProductList.Where(r => r.Id.ToLower().Contains(filterValue)).ToList();
                }
                if (filterType == "Product Code")
                {
                    this.model.ProductList = this.model.ProductList.Where(r => (!string.IsNullOrEmpty(r.ProductCode) && r.ProductCode.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Name")
                {
                    this.model.ProductList = this.model.ProductList.Where(r => (!string.IsNullOrEmpty(r.Name) && r.Name.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Size")
                {
                    this.model.ProductList = this.model.ProductList.Where(r => (!string.IsNullOrEmpty(r.Size) && r.Size.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Points")
                {
                    var fPoint = Convert.ToDecimal(filterValue);
                    this.model.ProductList = this.model.ProductList.Where(r => r.Points == fPoint).ToList();
                }

                CreateProductTableString();
            }

            return View("ProductView", this.model);
        }

        public async Task<ActionResult> GetGallaryView(string filterType, string filterValue)
        {

            this.repository = new Repository();
            this.model = new DashboardModel();

            this.model.MediaList = await this.repository.ListUploadedMedia();

            this.model.FilterDetail = new Filters();
            this.model.FilterDetail.AssignMediaCategoryFilter();
            this.model.FilterDetail.SelectedFilterName = filterType;
            this.model.FilterDetail.FilterValue = filterValue;

            if (!string.IsNullOrEmpty(filterValue))
            {
                filterValue = filterValue.Trim().ToLower();
            }

            if (this.model.MediaList != null)
            {
                if (filterType == "Id")
                {
                    this.model.MediaList = this.model.MediaList.Where(r => r.Id.ToLower().Contains(filterValue)).ToList();
                }
                if (filterType == "Category Name")
                {
                    this.model.MediaList = this.model.MediaList.Where(r => (!string.IsNullOrEmpty(r.CategoryName) && r.CategoryName.ToLower().Contains(filterValue))).ToList();
                }

                foreach (var media in this.model.MediaList)
                {
                    media.FileName = media.DisplayName;

                    if (string.IsNullOrEmpty(media.FileName) && !(string.IsNullOrEmpty(media.Url)))
                    {
                        media.FileName = media.Url.Substring(media.Url.LastIndexOf("/") + 1).Trim();
                    }

                    media.Date = Convert.ToDateTime(media.DateString);
                }
            }

            return View("GallaryView", this.model);
        }

        public async Task<ActionResult> ListRetailerImagesView(string PromoId, string RetailerId)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();

            this.model.RetailerUploadedImagesList = await this.repository.ListRetailerImages(PromoId, RetailerId);
            return View("RetailerImagesView", this.model);
        }

        public async Task<ActionResult> UpdatePromotionRetailerEntrySatus(string PromoId, string RetailerId, bool operation)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();

            var res = await this.repository.UpdatePromotionEntryStatus(PromoId, RetailerId, operation);
            if (res == null)
            {
                res = new Response();
                res.ResponseValue = "Something went wrong. Please try again later.";
            }

            return Json(res);
        }

        public async Task<ActionResult> RetailerListByPromoIdView(string Id)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();

            this.model.RetailerListByPromoId = await this.repository.ListRetailerListByPromotionId(Id);
            if (!(this.model.RetailerListByPromoId == null || this.model.RetailerListByPromoId.Count == 0))
            {
                this.model.PromotionDetail = new R_Promotion();
                var detail = this.model.RetailerListByPromoId.FirstOrDefault();
                this.model.PromotionDetail.Id = detail.PromoId;
                this.model.PromotionDetail.Heading = detail.PromoHeading;
                this.model.PromotionDetail.HeadingText = detail.PromoText;
            }

            return View("RetailerListByPromoView", this.model);
        }

        public async Task<ActionResult> GallaryByIdView(string Id)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            if (string.IsNullOrEmpty(Id))
            {
                this.model.MediaDetail = new R_UploadedMedia();
            }
            else
            {
                this.model.MediaDetail = await this.repository.GetUploadedMediaById(Id);
            }

            this.model.MediaCategoryList = await this.repository.ActiveListMediaCategory();
            if (this.model.MediaCategoryList == null || this.model.MediaCategoryList.Count == 0)
            {
                this.model.MediaCategoryList = new List<R_MediaCategory>();
                this.model.MediaCategoryList.Add(new R_MediaCategory
                {
                    Id = "0",
                    Category = "-No CategoryFound-"
                });
            }

            return View("AddGallaryView", this.model);
        }

        //private void CreateGallaryTableString()
        //{
        //    this.model.TableString = "<table id=gallaryStringTable><thead><tr><th>ID</th><th>PRODUCT CODE</th><th>NAME</th><th>SIZE</th><th>POINTS</th><th>QUANTITY</th><th>STATUS</th></tr></thead><tbody>";
        //    foreach (var product in this.model.ProductList)
        //    {
        //        this.model.TableString += "<tr><td>" + product.Id + "</td><td>" + product.ProductCode + "</td><td>" + product.Name + "</td>" +
        //            "<td>" + product.Size + "</td><td>" + product.Points + "</td><td>" + product.Quantity + "</td><td id='" + ("data" + product.Id) + "'>" + (product.IsActive == true ? "DeActivate" : "Activate") + "</td></tr>";
        //    }
        //    this.model.TableString += "</tbody></table>";
        //}

        private void CreateProductTableString()
        {
            this.model.TableString = "<table id=productStringTable><thead><tr><th>ID</th><th>PRODUCT CODE</th><th>NAME</th><th>SIZE</th><th>POINTS</th><th>QUANTITY</th><th>STATUS</th></tr></thead><tbody>";
            foreach (var product in this.model.ProductList)
            {
                this.model.TableString += "<tr><td>" + product.Id + "</td><td>" + product.ProductCode + "</td><td>" + product.Name + "</td>" +
                    "<td>" + product.Size + "</td><td>" + product.Points + "</td><td>" + product.Quantity + "</td><td id='" + ("data" + product.Id) + "'>" + (product.IsActive == true ? "DeActivate" : "Activate") + "</td></tr>";
            }

            this.model.TableString += "</tbody></table>";
        }

        private void CreatePointsLedgerTableString()
        {
            this.model.TableString = "<table id=pointsLedgerStringTable><thead><tr><th>RETAILER ID</th><th>FIRM NAME</th><th>BARCODE</th><th>EARNED POINTS</th><th>REDEEM POINTS</th><th>DATE</th><th>PRODUCT</th></tr></thead><tbody>";
            foreach (var point in this.model.PointsLedger)
            {
                this.model.TableString += "<tr><td>" + point.RetailerId + "</td><td>" + point.FirmName + "</td><td>" + point.Barcode + "</td>" +
                    "<td>" + point.DabitPoints + "</td><td>" + point.CreditPoints + "</td><td>" + point.EarnSpentDate + "</td><td>" + point.ProductName + "</td></tr>";
            }
            this.model.TableString += "</tbody></table>";
        }

        private void CreatePromotionTableString()
        {
            this.model.TableString = "<table id=promotionStringTable><thead><tr><th>ID</th><th>HEADING</th><th>DETAIL</th><th>IMAGE COUNT</th><th>STATUS</th></tr></thead><tbody>";
            foreach (var promotion in this.model.PromotionList)
            {
                this.model.TableString += "<tr><td>" + promotion.Id + "</td><td>" + promotion.Heading + "</td><td>" + promotion.HeadingText + "</td>" +
                    "<td>" + promotion.ImageCount + "</td><td id='" + ("data" + promotion.Id) + "'>" + (promotion.IsActive == true ? "DeActivate" : "Activate") + "</td></tr>";
            }
            this.model.TableString += "</tbody></table>";
        }

        private void CreateRetailerTableString()
        {
            this.model.TableString = "<table id=retailerStringTable><thead><tr><th>ID</th><th>FIRM NAME</th><th>DISTRIBUTER NAME</th><th>DISTRIBUTER CITY</th><th>DISTRIBUTER MOBILE</th><th>ADDRESS</th><th>PIN</th><th>USERNAME</th><th>PASSWORD</th><th>EMAIL</th><th>CITY</th><th>STATE</th><th>POINTS</th><th>TOTAL EARNED POINTS</th><th>SHOP GPS</th><th>REGISTRATION GPS</th><th>DISTANCE</th><th>REGISTRATION DATE</th><th>STATUS</th></tr></thead><tbody>";
            foreach (var retailer in this.model.RetailerList)
            {
                this.model.TableString += "<tr><td>" + retailer.ID + "</td><td>" + retailer.FirmName + "</td><td>" + retailer.DistributerName + "</td><td>" + retailer.DistributerCity + "</td><td>" + retailer.DistributerMobileNo + "</td>" +
                    "<td>" + retailer.Address + "</td><td>" + retailer.PinCode + "</td><td>" + retailer.Mobile + "</td><td>" + retailer.Password + "</td><td>" + retailer.Email + "</td><td>" + retailer.CityName + "</td><td>" + retailer.StateName + "</td><td>" + retailer.Points + "</td><td>" + retailer.TotalEarned + "</td><td>" + retailer.AddressGpsX + " , " + retailer.AddressGpsY + "</td><td>" + retailer.ShopGpsX + " , " + retailer.ShopGpsY + "</td><td>" + retailer.Distance + "Km</td><td>" + retailer.RegistrationDate + "</td><td id='" + ("data" + retailer.ID) + "'>" + (retailer.IsActive == true ? "True" : "False") + "</td></tr>";
            }
            this.model.TableString += "</tbody></table>";
        }

        public async Task<ActionResult> GetPromotionView(string filterType, string filterValue)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.PromotionList = await this.repository.GetPromotionList();

            this.model.FilterDetail = new Filters();
            this.model.FilterDetail.AssignPromotionFilter();
            this.model.FilterDetail.SelectedFilterName = filterType;
            this.model.FilterDetail.FilterValue = filterValue;

            if (!string.IsNullOrEmpty(filterValue))
            {
                filterValue = filterValue.Trim().ToLower();
            }

            if (this.model.PromotionList != null)
            {
                if (filterType == "Id")
                {
                    this.model.PromotionList = this.model.PromotionList.Where(r => r.Id.ToLower().Contains(filterValue)).ToList();
                }
                if (filterType == "Heading")
                {
                    this.model.PromotionList = this.model.PromotionList.Where(r => (!string.IsNullOrEmpty(r.Heading) && r.Heading.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Detail")
                {
                    this.model.PromotionList = this.model.PromotionList.Where(r => (!string.IsNullOrEmpty(r.HeadingText) && r.HeadingText.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Image Count")
                {
                    var fcount = Convert.ToInt32(filterValue);
                    this.model.PromotionList = this.model.PromotionList.Where(r => (r.ImageCount == fcount)).ToList();
                }
                if (filterType == "Total Entry Allowed")
                {
                    var fentry = Convert.ToInt32(filterValue);
                    this.model.PromotionList = this.model.PromotionList.Where(r => (r.TotalEntries == fentry)).ToList();
                }

                //foreach (var promo in this.model.PromotionList)
                //{

                //}

                CreatePromotionTableString();
            }
            return View("PromotionView", this.model);
        }

        public async Task<ActionResult> GetMediaCategoryView(string filterType, string filterValue)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.MediaCategoryList = await this.repository.ListMediaCategory();

            this.model.FilterDetail = new Filters();
            this.model.FilterDetail.AssignMediaCategoryFilter();
            this.model.FilterDetail.SelectedFilterName = filterType;
            this.model.FilterDetail.FilterValue = filterValue;

            if (!string.IsNullOrEmpty(filterValue))
            {
                filterValue = filterValue.Trim().ToLower();
            }

            if (this.model.MediaCategoryList != null)
            {
                if (filterType == "Id")
                {
                    this.model.MediaCategoryList = this.model.MediaCategoryList.Where(r => r.Id.ToLower().Contains(filterValue)).ToList();
                }
                if (filterType == "Category Name")
                {
                    this.model.MediaCategoryList = this.model.MediaCategoryList.Where(r => (!string.IsNullOrEmpty(r.Category) && r.Category.ToLower().Contains(filterValue))).ToList();
                }
            }
            return View("MediaCategoryView", this.model);
        }

        public async Task<ActionResult> GetBannerListView()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.BannerList = await this.repository.ListBanners();

            foreach (var noti in this.model.BannerList)
            {
                noti.Date = Convert.ToDateTime(noti.DateString);

            }
            return View("BannerView", this.model);
        }

        public async Task<ActionResult> GetFestivePointListView()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.FestivePointList = await this.repository.ListFestivePoints();

            if (this.model.FestivePointList != null)
            {
                foreach (var noti in this.model.FestivePointList)
                {
                    noti.AddedDate = Convert.ToDateTime(noti.DateString);
                    noti.FromDate = Convert.ToDateTime(noti.FromDateString);
                    noti.ToDate = Convert.ToDateTime(noti.ToDateString);

                }
            }
            return View("FestivePointView", this.model);
        }

        public async Task<ActionResult> GetAddFestivePointView(string Id)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            if (string.IsNullOrEmpty(Id))
            {
                this.model.FestivePointDetail = new R_FestivePointMaster();
            }
            else
            {
                this.model.FestivePointDetail = await this.repository.GetFestivePointById(Id);
            }

            return View("AddFestivePoint", this.model);
        }

        public async Task<ActionResult> GetAddBannerView(string Id)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            if (string.IsNullOrEmpty(Id))
            {
                this.model.BannerDetail = new R_BannerMaster();
            }
            else
            {
                this.model.BannerDetail = await this.repository.GetBannerById(Id);
            }

            return View("AddBannerView", this.model);
        }

        public async Task<ActionResult> GetMessageDetailListView()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.MessagesList = await this.repository.ListMessages();

            if (this.model.MessagesList != null)
            {
                foreach (var msg in this.model.MessagesList)
                {
                    msg.PublishTo = Convert.ToDateTime(msg.PublishToDate);
                    msg.MessagePublishDate = Convert.ToDateTime(msg.DateString);
                }
            }
            return View("MessagesListView", this.model);
        }

        public async Task<ActionResult> GetAddMessageDetailView(string Id)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            if (string.IsNullOrEmpty(Id))
            {
                this.model.MessageDetail = new R_MessageMaster();
            }
            else
            {
                this.model.MessageDetail = await this.repository.GetMessageById(Id);
            }

            return View("AddMessageView", this.model);
        }

        public async Task<ActionResult> SaveMessageDetail(DashboardModel messageModel)
        {
            if (messageModel == null || messageModel.MessageDetail == null)
            {
                return Json("Please send complete detail.");
            }

            this.repository = new Repository();
            var res = await this.repository.AddEditMessageDetail(messageModel.MessageDetail);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
        }

        public async Task<ActionResult> SaveBannerForm(DashboardModel bannerModel)
        {
            if (bannerModel == null || bannerModel.BannerDetail == null)
            {
                return Json("Please send complete detail.");
            }

            this.repository = new Repository();
            var res = await this.repository.AddEditBannerDetail(bannerModel.BannerDetail);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
        }

        public async Task<ActionResult> SaveFestivePointDetail(DashboardModel pointModel)
        {
            if (pointModel == null || pointModel.FestivePointDetail == null)
            {
                return Json("Please send complete detail.");
            }

            this.repository = new Repository();
            var res = await this.repository.AddEditFestivePointDetail(pointModel.FestivePointDetail);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
        }

        /******Version Functions***start**/

        public async Task<ActionResult> GetAppVersionListView()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.VersionList = await this.repository.ListAppVersions();

            foreach (var noti in this.model.VersionList)
            {
                noti.Date = Convert.ToDateTime(noti.DateString);

            }
            return View("ApplicationVersionList", this.model);
        }

        public ActionResult GetAddVersionView()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();

            this.model.VersionDetail = new R_AppVersion();
            return View("AddAppVersionDetail", this.model);
        }

        public async Task<ActionResult> SaveAppVersionDetail(DashboardModel versionModel)
        {
            if (versionModel == null || versionModel.VersionDetail == null)
            {
                return Json("Please send complete detail.");
            }

            this.repository = new Repository();
            var res = await this.repository.AddAppVersionDetail(versionModel.VersionDetail);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
        }

        /*********App version end**********/

        public async Task<ActionResult> AddNotificationView()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.NotificationDetail = new R_NotificationManager();

            await AssignStateCityList();
            return View("AddNotification", this.model);
        }

        public async Task<ActionResult> GetNotificationListView(string filterType, string filterValue)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.NotificationList = await this.repository.ListNotification();

            this.model.FilterDetail = new Filters();
            this.model.FilterDetail.AssignNotificationFilter();
            this.model.FilterDetail.SelectedFilterName = filterType;
            this.model.FilterDetail.FilterValue = filterValue;

            if (!string.IsNullOrEmpty(filterValue))
            {
                filterValue = filterValue.Trim().ToLower();
            }

            if (this.model.NotificationList != null)
            {
                if (filterType == "Id")
                {
                    this.model.NotificationList = this.model.NotificationList.Where(r => r.Id.ToLower().Contains(filterValue)).ToList();
                }
                if (filterType == "Header")
                {
                    this.model.NotificationList = this.model.NotificationList.Where(r => (!string.IsNullOrEmpty(r.Header) && r.Header.ToLower().Contains(filterValue))).ToList();
                }
                if (filterType == "Notification Message")
                {
                    this.model.NotificationList = this.model.NotificationList.Where(r => (!string.IsNullOrEmpty(r.Notification) && r.Notification.ToLower().Contains(filterValue))).ToList();
                }
            }

            foreach (var noti in this.model.NotificationList)
            {
                noti.Date = Convert.ToDateTime(noti.DateString);
                // noti.DateString = noti.Date.ToString( "yyyy-MM-dd HH:mm tt");
            }

            return View("NotificationListView", this.model);
        }

        public async Task<ActionResult> AddAndSendNotificationDetail(DashboardModel mediaModel)
        {
            if (mediaModel == null || mediaModel.NotificationDetail == null)
            {
                return Json("Please send complete detail.");
            }

            this.repository = new Repository();
            var res = await this.repository.AddAndSendNotification(mediaModel.NotificationDetail);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
        }

        public async Task<ActionResult> SaveMediaCategoryDetail(DashboardModel mediaModel)
        {
            if (mediaModel == null || mediaModel.MediaCategory == null)
            {
                return Json("Please send complete detail.");
            }

            this.repository = new Repository();
            var res = await this.repository.AddEditMediaCategoryDetail(mediaModel.MediaCategory);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
        }

        public async Task<ActionResult> MediaCategoryByIdView(string Id)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            if (string.IsNullOrEmpty(Id))
            {
                this.model.MediaCategory = new R_MediaCategory();
            }
            else
            {
                this.model.MediaCategory = await this.repository.GetMediaCategoryById(Id);
            }

            return View("AddMediaCategory", this.model);
        }

        public async Task<ActionResult> PromotionByIdView(string Id)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            if (string.IsNullOrEmpty(Id))
            {
                this.model.PromotionDetail = new R_Promotion();
            }
            else
            {
                this.model.PromotionDetail = await this.repository.PromotionByIdView(Id);
            }

            return View("AddPromotionView", this.model);
        }

        public async Task<ActionResult> ProductByIdView(string Id)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.ProductDetail = await this.repository.ProductByIdView(Id);

            return View("AddProductView", this.model);
        }

        public async Task<ActionResult> EncryptBarcode()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            var result = await this.repository.EncryptBarcode();

            return View("EncryptBarcode", result);
        }

        public async Task<ActionResult> GetRetailerPointsLedgerView(string filterType, string filterValue, string fromDate, string toDate)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();


            this.model.FilterDetail = new Filters();
            this.model.FilterDetail.AssignPointsLedgerFilter();
            this.model.FilterDetail.SelectedFilterName = filterType;
            this.model.FilterDetail.FilterValue = filterValue;
            this.model.FilterDetail.FromDate = fromDate;
            this.model.FilterDetail.ToDate = toDate;

            if (!(string.IsNullOrEmpty(filterType) && string.IsNullOrEmpty(filterValue) && string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate)))
            {
                this.model.PointsLedger = await this.repository.GetPointsLedgerWithFilters(this.model.FilterDetail);
                //if (!string.IsNullOrEmpty(filterValue))
                //{
                //    filterValue = filterValue.Trim().ToLower();
                //}

                if (this.model.PointsLedger != null)
                {
                    //    if (filterType == "Retailer Id")
                    //    {
                    //        this.model.PointsLedger = this.model.PointsLedger.Where(r => r.RetailerId.ToLower().Contains(filterValue)).ToList();
                    //    }
                    //    if (filterType == "Firm Name")
                    //    {
                    //        this.model.PointsLedger = this.model.PointsLedger.Where(r => (!string.IsNullOrEmpty(r.FirmName) && r.FirmName.ToLower().Contains(filterValue))).ToList();
                    //    }
                    //    if (filterType == "Barcode")
                    //    {
                    //        this.model.PointsLedger = this.model.PointsLedger.Where(r => (!string.IsNullOrEmpty(r.Barcode) && r.Barcode.ToLower().Contains(filterValue))).ToList();
                    //    }

                    //    if (filterType == "Date")
                    //    {
                    //        this.model.PointsLedger = this.model.PointsLedger.Where(r => (!string.IsNullOrEmpty(r.EarnSpentDateString) && r.Barcode.Contains(filterValue))).ToList();
                    //    }

                    //    if (fromDate == "Date")
                    //    {
                    //        this.model.PointsLedger = this.model.PointsLedger.Where(r => (!string.IsNullOrEmpty(r.EarnSpentDateString) && r.Barcode.Contains(filterValue))).ToList();
                    //    }

                    foreach (var points in this.model.PointsLedger)
                    {
                        if (string.IsNullOrEmpty(points.LocationX))
                        {
                            points.LocationLink = " - ";
                        }
                        else
                        {
                            points.LocationLink = "https://www.google.com/maps/place/" + points.LocationX + "," + points.LocationY;
                        }

                        points.EarnSpentDate = Convert.ToDateTime(points.EarnSpentDateString);
                    }

                    CreatePointsLedgerTableString();
                }
            }

            return View("PointsLedgerView", this.model);
        }

        public async Task<ActionResult> GetCityListByState(string Id)
        {
            var cityList = new List<R_CityMaster>();
            this.repository = new Repository();
            if (string.IsNullOrEmpty(Id))
            {
                return null;
            }
            else
            {
                cityList = await this.repository.GetCityList(Id);
            }

            if (cityList == null || cityList.Count == 0)
            {
                cityList = new List<R_CityMaster>();
                if (this.model.CityList == null)
                {
                    this.model.CityList = new List<R_CityMaster>();
                    this.model.CityList.Add(new R_CityMaster
                    {
                        cityID = 0,
                        cityName = "-Not Available-"
                    });
                }
            }
            return Json(cityList);
        }

        public async Task<ActionResult> GetRetailerEditView(string retailerId)
        {
            this.repository = new Repository();
            this.model = new DashboardModel();

            this.model.RetailerDetail = await this.repository.GetRetailerDetail(retailerId);
            if (this.model.RetailerDetail != null)
            {
                await AssignStateCityList();
                this.model.AssignBlockOptionList();
            }

            //this.model.DistributerCityList = await this.repository.GetDistributerCityList();
            //this.model.DistributerList = await this.repository.GetDistributerList(this.model.RetailerDetail.DistributorId);

            return View("EditRegisteredRetailer", this.model);
        }

        private async Task AssignStateCityList()
        {
            this.model.StateList = await this.repository.GetStateList();
            if (this.model.StateList == null)
            {
                this.model.StateList = new List<R_StateMaster>();
                this.model.StateList.Add(new R_StateMaster
                {
                    Id = 0,
                    Name = "-Not Available-"
                });
            }

            if (this.model.RetailerDetail != null)
            {
                this.model.CityList = await this.repository.GetCityList(this.model.RetailerDetail.StateId);
            }

            if (this.model.CityList == null)
            {
                this.model.CityList = new List<R_CityMaster>();
                this.model.CityList.Add(new R_CityMaster
                {
                    cityID = 0,
                    cityName = "-Not Available-"
                });
            }
        }

        public async Task<ActionResult> SaveRetailerDetail(DashboardModel modelDetail)
        {
            try
            {
                if (modelDetail == null)
                {
                    return Json("Please Send Complate Detail.");
                }

                this.repository = new Repository();
                var result = await this.repository.SaveRetailerDetail(modelDetail.RetailerDetail);
                if (result == null)
                {
                    return Json("Something went wrong. Please try again later.");
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException);
            }

            //return Json(string.Empty);
        }

        public void DownloadTemplate()
        {
            {
                var data = new[]{
                               new{ ProductCode="", ProductName="", ProductPoints="" , ProductSize="", ProductQuantity = ""}
                      };

                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(data, true);
                using (var memoryStream = new MemoryStream())
                {
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.ContentType = "application/force-download";
                    Response.AddHeader("content-disposition", "attachment;  filename=ProductTemplate.xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Upload()
        {
            DashboardModel objResponse = new DashboardModel();
            try
            {
                HttpPostedFileBase upload = null;
                string filename = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        filename = Path.GetFileName(Request.Files[i].FileName);
                        upload = files[i];
                    }
                    if (upload != null && upload.ContentLength > 0)
                    {
                        try
                        {
                            DataTable dt = GetDataTableFromSpreadsheet(upload, filename, false);
                            objResponse.ProductList = SaveExcelReportData(dt);
                            this.repository = new Repository();
                            var result = await this.repository.AddBulkProduct(objResponse.ProductList);
                            model = new DashboardModel();
                            if (result == null)
                            {
                                model.Error = "Something went wrong, Please try again later.";
                            }
                            else if (!result.Status)
                            {
                                model.Error = result.ResponseValue;
                            }
                            else
                            {
                                model.Error = "";
                                //  model.ProductList = await this.repository.GetProductList();
                            }

                            return Json(result);
                        }
                        catch (Exception e)
                        {
                            //objResponse.ErrorMessage = "error--" + e.Message + "--Inner Exception--" + e.InnerException;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // objResponse.ErrorMessage = "error--" + e.Message + "--Inner Exception--" + e.InnerException;
            }

            var jsonProduct = Json(objResponse, JsonRequestBehavior.AllowGet);
            jsonProduct.MaxJsonLength = int.MaxValue;
            return jsonProduct;
        }

        private static ISheet GetFileStream(HttpPostedFileBase MyExcelStream, string filename1, bool ReadOnly)
        {

            HttpPostedFileBase files = MyExcelStream; //Read the Posted Excel File  
            ISheet sheet; //Create the ISheet object to read the sheet cell values  
            string filename = Path.GetFileName(files.FileName); //get the uploaded file name  
            var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
            if (fileExt == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(files.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(files.InputStream); //XSSFWorkBook will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
            }

            return sheet;
        }


        public static DataTable GetDataTableFromSpreadsheet(HttpPostedFileBase MyExcelStream, string filename, bool ReadOnly)
        {

            try
            {
                var sh = GetFileStream(MyExcelStream, filename, ReadOnly);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sh.GetRow(0);
                int colCount = headerRow.LastCellNum;
                for (var c = 0; c < colCount; c++)
                    dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString());
                var i = 1;
                var currentRow = sh.GetRow(i);
                while (currentRow != null)
                {
                    var dr = dtExcelTable.NewRow();
                    for (var j = 0; j < currentRow.Cells.Count; j++)
                    {
                        var cell = currentRow.GetCell(j);

                        if (cell != null)
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    {
                                        DateTime date = cell.DateCellValue;
                                        dr[j] = date.ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        dr[j] = cell.NumericCellValue.ToString();
                                    }
                                    break;
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Blank:
                                    dr[j] = string.Empty;
                                    break;
                            }
                    }
                    dtExcelTable.Rows.Add(dr);
                    i++;
                    currentRow = sh.GetRow(i);
                }
                return dtExcelTable;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        /// <summary>
        /// Save qci report data
        /// </summary>
        /// <param name="dt"></param>
        public List<R_ProductMaster> SaveExcelReportData(DataTable dt)
        {
            var productList = new List<R_ProductMaster>();
            try
            {
                if (dt == null)
                {
                    return null;
                }

                var prodDetail = new R_ProductMaster();
                DataColumnCollection columns = dt.Columns;

                foreach (DataRow row in dt.Rows)
                {

                    prodDetail = new R_ProductMaster();
                    if (columns.Contains("ProductCode"))
                    {
                        prodDetail.ProductCode = Convert.ToString(row["ProductCode"]);
                    }
                    if (columns.Contains("ProductName"))
                    {
                        prodDetail.Name = Convert.ToString(row["ProductName"]);
                    }
                    if (columns.Contains("ProductPoints"))
                    {
                        prodDetail.Points = Convert.ToInt32(row["ProductPoints"]);
                    }
                    if (columns.Contains("ProductQuantity"))
                    {
                        prodDetail.Quantity = Convert.ToInt32(row["ProductQuantity"]);
                    }
                    if (columns.Contains("ProductSize"))
                    {
                        prodDetail.Size = Convert.ToString(row["ProductSize"]);
                    }

                    productList.Add(prodDetail);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return productList;
        }

        public async Task<ActionResult> SaveImage(string data, string fileName)
        {
            var fileData = data.Split(',');

            var imageDetail = new ProfilePhoto();
            imageDetail.fileBytes = fileData[1];
            imageDetail.fileName = fileName;
            imageDetail.mediaType = Resources.ProductImage;

            this.repository = new Repository();
            var res = await this.repository.SaveImage(imageDetail);
            if (res == null || !res.Status)
            {
                return null;
            }
            else
            {
                return Json(res.Url);
            }

        }

        public async Task<ActionResult> SaveGallaryDetail(DashboardModel gallaryModel)
        {
            if (gallaryModel == null || gallaryModel.MediaDetail == null)
            {
                return Json("Please send complete detail.");
            }
            if (string.IsNullOrEmpty(gallaryModel.MediaDetail.Id) && string.IsNullOrEmpty(gallaryModel.MediaDetail.Url))
            {
                return Json("Please Select File to upload.");
            }

            this.repository = new Repository();

            var res = await this.repository.AddEditGallaryMediaDetail(gallaryModel.MediaDetail);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
        }

        public async Task<ActionResult> SaveProductImageDetail(DashboardModel productModel)
        {
            if (productModel == null || productModel.ProductDetail == null)
            {
                return Json("Please send complete detail.");
            }

            this.repository = new Repository();
            var list = new List<R_ProductMaster>();
            list.Add(productModel.ProductDetail);
            var res = await this.repository.EditProduct(list);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
            return Json(res);
        }

        public async Task<ActionResult> SavePromotionImageDetail(DashboardModel promoModel)
        {
            if (promoModel == null || promoModel.PromotionDetail == null)
            {
                return Json("Please send complete detail.");
            }

            this.repository = new Repository();
            var res = await this.repository.SavePromotionDetail(promoModel.PromotionDetail);
            if (res == null)
            {
                return Json("Something went wrong. Please try again later.");
            }
            else
            {
                return Json(res.ResponseValue);
            }
        }

        public async Task<ActionResult> GetCompanySettings()
        {
            this.repository = new Repository();
            this.model = new DashboardModel();
            this.model.SettingDetail = new R_Setting();
            this.model.SettingDetail = await this.repository.GetSettingDetail();
            return View("SettingDetail", this.model);
        }

        
            public async Task<ActionResult> SaveSettings(DashboardModel modelDetail)
        {
            try
            {
                if (modelDetail == null)
                {
                    return Json("Please Send Complate Detail.");
                }

                this.repository = new Repository();
                var result = await this.repository.SaveSettings(modelDetail.SettingDetail);
                if (result == null)
                {
                    return Json("Something went wrong. Please try again later.");
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException);
            }

            //return Json(string.Empty);
        }

    }
}