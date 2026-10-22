using Newtonsoft.Json;
using SqlData.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viman.Models;
using System.IO;
using Newtonsoft.Json.Serialization;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Security.Policy;



namespace Viman.Controllers
{
    public class FlightController : Controller
    {
        string tranId = String.Empty;
        string guid = String.Empty;
        string searchId = String.Empty;
        string out_id = String.Empty;
        string in_id = String.Empty;


        // GET: Flight
        public ActionResult Index()
        {


            return View();
        }





        //public async Task<ActionResult> Search()
        //{
        //    try
        //    {
        //        FlightsSearch flightSearch = new FlightsSearch();

        //        flightSearch.SearchId = Guid.NewGuid().ToString();
        //        flightSearch.From = new Airport
        //        {
        //            Code = Request.QueryString["From"]
        //        };

        //        flightSearch.To = new Airport
        //        {
        //            Code = Request.QueryString["To"]
        //        };

        //        flightSearch.DepDate = Convert.ToDateTime(Request.QueryString["DepDate"]);

        //        if (!string.IsNullOrEmpty(Request.QueryString["RetDate"]))
        //            flightSearch.RetDate = Convert.ToDateTime(Request.QueryString["RetDate"]);

        //        flightSearch.Adult = Convert.ToInt32(Request.QueryString["Adult"]);
        //        flightSearch.Child = Convert.ToInt32(Request.QueryString["Child"]);
        //        flightSearch.Infant = Convert.ToInt32(Request.QueryString["Infant"]);

        //        flightSearch.FlightWay = (FlightWay)Convert.ToByte(Request.QueryString["FlightWay"]);

        //        FlightService service = new FlightService();

        //        FlightsResult result = await service.SearchFlight(flightSearch);

        //        return View("result", result);
        //    }
        //    catch
        //    {
        //        return Redirect("/");
        //    }
        //}



