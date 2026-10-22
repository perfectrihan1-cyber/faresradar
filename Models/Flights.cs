using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Viman.Models
{
    public class Flights
    {
        public string Provider { get; set; }
        public string FlightId { get; set; }
        public string SessionId { get; set; }
        public FlightClass FlightClass { get; set; }
        public Airline Airline { get; set; }
        public string Currency { get; set; }
        public bool IsDeal { get; set; }
        public string DeepLink { get; set; }
        public int TotalTime { get; set; }
        public double TotalCost { get; set; }
        public int Stops { get; set; }
        public string FareRules { get; set; }
        public FlightsFare FlightFare { get; set; }
        public List<FlightsSegment> Outbound { get; set; }
        public List<FlightsSegment> Inbound { get; set; }
        public List<List<FlightsSegment>> OutboundList { get; set; }
        public List<List<FlightsSegment>> InboundList { get; set; }
        public Baglimit Baglimit { get; set; }
        public List<BagsPrice> ListBagsPrice { get; set; }

        public object ProviderData { get; set; }
    }
    public class BagsPrice
    {
        public string BagsPcs { get; set; }
        public float BagsPcsPrice { get; set; }
    }
    public class Baglimit
    {
        public string HandWidth { get; set; }
        public string HandHeight { get; set; }
        public string HandLength { get; set; }
        public string HandWeight { get; set; }
        public string HoldWidth { get; set; }
        public string HoldHeight { get; set; }
        public string HoldLength { get; set; }
        public string HoldWeight { get; set; }
        public string HoldDimensionsSum { get; set; }


      
    }
    public class FlightsSegment
    {
        public string OptionRef { get; set; }
        public string SegmentRef { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDateLegacy { get; set; }

        public DateTime ArrivalDateLegacy { get; set; }

       
        public DateTime Depdate1 { get; set; }
        public DateTime Arrdate1 { get; set; }

        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public Airport FromAirport { get; set; }
        public Airport ToAirport { get; set; }
        public Airline Airline { get; set; }
        public Airline OperatingAirline { get; set; }
        public string FromTerminal { get; set; }
        public string ToTerminal { get; set; }
        public string FlightNo { get; set; }
        public string EquipmentType { get; set; }
        public string Distance { get; set; }
        public string ETicket { get; set; }
        public string ChangePlane { get; set; }
        public string BaggageAllowance { get; set; }
        public string ElapsedTime { get; set; }
        public string TotalTime { get; set; }
        public string CabinClass { get; set; }
		public string Availability { get; set; }
        public string BookingCode { get; set; }
        public bool IsReturn { get; set; }
    }

    public class FlightsFare
    {
        public double AdultFare { get; set; }
        public double ChildFare { get; set; }
        public double InfantFare { get; set; }
        public double AdultTax { get; set; }
        public double ChildTax { get; set; }
        public double InfantTax { get; set; }
        public double AdultMarkup { get; set; }
        public double ChildMarkup { get; set; }
        public double InfantMarkup { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }
        public double AtolCharge { get; set; }
        public double CardCharge { get; set; }
        public string MarkupType { get; set; }
        public double AvgCost
        {
            set { }
            get
            {
                return (GrandTotal / (Adult + Child + Infant));
            }
        }
        public double GrandTotal
        {
            set { }
            get
            {
                return (((AdultFare + AdultMarkup + AdultTax) * Adult) + ((ChildFare + ChildMarkup + ChildTax) * Child) + ((InfantFare + InfantMarkup + InfantTax) * Infant));
                               
            }
        }
    }

    public class FlightsSearch
    {
        public long Id { get; set; }
        public string SearchId { get; set; }
        public Airport From { get; set; }
        public Airport To { get; set; }
        public DateTime DepDate { get; set; }
        public DateTime RetDate { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }
        public FlightWay FlightWay { get; set; }
        public FlightClass FlightClass { get; set; }
        public Airline Airline { get; set; }
        public bool IsDirect { get; set; }
        public bool IsFlexi { get; set; }
        public string Currency { get; set; }
        public string SiteCode { get; set; }
        public string SourceMedia { get; set; }
        public bool IsDeepLink { get; set; }
        public string ApiKey { get; set; }
    }

    public class MetaFlightSearch
    {
        public Airport From { get; set; }
        public Airport To { get; set; }
        public Airline Airline { get; set; }
        public float Amount { get; set; }
        public FlightWay FlightWay { get; set; }
        public DateTime TripDate { get; set; }
        public DateTime SearchDate { get; set; }
    }

    public class FlightsResult
    {
        public string Error { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public List<FlightsExtra> StopExtra { get; set; }
        public List<FlightsExtra> BarExtra { get; set; }
        public List<FlightsExtra> AirlineExtra { get; set; }

        public List<Airline> Airline { get; set; }
        public FlightsSearch FlightsSearch { get; set; }
        public List<Flights> FlightsList { get; set; }
    }

    public class FilterResult
    {
        public string Airline { get; set; }
        public float Price { get; set; }
        public List<FilterSegment> OutboundFilter { get; set; }
        public List<FilterSegment> InboundFilter { get; set; }
    }
    public class FilterSegment
    {
        public int Stops { get; set; }
        public int PlaneTime { get; set; }
    }

    public class FlightsExtra
    {
        public string FlightId { get; set; }
        public string ExtraType { get; set; }
        public Airline Airline { get; set; }
        public double TotalCost { get; set; }
        public int TotalTime { get; set; }
        public int TotalStop { get; set; }
    }

    public class Airport
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Continent { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
    }

    public class Airline
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Markup
    {
        public double AdultMarkup { get; set; }
        public double ChildMarkup { get; set; }
        public double InfantMarkup { get; set; }
    }

    public class BookingToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryAt { get; set; }
        public bool IsUsed { get; set; }
    }
}