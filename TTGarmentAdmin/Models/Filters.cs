using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTGarmentAdmin.Models
{
    public class Filters
    {
        public List<string> FilterName { get; set; }

        public string FilterValue { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string SelectedFilterName { get; set; }

        public void AssignRetailerFilter()
        {
            FilterName = new List<string>();
            FilterName.Add("Id");
            FilterName.Add("Firm Name");
            FilterName.Add("Mobile Number");
            FilterName.Add("City");
            FilterName.Add("State");
            FilterName.Add("PinCode");
            FilterName.Add("Date");
        }

        public void AssignPointsLedgerFilter()
        {
            FilterName = new List<string>();
            FilterName.Add("Retailer Id");
            FilterName.Add("Firm Name");
            FilterName.Add("Barcode");
            FilterName.Add("Date");
        }

        public void AssignProductFilter()
        {
            FilterName = new List<string>();
            FilterName.Add("Id");
            FilterName.Add("Product Code");
            FilterName.Add("Name");
            FilterName.Add("Size");
            FilterName.Add("Points");
        }

        public void AssignPromotionFilter()
        {
            FilterName = new List<string>();
            FilterName.Add("Id");
            FilterName.Add("Heading");
            FilterName.Add("Detail");
            FilterName.Add("Image Count");
            FilterName.Add("Total Entry Allowed");
        }

        public void AssignMediaCategoryFilter()
        {
            FilterName = new List<string>();
            FilterName.Add("Id");
            FilterName.Add("Category Name");
        }

        public void AssignOrderListFilter()
        {
            FilterName = new List<string>();
            FilterName.Add("Id");
            FilterName.Add("Order No");
            FilterName.Add("Product Id");
            FilterName.Add("Product Name");
            FilterName.Add("Retailer Id");
            FilterName.Add("Retailer Name");
            FilterName.Add("Address");
            FilterName.Add("Order Status");
            FilterName.Add("Date");
            FilterName.Add("State");
        }

        public void AssignNotificationFilter()
        {
            FilterName = new List<string>();
            FilterName.Add("Id");
            FilterName.Add("Header");
            FilterName.Add("Notification Message");
        }
    }
}