using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Viman
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Net.ServicePointManager.SecurityProtocol =  System.Net.SecurityProtocolType.Tls12;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    if (isLocalhost() && Request.Url.ToString().IndexOf("portal.") == -1)
        //    {
        //        if (Request.Url.Authority.StartsWith("www") == false)
        //        {
        //            var url = string.Format("{0}://www.{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.Url.PathAndQuery);
        //            Response.RedirectPermanent(url, true);
        //        }
        //        if (HttpContext.Current.Request.IsSecureConnection == false)
        //        {
        //            Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
        //        }
        //    }
        //}

        //protected bool isLocalhost()
        //{
        //    string VisitorsIPAddr = string.Empty;
        //    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        //    {
        //        VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        //    }
        //    else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        //    {
        //        VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        //    }
        //    if (VisitorsIPAddr == "" || VisitorsIPAddr == "127.0.0.1" || VisitorsIPAddr == "::1")
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }

    public class SecurePath
    {
        public static bool IsSecure(string path)
        {
            System.Collections.Generic.List<SecurePage> lstPages = new System.Collections.Generic.List<SecurePage>();

            bool mIsSecure = false;

            try
            {
                //Fill the list of pages defined in web.config
                System.Collections.Specialized.NameValueCollection sectionPages = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("SecurePages");

                foreach (string key in sectionPages)
                {
                    if ((!string.IsNullOrEmpty(key)) && (!string.IsNullOrEmpty(sectionPages.Get(key))))
                    {
                        SecurePage secPage = new SecurePage();
                        secPage.PathType = sectionPages.Get(key);
                        secPage.Path = key;
                        lstPages.Add(secPage);
                    }
                }

                //loop through the list to match the path with the value in the list item
                foreach (SecurePage page in lstPages)
                {
                    switch (page.PathType.ToLower().Trim())
                    {
                        case "directory":
                            if (path.Contains(page.Path))
                            {
                                mIsSecure = true;
                            }
                            break;
                        case "page":
                            if (path.ToLower().Trim() == page.Path.ToLower().Trim())
                            {
                                mIsSecure = true;
                            }
                            break;
                        default:
                            mIsSecure = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return mIsSecure;
        }
        private class SecurePage
        {
            public SecurePage()
            {

            }
            public string PathType { set; get; }
            public string Path { set; get; }
        }
    }
}
