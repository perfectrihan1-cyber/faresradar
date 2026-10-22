using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SagePay.IntegrationKit;
using SagePay.IntegrationKit.Messages;

namespace Viman.Models
{
    public class Website
    {
    }

    public class SagePayment
    {
        public SagePayment()
        {
            Currency = "GBP";
            Email = "rajneesh.sharma@hotmail.co.uk";
            VendorName = "travelopedialtd";
            SecurePassword = "c14371ee7b794271";
            Protocall = "3.00";
            PaymentMode = ConfigurationManager.AppSettings["SagepayEnvironment"].ToString();
            TransactionType = "DEFERRED";
        }

        public string Currency { get; set; }
        public string Email { get; set; }
        public string VendorName { get; set; }
        public string SecurePassword { get; set; }
        public string Protocall { get; set; }
        public static string PaymentMode { set; get; }
        public decimal Amount { set; get; }
        public string TransactionType { set; get; }
        public string NotificationUrl { set; get; }
        public string VendorTxCode { set; get; }
        public static string ServerPaymentUrl
        {
            get { return "https://" + PaymentMode.ToLower() + ".sagepay.com/gateway/service/vspserver-register.vsp"; }
        }

        public Billing Billing { set; get; }
        public Billing Shipping { set; get; }

        public void SetServerPaymentRequestData(IServerPayment request)
        {
            request.VpsProtocol = (ProtocolVersion)Enum.Parse(typeof(ProtocolVersion), "V_" + Protocall.Replace(".", ""));
            request.TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), TransactionType);
            request.Vendor = VendorName;
            VendorTxCode = GetNewVendorTxCode();
            request.VendorTxCode = VendorTxCode;
            request.Amount = Amount;
            request.Currency = Currency;
            request.Description = "Booking from " + request.Vendor.ToUpper();
            request.BillingSurname = Billing.LastName;
            request.BillingFirstnames = Billing.FirstName;
            request.BillingAddress1 = Billing.Address1;
            request.BillingCity = Billing.City;
            request.BillingPostCode = Billing.PostCode;
            request.BillingCountry = Billing.Country;
            request.DeliverySurname = Shipping.LastName;
            request.DeliveryFirstnames = Shipping.FirstName;
            request.DeliveryAddress1 = Shipping.Address1;
            request.DeliveryCity = Shipping.City;
            request.DeliveryPostCode = Shipping.PostCode;
            request.DeliveryCountry = Shipping.Country;
            //Optional
            request.CustomerEmail = Billing.Email;
            request.ApplyAvsCv2 = 2;
            request.Apply3dSecure = 0;  // set 1 for "Force accept 3D secure"
            request.ReferrerId = "";
            request.AccountType = "E";
            request.Profile = "NORMAL";
            request.BasketXml = "<basket><item><description>AirFare Ticket</description><quantity>1</quantity><unitNetAmount>" + Amount + "</unitNetAmount><unitTaxAmount>0</unitTaxAmount><unitGrossAmount>" + Amount + "</unitGrossAmount><totalGrossAmount>" + Amount + "</totalGrossAmount></item></basket>";
            request.SurchargeXml = "<surcharges><surcharge><paymentType>VISA</paymentType><percentage>0</percentage></surcharge><surcharge><paymentType>DELTA</paymentType><fixed>0</fixed></surcharge><surcharge><paymentType>AMEX</paymentType><percentage>0</percentage></surcharge><surcharge><paymentType>UKE</paymentType><percentage>0</percentage></surcharge><surcharge><paymentType>JCB</paymentType><percentage>0</percentage></surcharge><surcharge><paymentType>MAESTRO</paymentType><percentage>0</percentage></surcharge><surcharge><paymentType>MCDEBIT</paymentType><percentage>0</percentage></surcharge><surcharge><paymentType>MC</paymentType><percentage>0</percentage></surcharge></surcharges>";
            request.NotificationUrl = NotificationUrl;
            request.CreateToken = 0;// (cart.Card.SaveTokenForFutureUse == true ? 1 : 0);
        }

        public string GetNewVendorTxCode()
        {
            Random random = new Random();
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            // 18 char max -13 chars - 6 chars
            return string.Format("{0}-{1}-{2}",
                VendorName.Substring(0, Math.Min(18, VendorName.Length)),
                (long)ts.TotalMilliseconds, random.Next(100000, 999999));
        }
    }

    public class Billing
    {
        string _FirstName;
        string _LastName;
        string _Address1;
        string _Address2;
        string _City;
        string _PostCode;
        string _Country;
        string _CountryName;
        string _Region;
        string _Phone;
        string _Email;
        
        public string FirstName { get { return _FirstName; } set { _FirstName = value; } }
        public string LastName { get { return _LastName; } set { _LastName = value; } }
        public string Address1 { get { return _Address1; } set { _Address1 = value; } }
        public string Address2 { get { return _Address2; } set { _Address2 = value; } }
        public string City { get { return _City; } set { _City = value; } }
        public string PostCode { get { return _PostCode; } set { _PostCode = value; } }
        public string Country { get { return _Country; } set { _Country = value; } }
        public string CountryName { get { return _CountryName; } set { _CountryName = value; } }
        public string Region { get { return _Region; } set { _Region = value; } }
        public string Phone { get { return _Phone; } set { _Phone = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }

        public Billing()
        {
            _FirstName = string.Empty;
            _LastName = string.Empty;
            _Address1 = string.Empty;
            _Address2 = string.Empty;
            _City = string.Empty;
            _PostCode = string.Empty;
            _Country = string.Empty;
            _CountryName = string.Empty;
            _Region = string.Empty;
            _Phone = string.Empty;
            _Email = string.Empty;
        }
    }

    public class Booking
    {
        public string Currency { get; set; }
        public string TotalAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class Trustpilot
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string TimeAgo { get; set; }
    }

    public class HolidayOffer
    {
        public int Id { get; set; }
        public string DealType { get; set; }
        public string RefNo { get; set; }
        public string Title { get; set; }
        public int Discount { get; set; }
        public string Price { get; set; }
        public string Night { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
    }

    public class BestDeal
    {
        public int Id { get; set; }
        public string DealType { get; set; }
        public string RefNo { get; set; }
        public string Title { get; set; }
        public int Discount { get; set; }
        public string Price { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
    }

    public class QuickPayment
    {
        public string SiteCode { get; set; }
        public string TranscationId { get; set; }
        public string ReferenceId { get; set; }
        public string Amount { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
    }

    public class TravelGuide
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Category { get; set; }
        public string Continent { get; set; }
        public string Archive { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class MetaAirport
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CitySlug { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string StateSlug { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountrySlug { get; set; }
        public string Continent { get; set; }
        public string ContinentSlug { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string Airline { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
    }

    public class MetaAirline
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
        public string SideDescription { get; set; }
        
    }
}