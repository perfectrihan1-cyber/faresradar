using System;
using System.Collections.Generic;

namespace Viman.Models
{
    public class DuffelOfferRequest
    {
        public DuffelOfferRequestData data { get; set; }
    }

    public class DuffelOfferRequestData
    {
        public List<DuffelSlice> slices { get; set; }
        public List<DuffelPassenger> passengers { get; set; }
        public string cabin_class { get; set; }
        public int max_connections { get; set; } = 2; // Reduce flight overload on roundtrips
        public bool return_offers { get; set; } = true;
    }

    public class DuffelSlice
    {
        public string origin { get; set; }
        public string destination { get; set; }
        public string departure_date { get; set; }
    }

    public class DuffelPassenger
    {
        public string type { get; set; }
        public int? age { get; set; }
    }

    public class DuffelOfferResponse
    {
        public DuffelOfferResponseData data { get; set; }
    }

    public class DuffelOfferResponseData
    {
        public List<DuffelOffer> offers { get; set; }
    }

    public class DuffelOffer
    {
        public string id { get; set; }
        public string total_amount { get; set; }
        public string total_currency { get; set; }
        public string tax_amount { get; set; }
        public string tax_currency { get; set; }
        public string base_amount { get; set; }
        public string base_currency { get; set; }
        public DuffelOwner owner { get; set; }
        public List<DuffelOfferSlice> slices { get; set; }
        public List<DuffelOfferPassenger> passengers { get; set; }
    }

    public class DuffelOwner
    {
        public string name { get; set; }
        public string iata_code { get; set; }
    }

    public class DuffelOfferSlice
    {
        public string id { get; set; }
        public string duration { get; set; }
        public DuffelLocation origin { get; set; }
        public DuffelLocation destination { get; set; }
        public List<DuffelSegment> segments { get; set; }
    }

    public class DuffelLocation
    {
        public string type { get; set; }
        public string name { get; set; }
        public string iata_code { get; set; }
        public string iata_country_code { get; set; }
        public string iata_city_code { get; set; }
    }

    public class DuffelSegment
    {
        public string id { get; set; }
        public DuffelLocation origin { get; set; }
        public DuffelLocation destination { get; set; }
        public string origin_terminal { get; set; }
        public string destination_terminal { get; set; }
        public string departing_at { get; set; }
        public string arriving_at { get; set; }
        public string operating_carrier_flight_number { get; set; }
        public DuffelOwner operating_carrier { get; set; }
        public DuffelOwner marketing_carrier { get; set; }
        public string marketing_carrier_flight_number { get; set; }
        public string duration { get; set; }
        public DuffelAircraft aircraft { get; set; }
        public List<DuffelSegmentPassenger> passengers { get; set; }
    }

    public class DuffelAircraft
    {
        public string name { get; set; }
        public string iata_code { get; set; }
    }

    public class DuffelSegmentPassenger
    {
        public string passenger_id { get; set; }
        public string cabin_class { get; set; }
        public List<DuffelBaggage> baggages { get; set; }
    }

    public class DuffelBaggage
    {
        public string type { get; set; } // "checked" or "carry_on"
        public int quantity { get; set; }
    }

    public class DuffelOfferPassenger
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class DuffelOrderRequest
    {
        public DuffelOrderRequestData data { get; set; }
    }

    public class DuffelOrderRequestData
    {
        public string type { get; set; } = "instant";
        public List<string> selected_offers { get; set; }
        public List<DuffelOrderPayment> payments { get; set; }
        public List<DuffelOrderPassenger> passengers { get; set; }
    }

    public class DuffelOrderPayment
    {
        public string type { get; set; } // "balance"
        public string currency { get; set; }
        public string amount { get; set; }
    }

    public class DuffelOrderPassenger
    {
        public string id { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string born_on { get; set; }
        public string title { get; set; }
        public string gender { get; set; }
        public string family_name { get; set; }
        public string given_name { get; set; }
        public string infant_passenger_id { get; set; }
    }

    public class DuffelOrderResponse
    {
        public DuffelOrderResponseData data { get; set; }
    }

    public class DuffelOrderResponseData
    {
        public string id { get; set; }
        public string booking_reference { get; set; }
        public List<DuffelOrderPassenger> passengers { get; set; }
        public string total_amount { get; set; }
        public string total_currency { get; set; }
    }
}
