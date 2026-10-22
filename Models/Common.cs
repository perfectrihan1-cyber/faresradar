using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using SqlData.Data;
using Viman.Models;
using System.Security.Cryptography;
using System.Net.Mail;
using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using static System.Collections.Specialized.BitVector32;
using System.Drawing.Printing;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;

namespace Viman.Models
{
   
    public class Common
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static string WebsiteURL = ConfigurationManager.AppSettings["WebsiteURL"];
        public static string StaticURL = ConfigurationManager.AppSettings["StaticURL"];
        public static string Currency = ConfigurationManager.AppSettings["Currency"];
        public static string CurrencyCode = ConfigurationManager.AppSettings["CurrencyCode"];

        public static string GetId()
        {
            return Guid.NewGuid().ToString();
        }
        public static string Encrypt(string PlainText)
        {
            string _securityKey = "FLIGHTOFFICEUK";

            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));
            objMD5CryptoService.Clear();
            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            objTripleDESCryptoService.Key = securityKeyArray;
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;
            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);

            objTripleDESCryptoService.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string CipherText)
        {
            string _securityKey = "FLIGHTOFFICEUK";

            byte[] toEncryptArray = Convert.FromBase64String((CipherText).Replace(" ", "+"));
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));
            objMD5CryptoService.Clear();
            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            objTripleDESCryptoService.Key = securityKeyArray;
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;
            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static Airport GetAirport(string Code)
        {
            Airport airport = new Airport();

            DataSet ds = SqlHelper.ExecuteDataset("SELECT * FROM Airport WHERE Code='" + Code + "'", null);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                airport.Code = dr["Code"].ToString();
                airport.Name = dr["Name"].ToString();
                airport.CityCode = dr["CityCode"].ToString();
                airport.CityName = dr["CityName"].ToString();
                airport.StateCode = dr["StateCode"].ToString();
                airport.StateName = dr["StateName"].ToString();
                airport.CountryCode = dr["CountryCode"].ToString();
                airport.CountryName = dr["CountryName"].ToString();
                airport.Continent = dr["Continent"].ToString();
                airport.Latitude = dr["Latitude"].ToString();
                airport.Longtitude = dr["Longtitude"].ToString();
            }
            else
            {
                airport.Code = Code;
                airport.Name = Code;
                airport.CityCode = Code;
                airport.CityName = Code;
                airport.StateCode = Code;
                airport.StateName = Code;
                airport.CountryCode = Code;
                airport.CountryName = Code;
                airport.Continent = Code;
                airport.Latitude = Code;
                airport.Longtitude = Code;
            }

            return airport;
        }

        public static Airline GetAirline(string Code)
        {
            Airline airline = new Airline();

            DataSet ds = SqlHelper.ExecuteDataset("SELECT * FROM Airline WHERE Code='" + Code + "'", null);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                airline.Code = dr["Code"].ToString();
                airline.Name = dr["Name"].ToString();
            }
            else
            {
                airline.Code = Code;
                airline.Name = Code;
            }

            return airline;
        }

        public static List<Airline> GetAllAirline()
        {
            List<Airline> airline = new List<Airline>();

            DataSet ds = SqlHelper.ExecuteDataset("SELECT * FROM Airline ORDER BY Name", null);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    airline.Add(new Airline()
                    {
                        Code = dr["Code"].ToString(),
                        Name = dr["Name"].ToString()
                    });
                }
            }

            return airline;
        }

        public static string UpperCaseWords(string value)
        {
            value = value.ToLower();
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        public static int GetTime(DateTime d2, DateTime d1)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalMinutes;
        }


        public static async Task<FlightsResult> SearchFlightAsync(FlightsSearch flightSearch)
        {
            FlightsResult resultModel = new FlightsResult { FlightsList = new List<Flights>() };
            if (flightSearch == null) return resultModel;

        
            var duffelTask = SearchDuffelAsync(flightSearch);

            await Task.WhenAll(duffelTask);

            
            var duffelResult = duffelTask.Result;

           

            if (duffelResult?.FlightsList != null)
                resultModel.FlightsList.AddRange(duffelResult.FlightsList);

            // Populate airline names from database for combined list (in case Duffel needs it or Etrav missed it)
            var distinctAirlines = resultModel.FlightsList
                .Where(f => f.Airline != null && !string.IsNullOrEmpty(f.Airline.Code))
                .Select(f => f.Airline.Code)
                .Distinct()
                .ToList();

            foreach (var code in distinctAirlines)
            {
                var airlineDetails = GetAirport(code);
                if (airlineDetails != null && !string.IsNullOrEmpty(airlineDetails.Name))
                {
                    foreach (var flight in resultModel.FlightsList.Where(f => f.Airline?.Code == code))
                    {
                        if (flight.Airline != null && (string.IsNullOrEmpty(flight.Airline.Name) || flight.Airline.Name == flight.Airline.Code))
                        {
                            flight.Airline.Name = airlineDetails.Name;
                        }
                    }
                }
            }

            // Order by price and aggressively limit to top 50 to prevent frontend UI freeze
            resultModel.FlightsList = resultModel.FlightsList.OrderBy(f => f.TotalCost).ToList();
            resultModel.FlightsSearch = flightSearch;

            // Combine errors if any
            var errors = new List<string>();
            
            if (!string.IsNullOrEmpty(duffelResult?.Error)) errors.Add(duffelResult.Error);
            if (errors.Any() && !resultModel.FlightsList.Any())
                resultModel.Error = string.Join(" | ", errors);

            return resultModel;
        }

        private static  async Task<FlightsResult> SearchDuffelAsync(FlightsSearch flightSearch)
        {
            FlightsResult resultModel = new FlightsResult { FlightsList = new List<Flights>() };
            try
            {
                string duffelBaseUrl = "https://api.duffel.com";
                string duffelAccessToken = "duffel_live_iCoMKWZ3bOfvy-VogysJeP73dLjYKEXv4jZn7iqGOF8";

                if (string.IsNullOrEmpty(duffelAccessToken)) return resultModel;

                var slices = new List<DuffelSlice>();
                slices.Add(new DuffelSlice
                {
                    origin = flightSearch.From?.Code ?? "",
                    destination = flightSearch.To?.Code ?? "",
                    departure_date = flightSearch.DepDate.ToString("yyyy-MM-dd")
                });

                if (flightSearch.FlightWay == FlightWay.RoundTrip)
                {
                    slices.Add(new DuffelSlice
                    {
                        origin = flightSearch.To?.Code ?? "",
                        destination = flightSearch.From?.Code ?? "",
                        departure_date = flightSearch.RetDate.ToString("yyyy-MM-dd")
                    });
                }

                var passengers = new List<DuffelPassenger>();
                for (int i = 0; i < flightSearch.Adult; i++) passengers.Add(new DuffelPassenger { type = "adult" });
                for (int i = 0; i < flightSearch.Child; i++) passengers.Add(new DuffelPassenger { age = 10 });
                for (int i = 0; i < flightSearch.Infant; i++) passengers.Add(new DuffelPassenger { age = 1 });

                string cabinClass = "economy";

                if (flightSearch.FlightClass == FlightClass.EconomyPremium)
                    cabinClass = "premium_economy";
                else if (flightSearch.FlightClass == FlightClass.Business)
                    cabinClass = "business";
                else if (flightSearch.FlightClass == FlightClass.First)
                    cabinClass = "first";

                var duffelRequest = new DuffelOfferRequest
                {
                    data = new DuffelOfferRequestData
                    {
                        slices = slices,
                        passengers = passengers,
                        cabin_class = cabinClass,
                        return_offers = true
                    }
                };

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Duffel-Version", "v2");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {duffelAccessToken}");
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                string url = duffelBaseUrl.TrimEnd('/') + "/air/offer_requests";
                var jsonContent = JsonConvert.SerializeObject(duffelRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                // DEBUG LOGGING TO FILE
                try
                {
                    string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "File", $"DuffelLog_{flightSearch.SearchId}.txt");
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "File"));
                    System.IO.File.WriteAllText(logPath, $"[DUFFEL DEBUG] Request JSON: {jsonContent}\n[DUFFEL DEBUG] Response Status Code: {response.StatusCode}\n[DUFFEL DEBUG] Response JSON: {responseString}");
                }
                catch { }

                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Error = $"Duffel API error: {(int)response.StatusCode} - {responseString}";
                    return resultModel;
                }

                var duffelResponse = JsonConvert.DeserializeObject<DuffelOfferResponse>(responseString);

                if (duffelResponse?.data?.offers != null && duffelResponse.data.offers.Any())
                {
                    foreach (var offer in duffelResponse.data.offers)
                    {
                        var mappedFlight = MapDuffelOfferToFlights(offer, flightSearch);
                        if (mappedFlight != null)
                            resultModel.FlightsList.Add(mappedFlight);
                    }
                }
                else
                {
                    resultModel.Error = $"Duffel returned success but no offers. Response JSON: {responseString}";
                }
            }
            catch
            {

            }

            

            return resultModel;
        }

        public static async Task<bool> BookDuffelFlightAsync(FlightsBooking bookFlights)
        {
            try
            {
                if (bookFlights?.Flights?.Provider != "Duffel") return true;
                if (bookFlights.Flights.ProviderData == null) return false;

                string duffelBaseUrl = "https://api.duffel.com";
                string duffelAccessToken = "duffel_live_iCoMKWZ3bOfvy-VogysJeP73dLjYKEXv4jZn7iqGOF8";

                if (string.IsNullOrEmpty(duffelAccessToken)) return false;

                var offerPaxStr = bookFlights.Flights.ProviderData.ToString();
                var offerPassengers = JsonConvert.DeserializeObject<List<DuffelOfferPassenger>>(offerPaxStr);

                var orderPassengers = new List<DuffelOrderPassenger>();
                int paxIndex = 0;

                // Track infants to assign to adults mapping
                var adultPassengers = new List<DuffelOrderPassenger>();
                var infantPassengers = new List<DuffelOrderPassenger>();

                foreach (var pax in bookFlights.PassengerDetail)
                {
                    if (paxIndex >= offerPassengers.Count) break;
                    var offerPax = offerPassengers[paxIndex++];

                    var rawPhone = bookFlights.PhoneNo ?? "";
                    var safePhone = string.IsNullOrEmpty(rawPhone) ? "+14155552671" : (rawPhone.StartsWith("+") ? rawPhone : "+1" + rawPhone);
                    if (safePhone == "+19876543210" || safePhone == "+19999999999" || safePhone == "+10000000000" || safePhone == "+11234567890") safePhone = "+14155552671";

                    var orderPax = new DuffelOrderPassenger
                    {
                        id = offerPax.id,
                        phone_number = safePhone,
                        email = string.IsNullOrEmpty(bookFlights.Email) ? "guest@example.com" : bookFlights.Email,
                        born_on = pax.DOB.ToString("yyyy-MM-dd"),
                        title = string.IsNullOrEmpty(pax.Title) ? "mr" : pax.Title.ToLower(),
                        gender = string.IsNullOrEmpty(pax.Gender) ? "m" : (pax.Gender.ToLower().StartsWith("m") ? "m" : "f"),
                        family_name = string.IsNullOrEmpty(pax.LastName) ? "Traveler" : pax.LastName,
                        given_name = string.IsNullOrEmpty(pax.FirstName) ? "Valued" : pax.FirstName
                    };

                    orderPassengers.Add(orderPax);

                    if (pax.PassengerType == PassengerType.Adult) adultPassengers.Add(orderPax);
                    if (pax.PassengerType == PassengerType.Infant) infantPassengers.Add(orderPax);
                }

                // Assign infants to adults
                for (int i = 0; i < Math.Min(adultPassengers.Count, infantPassengers.Count); i++)
                {
                    infantPassengers[i].infant_passenger_id = adultPassengers[i].id;
                    // API rule: "assign the infant's passenger id to the responsible adult's infant_passenger_id"
                    adultPassengers[i].infant_passenger_id = infantPassengers[i].id;
                }

                var request = new DuffelOrderRequest
                {
                    data = new DuffelOrderRequestData
                    {
                        type = "instant",
                        selected_offers = new List<string> { bookFlights.Flights.FlightId },
                        payments = new List<DuffelOrderPayment>
                        {
                            new DuffelOrderPayment
                            {
                                type = "balance",
                                currency = bookFlights.Flights.Currency,
                                amount = bookFlights.Flights.TotalCost.ToString("0.00")
                            }
                        },
                        passengers = orderPassengers
                    }
                };

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Duffel-Version", "v2");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {duffelAccessToken}");
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                string url = duffelBaseUrl.TrimEnd('/') + "/air/orders";
                var jsonContent = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                try
                {
                    string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "File", $"DuffelOrderLog_{bookFlights.BookingId}.txt");
                    System.IO.File.WriteAllText(logPath, $"[DUFFEL ORDER] Request JSON: {jsonContent}\n[DUFFEL ORDER] Response Status Code: {response.StatusCode}\n[DUFFEL ORDER] Response JSON: {responseString}");
                }
                catch { }

                if (!response.IsSuccessStatusCode)
                {
                    bookFlights.WorldResponse = "Duffel API order error: " + responseString;
                    return false;
                }

                var orderResp = JsonConvert.DeserializeObject<DuffelOrderResponse>(responseString);
                if (orderResp?.data != null)
                {
                    bookFlights.PNRNo = orderResp.data.booking_reference;
                    bookFlights.WorldResponse = "Duffel Order ID: " + orderResp.data.id;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                bookFlights.WorldResponse = "Duffel Exception: " + ex.Message;
                return false;
            }
        }

        private static Flights MapDuffelOfferToFlights(DuffelOffer offer, FlightsSearch flightSearch)
        {
            if (offer == null || offer.slices == null || !offer.slices.Any()) return null;

            double totalAmt;
            double.TryParse(offer.total_amount, out totalAmt);

            double baseAmt;
            double.TryParse(offer.base_amount, out baseAmt);

            double taxAmt;
            double.TryParse(offer.tax_amount, out taxAmt);

            // In Duffel, currency is usually the same as requested, but if not we might need to convert. 
            // We assume it returns USD or we convert it like Etrav if needed. Let's assume it returns requested or standard.
            FlightsFare flightFare = new FlightsFare
            {
                Adult = flightSearch.Adult,
                Child = flightSearch.Child,
                Infant = flightSearch.Infant,
                GrandTotal = totalAmt,
                // Duffel doesn't easily split fare by pax type out of the box without iterating passengers array and matching.
                // We will split evenly or just map it all to Adult for aggregate view, similar to some simple integrations,
                // But let's try to map realistically if needed, or just set AdultFare.
                AdultFare = baseAmt,
                AdultTax = taxAmt,
                AdultMarkup = 0
            };

            var mappedFlight = new Flights
            {
                Provider = "Duffel",
                FlightId = offer.id,
                SessionId = offer.id,
                FlightClass = flightSearch.FlightClass,
                Airline = new Airline { Code = offer.owner?.iata_code ?? "", Name = offer.owner?.name ?? "" },
                Currency = offer.total_currency ?? "USD",
                TotalCost = totalAmt,
                FlightFare = flightFare,
                ProviderData = JsonConvert.SerializeObject(offer.passengers)
            };

            // Parse slices
            var onwardSlice = offer.slices.FirstOrDefault();
            var returnSlice = offer.slices.Count > 1 ? offer.slices[1] : null;

            if (onwardSlice != null)
            {
                mappedFlight.Outbound = MapDuffelSegments(onwardSlice.segments, false);
                mappedFlight.OutboundList = new List<List<FlightsSegment>>();
                if (mappedFlight.Outbound.Any()) mappedFlight.OutboundList.Add(mappedFlight.Outbound);
                mappedFlight.Stops = Math.Max(0, mappedFlight.Outbound.Count - 1);
            }

            if (returnSlice != null)
            {
                mappedFlight.Inbound = MapDuffelSegments(returnSlice.segments, true);
                mappedFlight.InboundList = new List<List<FlightsSegment>>();
                if (mappedFlight.Inbound.Any()) mappedFlight.InboundList.Add(mappedFlight.Inbound);
            }

            // Calculate TotalTime
            int totalMins = 0;
            if (onwardSlice != null && !string.IsNullOrEmpty(onwardSlice.duration))
                totalMins += ParseDuffelDuration(onwardSlice.duration);
            if (returnSlice != null && !string.IsNullOrEmpty(returnSlice.duration))
                totalMins += ParseDuffelDuration(returnSlice.duration);

            mappedFlight.TotalTime = totalMins;

            // Bag allowance
            var firstPax = offer.passengers?.FirstOrDefault();
            if (firstPax != null && onwardSlice?.segments != null)
            {
                var segPax = onwardSlice.segments.FirstOrDefault()?.passengers?.FirstOrDefault(p => p.passenger_id == firstPax.id);
                if (segPax != null && segPax.baggages != null)
                {
                    mappedFlight.Baglimit = new Baglimit
                    {
                        HoldWeight = segPax.baggages.Where(b => b.type == "checked").Sum(b => b.quantity).ToString() + " Piece(s)",
                        HandWeight = segPax.baggages.Where(b => b.type == "carry_on").Sum(b => b.quantity).ToString() + " Piece(s)"
                    };
                }
            }

            return mappedFlight;
        }

        private static List<FlightsSegment> MapDuffelSegments(List<DuffelSegment> segments, bool isReturn)
        {
            if (segments == null) return new List<FlightsSegment>();
            var result = new List<FlightsSegment>();

            int cumulativeMinutes = 0;
            DateTime? lastArrDate = null;

            foreach (var seg in segments)
            {
                DateTime depDate;
                DateTime.TryParse(seg.departing_at, out depDate);

                DateTime arrDate;
                DateTime.TryParse(seg.arriving_at, out arrDate);

                if (lastArrDate.HasValue && depDate > lastArrDate.Value)
                {
                    int layoverMins = (int)(depDate - lastArrDate.Value).TotalMinutes;
                    if (layoverMins < 24 * 60) cumulativeMinutes += layoverMins;
                }
                lastArrDate = arrDate;

                int segmentMins = ParseDuffelDuration(seg.duration);
                if (segmentMins == 0 && depDate != DateTime.MinValue && arrDate != DateTime.MinValue)
                {
                    segmentMins = (int)(arrDate - depDate).TotalMinutes;
                }
                cumulativeMinutes += segmentMins;

                result.Add(new FlightsSegment
                {
                    SegmentRef = seg.id,
                    DepartureDate = depDate,
                    ArrivalDate = arrDate,
                    DepartureDateLegacy = depDate,
                    ArrivalDateLegacy = arrDate,
                    DepartureTime = depDate.ToString("HH:mm"),
                    ArrivalTime = arrDate.ToString("HH:mm"),
                    FromAirport = new Airport { Code = seg.origin?.iata_code ?? "", Name = seg.origin?.name ?? "" },
                    ToAirport = new Airport { Code = seg.destination?.iata_code ?? "", Name = seg.destination?.name ?? "" },
                    Airline = new Airline { Code = seg.operating_carrier?.iata_code ?? seg.marketing_carrier?.iata_code ?? "", Name = seg.operating_carrier?.name ?? seg.marketing_carrier?.name ?? "" },
                    OperatingAirline = new Airline { Code = seg.operating_carrier?.iata_code ?? "", Name = seg.operating_carrier?.name ?? "" },
                    FromTerminal = seg.origin_terminal ?? "",
                    ToTerminal = seg.destination_terminal ?? "",
                    FlightNo = seg.operating_carrier_flight_number ?? seg.marketing_carrier_flight_number ?? "",
                    EquipmentType = seg.aircraft?.name ?? "",
                    ElapsedTime = segmentMins.ToString(),
                    TotalTime = cumulativeMinutes.ToString(),
                    IsReturn = isReturn,
                    BaggageAllowance = "",
                    CabinClass = seg.passengers?.FirstOrDefault()?.cabin_class ?? ""
                });
            }
            return result;
        }

        private static int ParseDuffelDuration(string duration)
        {
            // Duffel returns ISO 8601 durations like "PT2H30M"
            try
            {
                if (string.IsNullOrEmpty(duration)) return 0;
                var ts = System.Xml.XmlConvert.ToTimeSpan(duration);
                return (int)ts.TotalMinutes;
            }
            catch { return 0; }
        }


        public static FlightsResult FillFlightPrice(FlightsResult flight)
        {
            FlightsResult newFlight = new FlightsResult
            {
                Error = flight.Error,
                FlightsSearch = flight.FlightsSearch,
                FlightsList = new List<Flights>(),
                StopExtra = new List<FlightsExtra>(),
                AirlineExtra = new List<FlightsExtra>(),
                BarExtra = new List<FlightsExtra>()
            };

            // Initialize StopExtra with 3 elements for Direct, 1 Stop, 2+ Stops
            for (int i = 0; i < 3; i++)
            {
                newFlight.StopExtra.Add(new FlightsExtra { TotalStop = i, TotalCost = 0, ExtraType = "Stop" });
            }

            if (flight.FlightsList != null && flight.FlightsList.Any())
            {
                newFlight.FlightsList.AddRange(flight.FlightsList);

                // 1. Min/Max Price global
                newFlight.MinPrice = (int)Math.Floor(newFlight.FlightsList.Min(f => f.TotalCost));
                newFlight.MaxPrice = (int)Math.Ceiling(newFlight.FlightsList.Max(f => f.TotalCost));

                // 2. Populate StopExtra
                foreach (var f in newFlight.FlightsList)
                {
                    int stopCount = 0;
                    if (f.OutboundList != null && f.OutboundList.Count > 0 && f.OutboundList[0] != null)
                    {
                        stopCount = f.OutboundList[0].Count - 1;
                    }
                    int stopIndex = (stopCount > 2) ? 2 : stopCount;
                    if (stopIndex >= 0)
                    {
                        if (newFlight.StopExtra[stopIndex].TotalCost == 0 || f.TotalCost < newFlight.StopExtra[stopIndex].TotalCost)
                        {
                            newFlight.StopExtra[stopIndex].TotalCost = f.TotalCost;
                        }
                    }
                }

                // 3. Populate AirlineExtra
                var airlineGroups = newFlight.FlightsList
                    .Where(f => f.Airline != null)
                    .GroupBy(f => f.Airline.Code)
                    .Select(g => new
                    {
                        Airline = g.First().Airline,
                        MinPrice = g.Min(x => x.TotalCost),
                        TotalStop = g.First().OutboundList != null && g.First().OutboundList.Count > 0 ? g.First().OutboundList[0].Count - 1 : 0
                    }).ToList();

                foreach (var grp in airlineGroups)
                {
                    newFlight.AirlineExtra.Add(new FlightsExtra
                    {
                        Airline = grp.Airline,
                        TotalCost = grp.MinPrice,
                        ExtraType = "Airline",
                        TotalStop = grp.TotalStop
                    });
                }

                newFlight.Airline = new List<Airline>();
                foreach (var item in newFlight.AirlineExtra)
                {
                    if (!newFlight.Airline.Any(a => a.Code == item.Airline.Code))
                    {
                        newFlight.Airline.Add(item.Airline);
                    }
                }

                // 4. Populate BarExtra
                var cheapest = newFlight.FlightsList.OrderBy(f => f.TotalCost).FirstOrDefault();
                if (cheapest != null)
                {
                    newFlight.BarExtra.Add(new FlightsExtra { ExtraType = "cheapest", TotalCost = cheapest.TotalCost, TotalTime = cheapest.TotalTime, FlightId = cheapest.FlightId, Airline = cheapest.Airline });
                    newFlight.BarExtra.Add(new FlightsExtra { ExtraType = "best", TotalCost = cheapest.TotalCost, TotalTime = cheapest.TotalTime, FlightId = cheapest.FlightId, Airline = cheapest.Airline });
                }

                var shortest = newFlight.FlightsList.OrderBy(f => f.TotalTime).FirstOrDefault();
                if (shortest != null)
                {
                    newFlight.BarExtra.Add(new FlightsExtra { ExtraType = "shortest", TotalCost = shortest.TotalCost, TotalTime = shortest.TotalTime, FlightId = shortest.FlightId, Airline = shortest.Airline });
                }
            }
            if (!newFlight.FlightsList.Any())
            {
                // Ensure lists are not null even if no flights
            }

            return newFlight;
        }

        public static List<FilterResult> FilterFlight(FlightsResult flight)
        {
            List<FilterResult> filterSegment = new List<FilterResult>();
            if (flight?.FlightsList == null) return filterSegment;

            foreach (Flights flightItem in flight.FlightsList)
            {
                try
                {
                    List<FilterSegment> outboundFilter = new List<FilterSegment>();
                    List<FilterSegment> inboundFilter = new List<FilterSegment>();

                    if (flightItem.OutboundList != null)
                    {
                        foreach (List<FlightsSegment> outbound in flightItem.OutboundList)
                        {
                            if (outbound != null && outbound.Any())
                            {
                                outboundFilter.Add(new FilterSegment
                                {
                                    Stops = (outbound.Count - 1),
                                    PlaneTime = int.Parse(outbound[0].DepartureDate.ToString("HHmm"))
                                });
                            }
                        }
                    }

                    if (flight.FlightsSearch.FlightWay == FlightWay.RoundTrip && flightItem.InboundList != null)
                    {
                        foreach (List<FlightsSegment> inbound in flightItem.InboundList)
                        {
                            if (inbound != null && inbound.Any())
                            {
                                inboundFilter.Add(new FilterSegment
                                {
                                    Stops = (inbound.Count - 1),
                                    PlaneTime = int.Parse(inbound[0].DepartureDate.ToString("HHmm"))
                                });
                            }
                        }
                    }

                    string airlineCode = "";
                    if (flightItem.OutboundList != null && flightItem.OutboundList.Count > 0 && flightItem.OutboundList[0].Count > 0)
                    {
                        airlineCode = flightItem.OutboundList[0][0].Airline?.Code ?? "";
                    }
                    else if (flightItem.Airline != null)
                    {
                        airlineCode = flightItem.Airline.Code;
                    }

                    filterSegment.Add(new FilterResult
                    {
                        Price = float.Parse(flightItem.TotalCost.ToString("0.00")),
                        OutboundFilter = outboundFilter,
                        InboundFilter = inboundFilter,
                        Airline = airlineCode
                    });
                }
                catch (Exception ex)
                {
                }
            }

            return filterSegment;
        }



        public static string GetBookingId(string pre = null)
        {
            string str = "";

            DataSet ds = SqlHelper.ExecuteDataset("SELECT TOP 1 * FROM FlightsBooking ORDER BY Id DESC", null);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string max_id = (int.Parse(dr["Id"].ToString()) + 1) + "";

                str = pre + max_id.PadLeft(6, '0');
            }
            else
            {
                str = pre + "000001";
            }

            return str;
        }


        public static List<Country> GetAllCountry(string str = null)
        {
            List<Country> country = new List<Country>();
            string qry = "SELECT * FROM Country";
            if (str != null && !str.Equals(""))
                qry = "SELECT * FROM Country WHERE Code LIKE '" + str + "%' OR Name LIKE '" + str + "%'";
            qry += " ORDER BY Name";

            DataSet ds = SqlHelper.ExecuteDataset(qry, null);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    country.Add(new Country()
                    {
                        Code = dr["Code"].ToString(),
                        Name = dr["Name"].ToString()
                    });
                }
            }

            return country;
        }

        public static FlightsBooking CheckFareMAN(FlightsBooking flightsBooking)
        {
            FlightsBooking responseFlights = new FlightsBooking();
            responseFlights = flightsBooking;

            try
            {
                if (flightsBooking != null)
                {
                    responseFlights.FlightsFare = flightsBooking.Flights.FlightFare;
                    responseFlights.TranscationStatus = "Connection error";
                }
            }
            catch (Exception e)
            {
                responseFlights.FlightsFare = flightsBooking.Flights.FlightFare;
                responseFlights.TranscationStatus = e.Message.ToString();
            }
            return responseFlights;
        }


        public static string GetLocalIP()
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            return ip;
            
        }

        public static FlightsBooking GetFlightByIdFlightoffice(string fid)
        {
            FlightsBooking flightsBooking = new FlightsBooking();

            DataSet ds = SqlHelper.ExecuteDatasetFlightoffice("SELECT * FROM FlightsBooking WHERE TranscationId='" + fid + "'", null);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                flightsBooking.Id = long.Parse(dr["Id"].ToString());
                flightsBooking.BookingId = dr["BookingId"].ToString();
                flightsBooking.SearchId = dr["SearchId"].ToString();
                flightsBooking.TranscationId = dr["TranscationId"].ToString();
                flightsBooking.PNRNo = dr["PNRNo"].ToString();
                flightsBooking.FullName = dr["FullName"].ToString();
                flightsBooking.Email = dr["Email"].ToString();
                flightsBooking.PhoneNo = dr["PhoneNo"].ToString();
                flightsBooking.FlightsSearch = JsonConvert.DeserializeObject<FlightsSearch>(dr["FlightsSearch"].ToString());
                flightsBooking.Flights = JsonConvert.DeserializeObject<Flights>(dr["Flights"].ToString());
                flightsBooking.BookerDetail = JsonConvert.DeserializeObject<BookerDetail>(dr["BookerDetail"].ToString());
                flightsBooking.PassengerDetail = JsonConvert.DeserializeObject<List<PassengerDetail>>(dr["PassengerDetail"].ToString());
                flightsBooking.PaymentDetail = JsonConvert.DeserializeObject<PaymentDetail>(dr["PaymentDetail"].ToString());
                flightsBooking.SagepayDetail = JsonConvert.DeserializeObject<SagepayDetail>(dr["SagepayDetail"].ToString());
                flightsBooking.SiteCode = dr["SiteCode"].ToString();
                flightsBooking.SourceMedia = dr["SourceMedia"].ToString();
                flightsBooking.FlightsFare = JsonConvert.DeserializeObject<FlightsFare>(dr["FlightsFare"].ToString());
                flightsBooking.PaymentStatus = dr["PaymentStatus"].ToString();
                flightsBooking.WorldResponse = dr["WorldResponse"].ToString();
                flightsBooking.IsHotel = dr["IsHotel"].ToString();
                flightsBooking.TranscationStatus = dr["TranscationStatus"].ToString();
                flightsBooking.Status = dr["Status"].ToString();
                flightsBooking.Timestamp = Convert.ToDateTime(dr["Timestamp"]);
                flightsBooking.BagPrice = JsonConvert.DeserializeObject<BagsPrice>(dr["BagsPrice"].ToString());
            }

            return flightsBooking;
        }
        public class Country
        {
           
            public long Id { get; set; }

           
            public string Code { get; set; }

           
            public string Name { get; set; }
        }

       
        public class City
        {
            
            public long Id { get; set; }

            
            public string CityId { get; set; }

           
            public string TravellandaCityId { get; set; }

          
            public string CityName { get; set; }

          
            public string StateCode { get; set; }

           
           
            public string CountryCode { get; set; }

            
            public string CountryName { get; set; }

           
            public string SearchName { get; set; }
        }

       
        public class State
        {
           
            public long Id { get; set; }

           
            public string Code { get; set; }

           
            public string Name { get; set; }

           
            public string CountryCode { get; set; }
        }


        public class TrainSearchModel
        {
            public int flight_way { get; set; }
            public string flight_fromtrain { get; set; }
            public string flight_totrain { get; set; }
            public string flight_from_date { get; set; }
            public string flight_to_date { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phoneno { get; set; }
            public int flight_adult { get; set; }
            public int flight_child { get; set; }
            public int flight_infant { get; set; }
        }




    }
}


      




        