        [Route("~/flight/search/")]
        //[Route("~/flight/deepLink/")]
        public async Task<ActionResult> Search()
        {
            if (Request.QueryString["QS"] == null)
            {
                string URLStr = Request.RawUrl;
                string QsStr = URLStr.Split('?')[1];
                string[] QsArr = QsStr.Split('&');
                string SiteCode = QsArr[0].Split('=')[1];
                string SourceMedia = QsArr[1].Split('=')[1];
                string From = QsArr[2].Split('=')[1];
                string To = QsArr[3].Split('=')[1];
                string DepDate = QsArr[4].Split('=')[1];
                string RetDate = QsArr[5].Split('=')[1];
                string Adult = QsArr[6].Split('=')[1];
                string Child = QsArr[7].Split('=')[1];
                string Infant = QsArr[8].Split('=')[1];
                string FlightWays = QsArr[9].Split('=')[1];
                string FlightClass = QsArr[10].Split('=')[1];
                string Airline = QsArr[11].Split('=')[1];
                string IsDirect = QsArr[12].Split('=')[1];
                string IsFlexi = QsArr[13].Split('=')[1];

                string Currency = "GBP";
                string QS = SiteCode + "|" + SourceMedia + "|" + From + "|" + To + "|" + DepDate + "|" + RetDate + "|" + Adult + "|" + Child + "|" + Infant + "|" + FlightWays + "|" + FlightClass + "|" + Airline + "|" + IsDirect + "|" + IsFlexi + "|" + Currency;
                string QSStr = Common.Encrypt(QS);

                string newurl = Common.StaticURL + URLStr.Split('?')[0].Substring(1, URLStr.Split('?')[0].Length - 1) + "?qs=" + QSStr;
                return Redirect(newurl);

            }


            else
            {
                string decrypturl = Common.Decrypt(Request.QueryString["qs"].ToString());
                string[] QSArr = decrypturl.Split('|');


                try
                {


                    FlightsSearch flightSearch = new FlightsSearch();

                    flightSearch.SearchId = Common.GetId();
                    flightSearch.FlightWay = (FlightWay)Convert.ToByte(QSArr[9]);
                    flightSearch.From = Common.GetAirport(QSArr[2]);
                    flightSearch.To = Common.GetAirport(QSArr[3]);
                    flightSearch.DepDate = Convert.ToDateTime(QSArr[4]);
                    if (flightSearch.FlightWay == FlightWay.RoundTrip)
                        flightSearch.RetDate = Convert.ToDateTime(QSArr[5]);
                    flightSearch.Adult = Convert.ToInt32(QSArr[6]);
                    flightSearch.Child = Convert.ToInt32(QSArr[7]);
                    flightSearch.Infant = Convert.ToInt32(QSArr[8]);
                    flightSearch.FlightClass = (FlightClass)Convert.ToByte(QSArr[10]);
                    flightSearch.Airline = Common.GetAirline(QSArr[11]);
                    flightSearch.IsDirect = Convert.ToBoolean(QSArr[12]);
                    flightSearch.IsFlexi = Convert.ToBoolean(QSArr[13]);
                    flightSearch.Currency = QSArr[14];
                    flightSearch.SiteCode = QSArr[0];
                    flightSearch.SourceMedia = QSArr[1];

                    Session["SourceMedia"] = flightSearch.SourceMedia;
                    //flightSearch.IsDeepLink = true;



                    if (flightSearch.SourceMedia == "VIMAN")
                    {
                        flightSearch.IsDeepLink = true;
                    }
                    else
                    {
                        flightSearch.IsDeepLink = false;
                    }



                    // flightSearch.ApiKey = Common.ApiKey;

                    //flightSearch.ApiKey = Common.GetApiKey(flightSearch.SourceMedia);
                    //------------------------------------------
                    try
                    {
                        FlightsResult flightsResult = await Common.SearchFlightAsync(flightSearch);
                        Session["FlightsResult"] = flightsResult;
                        // save cookie
                        Response.Cookies["FlightSearch"]["FlightWay"] = flightsResult.FlightsSearch.FlightWay.ToString();
                        Response.Cookies["FlightSearch"]["FromCode"] = flightsResult.FlightsSearch.From.Code;
                        Response.Cookies["FlightSearch"]["ToCode"] = flightsResult.FlightsSearch.To.Code;
                        Response.Cookies["FlightSearch"]["DepDate"] = flightsResult.FlightsSearch.DepDate.ToString("dd-MM-yyyy");
                        Response.Cookies["FlightSearch"]["RetDate"] = "";
                        if (flightsResult.FlightsSearch.FlightWay == FlightWay.RoundTrip)
                            Response.Cookies["FlightSearch"]["RetDate"] = flightsResult.FlightsSearch.RetDate.ToString("dd-MM-yyyy");
                        Response.Cookies["FlightSearch"].Expires = DateTime.Now.AddDays(1d);

                        List<Airline> airlineList = Common.GetAllAirline();
                        ViewBag.airlineList = airlineList;

                        ViewBag.flightWay = flightsResult.FlightsSearch.FlightWay.ToString();
                        ViewBag.fromName = flightsResult.FlightsSearch.From.Code + "-" + flightsResult.FlightsSearch.From.Name + ", " + flightsResult.FlightsSearch.From.CityName;
                        ViewBag.toName = flightsResult.FlightsSearch.To.Code + "-" + flightsResult.FlightsSearch.To.Name + ", " + flightsResult.FlightsSearch.To.CityName;
                        ViewBag.depDate = flightsResult.FlightsSearch.DepDate.ToString("dd-MM-yyyy");
                        ViewBag.retDate = "";
                        if (flightsResult.FlightsSearch.FlightWay == FlightWay.RoundTrip)
                            ViewBag.retDate = flightsResult.FlightsSearch.RetDate.ToString("dd-MM-yyyy");
                        ViewBag.adult = flightsResult.FlightsSearch.Adult + "";
                        ViewBag.child = flightsResult.FlightsSearch.Child + "";
                        ViewBag.infant = flightsResult.FlightsSearch.Infant + "";
                        ViewBag.flightClass = flightsResult.FlightsSearch.FlightClass.ToString();

                        if (ViewBag.flightClass == "Economy")
                        {
                            ViewBag.fltClass = "4";
                        }
                        if (ViewBag.flightClass == "Premimum Economy")
                        {
                            ViewBag.fltClass = "3";
                        }
                        if (ViewBag.flightClass == "Business")
                        {
                            ViewBag.fltClass = "2";
                        }
                        if (ViewBag.flightClass == "First")
                        {
                            ViewBag.fltClass = "1";
                        }


                        ViewBag.airline = flightsResult.FlightsSearch.Airline.Code;
                        ViewBag.isDirect = (flightsResult.FlightsSearch.IsDirect) ? "1" : "0";
                        ViewBag.isFlexi = (flightsResult.FlightsSearch.IsFlexi) ? "1" : "0";

                        //try
                        //{

                        //    Common.SaveTrackSearch(flightSearch);

                        //}
                        //catch (Exception ex)
                        //{


                        //}
                        // Session["SitePhone"] = Common.GetPhone("META", SiteCode.VIMAN.ToString(), flightsResult.FlightsSearch.SourceMedia);//Common.GetPhone("DEEPLINK", SiteCode.VIMAN.ToString(), flightsResult.FlightsSearch.SourceMedia);
                        if (flightsResult.FlightsList != null && flightsResult.FlightsList.Count > 0)
                        {
                            ViewBag.msg = "success";

                            // Session["SitePhone"] = Common.GetPhone("DEEPLINK", SiteCode.VIMAN.ToString(), flightsResult.FlightsSearch.SourceMedia);

                            FlightsResult newFlightsResult = Common.FillFlightPrice(flightsResult);
                            ViewBag.FilterData = JsonConvert.SerializeObject(Common.FilterFlight(newFlightsResult));
                            //ViewBag.newFlightsResult = JsonConvert.SerializeObject(newFlightsResult);


                            return View("result", newFlightsResult);
                        }
                        else
                        {
                            ViewBag.msg = "noresult";
                            return View("result", flightsResult);
                        }

                    }
                    catch (Exception e)
                    {
                        return Redirect("/");
                    }

                    //------------------------------------
                }
                catch (Exception e)
                {
                    return Redirect("/");
                }
            }



        }


