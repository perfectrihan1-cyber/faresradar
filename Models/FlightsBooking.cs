using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Viman.Models
{
    public class FlightsBooking
    {
        public long Id { get; set; }
        public string SearchId { get; set; }
        public string BookingId { get; set; }
        public string TranscationId { get; set; }
        public string PNRNo { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public FlightsSearch FlightsSearch { get; set; }
        public Flights Flights { get; set; }
        public BookerDetail BookerDetail { get; set; }
        public List<PassengerDetail> PassengerDetail { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        public SagepayDetail SagepayDetail { get; set; }
        public string SiteCode { get; set; }
        public string SourceMedia { get; set; }
        public string SupplierCode { get; set; }
        public FlightsFare SupplierFare { get; set; }
        public string SupplierStatus { get; set; }
        public string SupplierComment { get; set; }
        public FlightsFare FlightsFare { get; set; }
        public string AgentCode { get; set; }
        public string PaymentStatus { get; set; }
        public string AgentStatus { get; set; }
        public string AgentComment { get; set; }
        public string AdminStatus { get; set; }
        public string AdminComment { get; set; }
        public bool UpdateFare { get; set; }
        public string WorldResponse { get; set; }
        public string TranscationStatus { get; set; }
        public string IsHotel { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public BagsPrice BagPrice { get; set; }
        public float FreeChangeAmt { get; set; }
    }

    public class BookerDetail
    {
        public string Title { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string AlternativeEmail { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
    }

    public class PassengerDetail
    {
        public int TravelerNo { get; set; }
        public PassengerType PassengerType { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        
        public string PassportNumber { get; set; }
        public string Nationality { get; set; }
        public string IssueCountry { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class PaymentDetail
    {
        public string CardCode { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVVNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
    }

    public class SagepayDetail
    {
        public string BookingId { get; set; }
        public string TranscationId { get; set; }
        public string CryptVal { get; set; }
        public string VendorTxCode { get; set; }
        public string BankAuthCode { get; set; }
        public string CardType { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string Last4Digits { get; set; }
        public string ExpiryDate { get; set; }
        public string TxAuthNo { get; set; }
        public string VpsTxId { get; set; }
    }
}