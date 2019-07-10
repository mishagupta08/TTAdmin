using System;

namespace TTGarmentAdmin.Models
{
    public partial class RetailerMaster
    {
        public string ID { get; set; }
        public string DistributorId { get; set; }
        public string FirmName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public string ProfilePicture { get; set; }
        public string ShopLogo { get; set; }
        public string ShopGpsX { get; set; }
        public string ShopGpsY { get; set; }
        public string AddressGpsX { get; set; }
        public string AddressGpsY { get; set; }
        public string LocationLink { get; set; }
        public string DeviceLocationLink { get; set; }
        public string DetailedAddress { get; set; }
        public string DetailedDistributer { get; set; }
        public string PersonalDetail { get; set; }
        public string CredentialDetail { get; set; }
        public string GPSDetail { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Password { get; set; }
        public string DistributerName { get; set; }
        public string DistributerCity { get; set; }
        public string DistributerMobileNo { get; set; }
        public string DateString { get; set; }
        public decimal Distance { get; set; }
        public decimal Points { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public string IsBlock { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
    }
}