        [HttpGet]
        public ActionResult PreBooking()
        {
            try
            {
                if (Session["FlightsResult"] != null)
                {
                    FlightsResult flightsResult = Session["FlightsResult"] as FlightsResult;

                    if (flightsResult.FlightsList != null && flightsResult.FlightsList.Count > 0)
                    {
                        Flights bookFlight = new Flights();

                        // find flight from session
                        if (Request.QueryString.Get("searchId") != null)
                        {
                            string search_id = Request.QueryString.Get("searchId");
                            string tran_id = Request.QueryString.Get("tranId");

                            bookFlight = flightsResult.FlightsList.Where(f => f.FlightId.Equals(tran_id)).FirstOrDefault();
                        }
                        else if (Request.QueryString.Get("flightId") != null)
                        {
                            string flight_id = Request.QueryString["flightId"].ToString();
                            string out_id = Request.QueryString["outId"].ToString();
                            string in_id = Request.QueryString["inId"].ToString();

                            if (flightsResult.FlightsSearch.FlightWay == FlightWay.OneWay)
                                bookFlight = flightsResult.FlightsList.Where(f => f.Outbound[0].SegmentRef.Equals(out_id) && f.IsDeal == false).FirstOrDefault();
                            else
                                bookFlight = flightsResult.FlightsList.Where(f => f.Outbound[0].SegmentRef.Equals(out_id) && f.Inbound[0].SegmentRef.Equals(in_id) && f.IsDeal == false).FirstOrDefault();
                        }

                        // redirect booking page
                        if (bookFlight != null && bookFlight.Outbound.Count > 0)
                        {
                            FlightsBooking flightsBooking = new FlightsBooking();
                            flightsBooking.SearchId = flightsResult.FlightsSearch.SearchId;
                            flightsBooking.BookingId = Common.GetBookingId("TFG");
                            flightsBooking.Flights = bookFlight;
                            flightsBooking.FlightsSearch = flightsResult.FlightsSearch;
                            flightsBooking.SiteCode = SiteCode.VIMAN.ToString();
                            flightsBooking.SourceMedia = flightsResult.FlightsSearch.SourceMedia;
                            flightsBooking.TranscationStatus = "new";

                            //flightsBooking = Common.CheckFare(flightsBooking);
                            flightsBooking = Common.CheckFareMAN(flightsBooking);



                            if (!flightsBooking.TranscationStatus.Equals("error"))
                            {
                                ViewBag.flightSearch = flightsResult.FlightsSearch;
                                Session["FlightsBook"] = flightsBooking;

                                return Redirect("/flight/booking");
                            }
                            else
                            {
                                return Redirect("/flight/result");
                            }
                        }
                        else
                        {
                            return Redirect("/flight/result");
                        }
                    }
                    else
                    {
                        return Redirect("/");
                    }
                }
                else
                {
                    return Redirect("/");
                }
            }
            catch (Exception)
            {
                return Redirect("/");
            }
        }

        public ActionResult Booking()
        
