using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viman.Models;
using static Viman.Models.Common;


namespace Viman.Controllers
{
    public class HomeController : Controller
    {

       
    


        [Route("")]
        public ActionResult Index()
        {         

            return View();
        }

        [Route("about")]
        public ActionResult about()
        {

            return View();
        }

        [Route("contact")]
        public ActionResult contact()
        {

            return View();
        }

        [Route("privacy-policy")]
        public ActionResult privacypolicy()
        {

            return View();
        }

        [Route("terms-conditions")]
        public ActionResult termscondition()
        {

            return View();
        }


        [Route("flights-from-atlanta-to-hawaii")]
        public ActionResult flightsfromatlantatohawaii()
        {

            return View();
        }


        [Route("flights-from-atlanta-to-rome")]
        public ActionResult flightsfromatlantatorome()
        {

            return View();
        }


        [Route("flights-from-austin-to-london")]
        public ActionResult flightsfromaustintolondon()
        {

            return View();
        }


        [Route("flights-from-chicago-to-greece")]
        public ActionResult flightsfromchicagotogreece()
        {

            return View();
        }


        [Route("flights-from-chicago-to-madrid")]
        public ActionResult flightsfromchicagotomadrid()
        {

            return View();
        }


        [Route("flights-from-philadelphia-to-hawaii")]
        public ActionResult flightsfromphiladelphiatohawaii()
        {

            return View();
        }


        [Route("flights-from-pittsburgh-to-london")]
        public ActionResult flightsfrompittsburghtolondon()
        {

            return View();
        }


        [Route("flights-from-cincinnati-to-bonaire")]
        public ActionResult flightsfromcincinnatitobonaire()
        {

            return View();
        }

        [Route("flights-from-dallas-to-london")]
        public ActionResult flightsfromdallastolondon()
        {

            return View();
        }

        [Route("flights-from-denver-to-paris")]
        public ActionResult flightsfromdenvertoparis
()
        {

            return View();
        }

        [Route("flights-from-miami-to-madrid")]
        public ActionResult flightsfrommiamitomadrid()
        {

            return View();
        }

        [Route("flights-from-miami-to-paris")]
        public ActionResult flightsfrommiamitoparis()
        {
            return View();
        }

        [Route("flights-from-newark-to-hawaii")]
        public ActionResult flightsfromnewarktohawaii()
        {
            return View();
        }

        [Route("flights-from-new-york-to-greece")]
        public ActionResult flightsfromnewyorktogreece()
        {
            return View();
        }
        [Route("flights-from-new-york-to-hawaii")]
        public ActionResult flightsfromnewyorktohawaii()
        {
            return View();
        }


        [Route("flights-from-san-diego-to-london")]
        public ActionResult flightsfromsandiegotolondon()
        {

            return View();
        }

        [Route("flights-from-seattle-to-paris")]
        public ActionResult flightsfromseattletoparis()
        {

            return View();
        }


        [Route("flights-from-sacramento-to-maui")]
        public ActionResult flightsfromsacramentotomaui()
        {

            return View();
        }


        [Route("flights-from-salt-lake-city-to-hawaii")]
        public ActionResult flightsfromsaltlakecitytohawaii()
        {

            return View();
        }


        [Route("flights-from-boston-to-dublin")]
        public ActionResult flightsfrombostontodublin()
        {

            return View();
        }


        [Route("flights-from-seattle-to-maui")]
        public ActionResult flightsfromseattletomaui()
        {

            return View();
        }


        [Route("flights-from-boston-to-ireland")]
        public ActionResult flightsfrombostontoireland()
        {

            return View();
        }


        [Route("flights-from-boston-to-iceland")]
        public ActionResult flightsfrombostontoiceland()
        {

            return View();
        }


        [Route("flights-from-boston-to-london")]
        public ActionResult flightsfrombostontolondon()
        {

            return View();
        }


        [Route("flights-from-chicago-to-iceland")]
        public ActionResult flightsfromchicagotoiceland()
        {

            return View();
        }


        [Route("flights-from-las-vegas-to-hawaii")]
        public ActionResult flightsfromlasvegastohawaii()
        {

            return View();
        }


        [Route("flights-from-las-vegas-to-honolulu")]
        public ActionResult flightsfromlasvegastohonolulu()
        {

            return View();
        }



        [Route("flights-from-miami-to-lima")]
        public ActionResult flightsfrommiamitolima()
        {

            return View();
        }



        [Route("flights-from-sacramento-to-hawaii")]
        public ActionResult flightsfromsacramentotohawaii()
        {

            return View();
        }



        [Route("flights-from-salt-lake-city-to-cancun")]
        public ActionResult flightsfromsaltlakecitytocancun()
        {

            return View();
        }



        [Route("flights-from-salt-lake-city-to-new-york")]
        public ActionResult flightsfromsaltlakecitytonewyork()
        {

            return View();
        }



        [Route("flights-from-san-diego-to-boston")]
        public ActionResult flightsfromsandiegotoboston()
        {

            return View();
        }



        [Route("flights-from-new-york-to-iceland")]
        public ActionResult flightsfromnewyorktoiceland()
        {

            return View();
        }



        [Route("flights-from-phoenix-to-hawaii")]
        public ActionResult flightsfromphoenixtohawaii()
        {

            return View();
        }



        [Route("flights-from-los-angeles-to-hawaii")]
        public ActionResult flightsfromlosangelestohawaii()
        {

            return View();
        }





    }

}