        {
            try
            {
                if (Session["FlightsBook"] != null)
                {
                    string ass = Session["FlightsBook"].ToString();
                    FlightsBooking bookFlights = Session["FlightsBook"] as FlightsBooking;

                    if (bookFlights != null)
                    {

                        ViewBag.flightWay = bookFlights.FlightsSearch.FlightWay.ToString();
                        ViewBag.fromName = bookFlights.FlightsSearch.From.Code + "-" + bookFlights.FlightsSearch.From.Name + ", " + bookFlights.FlightsSearch.From.CityName;
                        ViewBag.toName = bookFlights.FlightsSearch.To.Code + "-" + bookFlights.FlightsSearch.To.Name + ", " + bookFlights.FlightsSearch.To.CityName;
                        ViewBag.depDate = bookFlights.FlightsSearch.DepDate.ToString("MM-dd-yyyy");
                        ViewBag.retDate = "";
                        if (bookFlights.FlightsSearch.FlightWay == FlightWay.RoundTrip)
                            ViewBag.retDate = bookFlights.FlightsSearch.RetDate.ToString("MM-dd-yyyy");
                        ViewBag.adult = bookFlights.FlightsSearch.Adult + "";
                        ViewBag.child = bookFlights.FlightsSearch.Child + "";
                        ViewBag.infant = bookFlights.FlightsSearch.Infant + "";
                        ViewBag.flightClass = bookFlights.FlightsSearch.FlightClass.ToString();
                        ViewBag.airline = bookFlights.FlightsSearch.Airline.Code;
                        ViewBag.isDirect = (bookFlights.FlightsSearch.IsDirect) ? "1" : "0";
                        ViewBag.isFlexi = (bookFlights.FlightsSearch.IsFlexi) ? "1" : "0";

                        ViewBag.countryList = Common.GetAllCountry();
                        //ViewBag.stateList = Common.GetUSState();

                        ViewBag.flightSearch = bookFlights.FlightsSearch;
                        ViewBag.flights = bookFlights.Flights;

                        ViewBag.Deeplink = bookFlights.Flights.DeepLink;
                        ViewBag.IsDeepLink = bookFlights.FlightsSearch.IsDeepLink;


                        return View(bookFlights);
                    }
                    else
                    {
                        return Redirect("/");
                    }
                }
                else
                {
                    return Redirect("/");
                }
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
        }



        [HttpPost]
        public ActionResult Payment(FormCollection FormColl)
        {
            var hdnFreeChangeAmt = string.IsNullOrEmpty(FormColl["hdnFreeChangeAmt"].ToString()) ? 0 : Convert.ToDecimal(FormColl["hdnFreeChangeAmt"]);
            var hdnFinalfare = string.IsNullOrEmpty(FormColl["hdnFinalfare"].ToString()) ? 0 : Convert.ToDecimal(FormColl["hdnFinalfare"]);
            try
            {
                Session["BookingID"] = null;

                if (Session["FlightsBook"] != null)
                {
                    FlightsBooking bookFlights = Session["FlightsBook"] as FlightsBooking;

                    if (Session["BookingID"] == null)
                    {
                        Session["BookingID"] = Common.GetBookingId("TFG");
                        DataTable dtxc = SqlData.Data.SqlHelperData.ExecuteDataset(SqlData.Data.SqlHelperData.ConnectionStringFlightoffice, CommandType.Text, "SELECT TOP 1 SUBSTRING(BookingId,4,6) BookingId FROM FlightsBooking ORDER BY Id DESC").Tables[0];
                        string max_id = "1";

                        if (dtxc != null && dtxc.Rows.Count > 0)
                        {
                            max_id = (Convert.ToInt32(dtxc.Rows[0]["BookingId"]) + 1).ToString();
                        }

                        //string max_id = (int.Parse(dtxc.Rows[0]["BookingId"].ToString()) + 1) + "";
                        Session["BookingID"] = "TFG" + max_id.PadLeft(6, '0');
                    }
                    bookFlights.BookingId = Session["BookingID"].ToString();
                    if (bookFlights != null)
                    {
                        bookFlights.TranscationId = Common.GetId();
                        string str = FormColl["radio_gender_" + 1];
                        string str2 = FormColl["radio_gender"];
                        bookFlights.FullName = FormColl["title_1"] + " " + FormColl["fname_1"] + " " + FormColl["lname_1"];
                        bookFlights.Email = FormColl["email"];
                        bookFlights.PhoneNo = FormColl["phone"];
                        bookFlights.IsHotel = (string.IsNullOrEmpty(FormColl["hotel_chk"])) ? "0" : FormColl["hotel_chk"];
                        BagsPrice bagPrice = new BagsPrice();

                        bagPrice.BagsPcs = (string.IsNullOrEmpty(FormColl["hdnBaggagePcs"])) ? "0" : FormColl["hdnBaggagePcs"];
                        bagPrice.BagsPcsPrice = float.Parse(FormColl["hdnBaggagePrice"] == null ? "0" : FormColl["hdnBaggagePrice"]);

                        bookFlights.BagPrice = bagPrice;
                        bookFlights.BookerDetail = new BookerDetail()
                        {
                            Gender = FormColl["radio_gender_" + 1],
                            Title = FormColl["title_1"],
                            FirstName = FormColl["fname_1"],
                            LastName = FormColl["lname_1"],
                            Address1 = FormColl["address"],
                            Address2 = "",
                            City = FormColl["city"],
                            Country = FormColl["countryCode"],
                            PostCode = FormColl["postcode"],
                            PhoneNo = FormColl["phone"],
                            Email = FormColl["email"],
                        };
                        int pass_cnt = 0;
                        bookFlights.PassengerDetail = new List<PassengerDetail>();
                        for (int i = 1; i <= bookFlights.FlightsSearch.Adult; i++)
                        {
                            pass_cnt++;
                            string adtP = FormColl["pYear_" + pass_cnt] + "-" + FormColl["pMonth_" + pass_cnt] + "-" + FormColl["pDay_" + pass_cnt];
                            bookFlights.PassengerDetail.Add(new PassengerDetail()
                            {
                                TravelerNo = pass_cnt,
                                PassengerType = PassengerType.Adult,
                                Gender = FormColl["radio_gender_" + pass_cnt],
                                Title = FormColl["title_" + pass_cnt],
                                FirstName = FormColl["fname_" + pass_cnt],
                                LastName = FormColl["lname_" + pass_cnt],
                                //DOB = Convert.ToDateTime(dob_str[2] + "-" + dob_str[1] + "-" + dob_str[0]),
                                DOB = Convert.ToDateTime(FormColl["pYear_" + pass_cnt] + "-" + FormColl["pMonth_" + pass_cnt] + "-" + FormColl["pDay_" + pass_cnt]),
                            });
                        }
                        for (int i = 1; i <= bookFlights.FlightsSearch.Child; i++)
                        {
                            pass_cnt++;
                            //string[] dob_str = FormColl["dob_" + pass_cnt].Split('-');
                            string chdP = FormColl["pYear_" + pass_cnt] + "-" + FormColl["pMonth_" + pass_cnt] + "-" + FormColl["pDay_" + pass_cnt];
                            bookFlights.PassengerDetail.Add(new PassengerDetail()
                            {
                                TravelerNo = pass_cnt,
                                PassengerType = PassengerType.Child,
                                Gender = FormColl["radio_gender_" + pass_cnt],
                                Title = FormColl["title_" + pass_cnt],
                                FirstName = FormColl["fname_" + pass_cnt],
                                LastName = FormColl["lname_" + pass_cnt],
                                //DOB = Convert.ToDateTime(dob_str[2] + "-" + dob_str[1] + "-" + dob_str[0]),
                                DOB = Convert.ToDateTime(FormColl["pYear_" + pass_cnt] + "-" + FormColl["pMonth_" + pass_cnt] + "-" + FormColl["pDay_" + pass_cnt]),
                            });
                        }
                        for (int i = 1; i <= bookFlights.FlightsSearch.Infant; i++)
                        {
                            pass_cnt++;
                            //string[] dob_str = FormColl["dob_" + pass_cnt].Split('-');
                            string infP = FormColl["pYear_" + pass_cnt] + "-" + FormColl["pMonth_" + pass_cnt] + "-" + FormColl["pDay_" + pass_cnt];
                            bookFlights.PassengerDetail.Add(new PassengerDetail()
                            {
                                TravelerNo = pass_cnt,
                                PassengerType = PassengerType.Infant,
                                Gender = FormColl["radio_gender_" + pass_cnt],
                                Title = FormColl["title_" + pass_cnt],
                                FirstName = FormColl["fname_" + pass_cnt],
                                LastName = FormColl["lname_" + pass_cnt],
                                //DOB = Convert.ToDateTime(dob_str[2] + "-" + dob_str[1] + "-" + dob_str[0]),
                                DOB = Convert.ToDateTime(FormColl["pYear_" + pass_cnt] + "-" + FormColl["pMonth_" + pass_cnt] + "-" + FormColl["pDay_" + pass_cnt]),
                            });
                        }



                        ViewBag.FlightId = bookFlights.TranscationId;
                        InsertBookingDetails(bookFlights, hdnFreeChangeAmt.ToString(), hdnFinalfare.ToString());

                        return Redirect("/Flight/Card?bRef=" + bookFlights.TranscationId);

                    }
                    else
                    {
                        ViewBag.PayResponse = "Session Error";
                    }
                }
                else
                {
                    ViewBag.PayResponse = "Session Error";
                }
            }
            catch (Exception e)
            {
                ViewBag.PayResponse = "Server Error";
                ViewBag.PayResponseError = e.Message.ToString();
            }

            return View();
        }

        public int InsertBookingDetails(FlightsBooking bookFlights, string hdnFreeChangeAmt, string hdnFinalfare)
        {
            bookFlights.PNRNo = "";
            bookFlights.WorldResponse = "";

            string SqlQuery = @"INSERT INTO FlightsBooking
    (BookingId, SearchId, TranscationId, PNRNo, FullName, Email, PhoneNo, FlightsSearch, DepDate, RetDate,
     Flights, BookerDetail, PassengerDetail, PaymentDetail, SagepayDetail, SiteCode, SourceMedia,
     FlightsFare, PaymentStatus, WorldResponse, IsHotel, Ip, Status, Timestamp, DeepLink,
     BagsPrice, FreeChangeAmt, FinalTotalAmt, FromCity, ToCity)
     VALUES
    (@bookingid, @searchid, @transcationid, NULL, @fullname, @email, @phoneno, @flightssearch, @depdate, @retdate,
     @flights, @bookerdetail, @passengerdetail, @paymentdetail, @sagepaydetail, @sitecode, @sourcemedia,
     @flightsfare, @paymentstatus, @worldresponse, @ishotel, @ip, @status, @timestamp, @deeplink,
     @BagsPrice, @FreeChangeAmt, @FinalTotalAmt, @FromCity, @ToCity)";

            SqlParameter[] commandParameters = new SqlParameter[]
            {
        new SqlParameter("@bookingid", bookFlights.BookingId),
        new SqlParameter("@searchid", bookFlights.SearchId),
        new SqlParameter("@transcationid", bookFlights.TranscationId),
        new SqlParameter("@fullname", bookFlights.FullName),
        new SqlParameter("@email", bookFlights.Email),
        new SqlParameter("@phoneno", bookFlights.PhoneNo),

        new SqlParameter("@flightssearch", JsonConvert.SerializeObject(bookFlights.FlightsSearch)),
        new SqlParameter("@flights", JsonConvert.SerializeObject(bookFlights.Flights)),
        new SqlParameter("@bookerdetail", JsonConvert.SerializeObject(bookFlights.BookerDetail)),
        new SqlParameter("@passengerdetail", JsonConvert.SerializeObject(bookFlights.PassengerDetail)),
        new SqlParameter("@paymentdetail", JsonConvert.SerializeObject(bookFlights.PaymentDetail)),
        new SqlParameter("@sagepaydetail", JsonConvert.SerializeObject(bookFlights.SagepayDetail)),

        new SqlParameter("@sitecode",  "Parallelfares"),
        new SqlParameter("@sourcemedia", bookFlights.SourceMedia ?? ""),

        new SqlParameter("@flightsfare", JsonConvert.SerializeObject(bookFlights.FlightsFare)),
        new SqlParameter("@paymentstatus", "unpaid"),
        new SqlParameter("@worldresponse", bookFlights.WorldResponse ?? ""),

        new SqlParameter("@ishotel", Convert.ToInt32(bookFlights.IsHotel)),
        new SqlParameter("@ip", Common.GetLocalIP()),
        new SqlParameter("@status", "new"),
        new SqlParameter("@timestamp", DateTime.Now),

        new SqlParameter("@depdate", bookFlights.FlightsSearch.DepDate),
        new SqlParameter("@retdate", bookFlights.FlightsSearch.RetDate),

        new SqlParameter("@deeplink", bookFlights.Flights?.DeepLink ?? (object)DBNull.Value),

        new SqlParameter("@BagsPrice", JsonConvert.SerializeObject(bookFlights.BagPrice)),

        new SqlParameter("@FreeChangeAmt", Convert.ToDecimal(hdnFreeChangeAmt)),
        new SqlParameter("@FinalTotalAmt", Convert.ToDecimal(hdnFinalfare)),

        new SqlParameter("@FromCity", bookFlights.FlightsSearch.From.CityCode),
        new SqlParameter("@ToCity", bookFlights.FlightsSearch.To.CityCode)
            };

            int chk = 0;

            try
            {
                chk = SqlHelperData.ExecuteNonQuery(
                    SqlHelperData.ConnectionStringFlightoffice,
                    CommandType.Text,
                    SqlQuery,
                    commandParameters);
            }
            catch (Exception ex)
            {
                // log error if needed
            }

            return chk;
        }

        //public int InsertBookingDetails(FlightsBooking bookFlights, string hdnFreeChangeAmt, string hdnFinalfare)
        //{

        //    bookFlights.PNRNo = "";
        //    bookFlights.WorldResponse = "";
        //    SqlParameter[] commandParameters = new SqlParameter[30];
        //    commandParameters[0] = new SqlParameter("@bookingid", SqlDbType.VarChar, 50) { Value = bookFlights.BookingId };
        //    commandParameters[1] = new SqlParameter("@searchid", SqlDbType.VarChar, 50) { Value = bookFlights.SearchId };
        //    commandParameters[2] = new SqlParameter("@transcationid", SqlDbType.VarChar, 50) { Value = bookFlights.TranscationId };
        //    commandParameters[3] = new SqlParameter("@pnrno", SqlDbType.VarChar, 50) { Value = bookFlights.PNRNo };
        //    commandParameters[4] = new SqlParameter("@fullname", SqlDbType.VarChar, 50) { Value = bookFlights.FullName };
        //    commandParameters[5] = new SqlParameter("@email", SqlDbType.VarChar, 50) { Value = bookFlights.Email };
        //    commandParameters[6] = new SqlParameter("@phoneno", SqlDbType.VarChar, 50) { Value = bookFlights.PhoneNo };
        //    commandParameters[7] = new SqlParameter("@flightssearch", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.FlightsSearch) };
        //    commandParameters[8] = new SqlParameter("@flights", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.Flights) };
        //    commandParameters[9] = new SqlParameter("@bookerdetail", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.BookerDetail) };
        //    commandParameters[10] = new SqlParameter("@passengerdetail", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.PassengerDetail) };
        //    commandParameters[11] = new SqlParameter("@paymentdetail", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.PaymentDetail) };
        //    commandParameters[12] = new SqlParameter("@sagepaydetail", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.SagepayDetail) };
        //    commandParameters[13] = new SqlParameter("@sitecode", SqlDbType.VarChar, 50) { Value = bookFlights.SiteCode == "VIMAN" ? "" : "" };
        //    commandParameters[14] = new SqlParameter("@sourcemedia", SqlDbType.VarChar, 50) { Value = bookFlights.SourceMedia == "VIMAN" ? "" : bookFlights.SourceMedia };
        //    commandParameters[15] = new SqlParameter("@flightsfare", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.FlightsFare) };
        //    commandParameters[16] = new SqlParameter("@paymentstatus", SqlDbType.VarChar, 50) { Value = "unpaid" };
        //    commandParameters[17] = new SqlParameter("@worldresponse", SqlDbType.VarChar) { Value = bookFlights.WorldResponse };
        //    commandParameters[18] = new SqlParameter("@ishotel", SqlDbType.Int) { Value = int.Parse(bookFlights.IsHotel) };
        //    commandParameters[19] = new SqlParameter("@ip", SqlDbType.VarChar, 50) { Value = Common.GetLocalIP() };
        //    commandParameters[20] = new SqlParameter("@status", SqlDbType.VarChar, 50) { Value = "new" };
        //    commandParameters[21] = new SqlParameter("@timestamp", SqlDbType.DateTime) { Value = DateTime.Now };
        //    commandParameters[22] = new SqlParameter("@depdate", SqlDbType.Date) { Value = bookFlights.FlightsSearch.DepDate };
        //    commandParameters[23] = new SqlParameter("@retdate", SqlDbType.Date) { Value = bookFlights.FlightsSearch.RetDate };
        //    commandParameters[24] = new SqlParameter("@deeplink", SqlDbType.VarChar) { Value = bookFlights.Flights.DeepLink };
        //    commandParameters[25] = new SqlParameter("@BagsPrice", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.BagPrice) };
        //    commandParameters[26] = new SqlParameter("@FreeChangeAmt", SqlDbType.Decimal) { Value = JsonConvert.SerializeObject(Convert.ToDecimal(hdnFreeChangeAmt)) };
        //    commandParameters[27] = new SqlParameter("@FinalTotalAmt", SqlDbType.Decimal) { Value = JsonConvert.SerializeObject(Convert.ToDecimal(hdnFinalfare)) };

        //    commandParameters[28] = new SqlParameter("@FromCity", SqlDbType.VarChar, 50) { Value = bookFlights.FlightsSearch.From.CityCode };
        //    commandParameters[29] = new SqlParameter("@ToCity", SqlDbType.VarChar, 50) { Value = bookFlights.FlightsSearch.To.CityCode };


        //    string SqlQuery = "INSERT INTO FlightsBooking(BookingId, SearchId, TranscationId, PNRNo, FullName, Email, PhoneNo, FlightsSearch, DepDate, RetDate, Flights, BookerDetail, PassengerDetail, PaymentDetail, SagepayDetail, SiteCode, SourceMedia, FlightsFare, PaymentStatus, WorldResponse, IsHotel, Ip, Status, Timestamp, DeepLink, BagsPrice, FreeChangeAmt, FinalTotalAmt,FromCity,ToCity) VALUES(@bookingid, @searchid, @transcationid, null, @fullname, @email, @phoneno, @flightssearch, @depdate, @retdate, @flights, @bookerdetail, @passengerdetail, @paymentdetail, @sagepaydetail, @sitecode, @sourcemedia, @flightsfare, @paymentstatus, @worldresponse, @ishotel, @ip, @status, @timestamp, @deeplink, @BagsPrice, @FreeChangeAmt, @FinalTotalAmt,@FromCity,@ToCity)";
        //    DataTable dt = SqlHelperData.ExecuteDataset(SqlHelperData.ConnectionString, CommandType.Text, "SELECT BookingId From FlightsBooking where BookingId='" + bookFlights.BookingId + "' ").Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {//update entry
        //        SqlQuery = "Update FlightsBooking SET BookingId=@bookingid, SearchId=@searchid, TranscationId=@transcationid, PNRNo=null, FullName=@fullname, Email=@email, PhoneNo=@phoneno, FlightsSearch=@flightssearch, DepDate=@depdate, RetDate=@retdate, Flights=@flights, BookerDetail=@bookerdetail, PassengerDetail=@passengerdetail, PaymentDetail=@paymentdetail, SagepayDetail=@sagepaydetail, SiteCode=@sitecode, SourceMedia=@sourcemedia, FlightsFare=@flightsfare, PaymentStatus=@paymentstatus, WorldResponse=@worldresponse, IsHotel=@ishotel, Ip=@ip, Status=@status, Timestamp=@timestamp, DeepLink=@deeplink, BagsPrice=@BagsPrice, FreeChangeAmt=@FreeChangeAmt, FinalTotalAmt=@FinalTotalAmt,FromCity=@FromCity,ToCity=@ToCity WHERE BookingID=@bookingid";

        //    }

        //    int chk = 0;
        //    //Common.WriteLog("Insertbooking", "BOOKONG");
        //    try
        //    {
        //        chk = SqlHelperData.ExecuteNonQuery(SqlHelperData.ConnectionStringFlightoffice, CommandType.Text, SqlQuery, commandParameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Common.WriteLog("Insertbooking", ex.ToString());
        //    }
        //    //Notification(bookFlights.TranscationId);


        //    return chk;

        //}




        [HttpPost]
        public ActionResult PaymentUpd(FormCollection FormColl)
        {
            var hdnFreeChangeAmt = string.IsNullOrEmpty(FormColl["hdnFreeChangeAmt"].ToString()) ? 0 : Convert.ToDecimal(FormColl["hdnFreeChangeAmt"]);
            var hdnFinalfare = string.IsNullOrEmpty(FormColl["hdnFinalfare"].ToString()) ? 0 : Convert.ToDecimal(FormColl["hdnFinalfare"]);
            try
            {
                Session["BookingID"] = null;

                if (Session["FlightsBook"] != null)
                {
                    FlightsBooking bookFlights = Session["FlightsBook"] as FlightsBooking;

                    //bookFlights.BookingId = Common.GetBookingId("VMB");

                    if (bookFlights != null)
                    {
                        //bookFlights.TranscationId = Common.GetId();
                        //Booking table Insertion


                        if (FormColl["cc_number"] != null)
                        {
                            bookFlights.SagepayDetail = new SagepayDetail()
                            {
                                BookingId = bookFlights.BookingId,
                                TranscationId = bookFlights.TranscationId,
                                CardType = FormColl["cardType"],
                                CardHolderName = FormColl["cc_name"],
                                CardNumber = FormColl["cc_number"],
                                CVV = FormColl["securityCode"],
                                ExpiryDate = FormColl["cc_expMM"] + "/" + FormColl["cc_expMM"],
                                CryptVal = "",
                                Last4Digits = FormColl["cc_number"].Length == 16 ? FormColl["cc_number"].Substring(12, 4) : string.Empty,
                            };

                            ViewBag.PayResponse = "Inhouse Redirect";

                        }
                        else
                        {
                            // generate sagepayment variable
                            var cryptValue = "Card not Enter";

                            if (cryptValue.Equals(""))
                            {
                                ViewBag.PayResponse = "Payment Error";
                            }
                            else
                            {
                                bookFlights.SagepayDetail = new SagepayDetail()
                                {
                                    BookingId = bookFlights.BookingId,
                                    TranscationId = bookFlights.TranscationId,
                                    CryptVal = cryptValue
                                };

                                ViewBag.PayResponse = cryptValue;
                            }
                        }

                        ViewBag.FlightId = bookFlights.TranscationId;

                        // Common.WriteLog("TranscationId :" + ViewBag.FlightId.ToString());
                        UpdateBookingDetails(bookFlights, hdnFreeChangeAmt.ToString(), hdnFinalfare.ToString());




                        return Redirect("/flight/Confirm?ref=" + bookFlights.TranscationId);
                    }
                    else
                    {
                        ViewBag.PayResponse = "Session Error";
                    }
                }
                else
                {
                    ViewBag.PayResponse = "Session Error";
                }
            }
            catch (Exception e)
            {
                ViewBag.PayResponse = "Server Error";
                ViewBag.PayResponseError = e.Message.ToString();
            }

            return View();
        }


        public int UpdateBookingDetails(FlightsBooking bookFlights, string hdnFreeChangeAmt, string hdnFinalfare)
        {

            bookFlights.PNRNo = "";
            bookFlights.WorldResponse = "";
            SqlParameter[] commandParameters = new SqlParameter[4];
            commandParameters[0] = new SqlParameter("@bookingid", SqlDbType.VarChar, 50) { Value = bookFlights.BookingId };
            commandParameters[1] = new SqlParameter("@searchid", SqlDbType.VarChar, 50) { Value = bookFlights.SearchId };
            commandParameters[2] = new SqlParameter("@transcationid", SqlDbType.VarChar, 50) { Value = bookFlights.TranscationId };
            commandParameters[3] = new SqlParameter("@sagepaydetail", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(bookFlights.SagepayDetail) };

            DataTable dt = SqlHelperData.ExecuteDataset(SqlHelperData.ConnectionString, CommandType.Text, "SELECT BookingId From FlightsBooking where BookingId='" + bookFlights.BookingId + "' ").Tables[0];

            string SqlQuery = "";
            if (dt.Rows.Count > 0)
            {//update entry
                SqlQuery = "Update FlightsBooking SET SagepayDetail='" + JsonConvert.SerializeObject(bookFlights.SagepayDetail) + "' WHERE BookingID='" + bookFlights.BookingId + "' and SearchId='" + bookFlights.SearchId + "'";

            }
            int chk = 0;//SqlHelperData.ExecuteNonQuery(SqlHelperData.ConnectionStringFlightoffice, CommandType.Text, SqlQuery, commandParameters);            
            try
            {
                chk = SqlHelperData.ExecuteNonQuery(SqlHelperData.ConnectionStringFlightoffice, CommandType.Text, SqlQuery, commandParameters);
            }
            catch (Exception ex)
            {
                //Common.WriteLog("Insertbooking", ex.ToString());
            }
            return chk;

        }

        public ActionResult Card(string bRef)
        {
            string bookingRef = bRef;//Request.QueryString["ref"];
            if (!string.IsNullOrEmpty(bookingRef))
            {
                FlightsBooking flightsBooking = Common.GetFlightByIdFlightoffice(bookingRef); //Common.GetFlightById(bookingRef);
               

                ViewBag.flights = flightsBooking.Flights;
                ViewBag.flightSearch = flightsBooking.FlightsSearch;

                return View(flightsBooking);
            }
            else
            {
                return Redirect("/");
            }
        }

        public ActionResult Confirm()
        {
            string bookingRef = Request.QueryString["ref"];
            if (!string.IsNullOrEmpty(bookingRef))
            {
                FlightsBooking flightsBooking = Common.GetFlightByIdFlightoffice(bookingRef); //Common.GetFlightById(bookingRef);
               

                ViewBag.flights = flightsBooking.Flights;
                ViewBag.flightSearch = flightsBooking.FlightsSearch;

                return View(flightsBooking);
            }
            else
            {
                return Redirect("/");
            }
        }



       
        public ActionResult RedirectToBooking(string flightId, string site)
        {
           
            FlightsBooking bookingObject = GetFlightById(flightId); // your logic


            string token = Guid.NewGuid().ToString();

           
            SaveBookingData(token, bookingObject);

            string redirectUrl = "";

            switch (site)
            {
                case "A":
                    redirectUrl = "https://www.parallelfares.com/flight/Booking?token=" + token;
                    //redirectUrl = "http://localhost:14882/flight/Booking?token=" + token;
                    break;

                case "B":
                    redirectUrl = "https://www.fareschoices.com/flight/Booking?token=" + token;
                    //redirectUrl = "http://localhost:14881/flight/Booking?token=" + token;
                    break;

                case "C":
                    redirectUrl = "https://faresbridge.com/flight/Booking?token=" + token;
                    break;

                case "D":
                    redirectUrl = "https://www.faresbundle.com/flight/Booking?token=" + token;
                    break;

                case "E":
                    redirectUrl = "https://www.faresgator.com/flight/Booking?token=" + token;
                    break;

                case "F":
                    redirectUrl = "https://www.airfaretool.com/flight/Booking?token=" + token;
                    break;

                case "G":
                    redirectUrl = "https://www.flightsearchradar.com/flight/Booking?token=" + token;
                    break;


                case "H":
                    redirectUrl = "https://www.faresbracket.com/flight/Booking?token=" + token;
                    break;



                default:
                    return Redirect("/");
            }

            return Redirect(redirectUrl);

            // return Redirect("https://www.fareschoices.com/flight/Booking?token=" + token);
        }

        public static Dictionary<string, FlightsBooking> tempStore = new Dictionary<string, FlightsBooking>();

        public void SaveBookingData(string token, FlightsBooking data)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            string query = @"INSERT INTO BookingTokens 
                    (Token, Data, CreatedAt, ExpiryAt, IsUsed)
                    VALUES (@Token, @Data, GETDATE(), DATEADD(MINUTE, 20, GETDATE()), 0)";

            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@Token", token),
        new SqlParameter("@Data", json)
            };

            SqlHelperData.ExecuteNonQuery(
                SqlHelperData.ConnectionString,
                CommandType.Text,
                query,
                param
            );
        }

        public FlightsBooking GetBookingData(string token)
        {
            return tempStore.ContainsKey(token) ? tempStore[token] : null;
        }

        private FlightsBooking GetFlightById(string flightId)
        {
            var result = Session["FlightsResult"] as FlightsResult;

            if (result == null)
                return null;

            var selectedFlight = result.FlightsList
                .FirstOrDefault(f => f.FlightId == flightId);

            if (selectedFlight == null)
                return null;

            return new FlightsBooking
            {
                FlightsSearch = result.FlightsSearch,
                Flights = selectedFlight
            };
        }

    }

}