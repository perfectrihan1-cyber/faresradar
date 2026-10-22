using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Viman.Models
{
    public class ErrorHandler
    {
        public static string UserName = string.Empty;

        public static void WriteError(Exception _Exception)
        {
            string _Error = "";
            string path = "~/ErrorLog/" + DateTime.UtcNow.ToString("dd-MM-yy") + ".txt";
            try
            {
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    if (_Exception != null)
                    {
                        _Error = "New Log Entry" + Environment.NewLine;
                        _Error = "===========================" + Environment.NewLine;
                        _Error = _Error + "User Id          :" + "Guest" + Environment.NewLine;
                        _Error = _Error + "User Name        :" + Environment.NewLine;
                        _Error = _Error + "Date             :" + DateTime.UtcNow + Environment.NewLine;
                        _Error = _Error + "URL              :" + HttpContext.Current.Request.ServerVariables["URL"] + Environment.NewLine;
                        _Error = _Error + "Query String     :" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"] + Environment.NewLine;
                        _Error = _Error + "Req From         :" + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + Environment.NewLine;
                        _Error = _Error + "HostAddress      :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine;
                        _Error = _Error + "Error Desc       :" + _Exception.Message + Environment.NewLine;
                        _Error = _Error + "Source           :" + _Exception.Source + Environment.NewLine;
                        _Error = _Error + "Line No          :" + _Exception.StackTrace + Environment.NewLine;
                        _Error = _Error + "=============================";
                    }
                    w.WriteLine(_Error);
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception)
            {
            }
            finally
            {

            }
        }

        public static void WriteError(string Path, Exception _Exception)
        {
            try
            {
                if (Path == null || Path == "")
                {
                    Path = System.Web.HttpContext.Current.Server.MapPath("~/ErrorLog/" + DateTime.UtcNow.ToString("dd-MM-yy") + ".txt");
                }
                if (!File.Exists(Path))
                {
                    File.Create(Path).Close();
                }
                using (StreamWriter w = File.AppendText(Path))
                {
                    w.WriteLine("New Log Entry : ");
                    w.WriteLine("{0}", DateTime.UtcNow);

                    string _Error = "";
                    if (_Exception != null)
                    {
                        _Error = "New Log Entry" + Environment.NewLine;
                        _Error = "===========================" + Environment.NewLine;
                        _Error = _Error + "User Id          :" + "Guest <br/>";
                        _Error = _Error + "User Name        :" + "<br/>";
                        _Error = _Error + "Date             :" + DateTime.UtcNow + Environment.NewLine;
                        _Error = _Error + "URL              :" + HttpContext.Current.Request.ServerVariables["URL"] + Environment.NewLine;
                        _Error = _Error + "Query String     :" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"] + Environment.NewLine;
                        _Error = _Error + "Req From         :" + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + Environment.NewLine;
                        _Error = _Error + "HostAddress      :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine;
                        _Error = _Error + "Error Desc       :" + _Exception.Message + Environment.NewLine;
                        _Error = _Error + "Source           :" + _Exception.Source + Environment.NewLine;
                        _Error = _Error + "Line No          :" + _Exception.StackTrace + Environment.NewLine;
                        _Error = _Error + "=============================";
                    }
                    w.WriteLine(_Error);
                    w.Flush();
                    w.Close();
                    UserName = string.Empty;
                }
            }
            catch (Exception)
            {

            }
        }

        public static void WriteError(System.Net.Mail.MailMessage _MailMessage)
        {
            string path = "~/ErrorLog/" + DateTime.UtcNow.ToString("dd-MM-yy") + ".txt";
            try
            {
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    string _Error = "Error while sending email : ";
                    _Error = "New Log Entry" + Environment.NewLine;
                    _Error = "===========================" + Environment.NewLine;
                    _Error = _Error + "User Id          :" + "Guest <br/>";
                    _Error = _Error + "User Name        :" + "<br/>";
                    _Error = _Error + "Date             :" + DateTime.UtcNow + Environment.NewLine;
                    _Error = _Error + "URL              :" + HttpContext.Current.Request.ServerVariables["URL"] + Environment.NewLine;
                    _Error = _Error + "Query String     :" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"] + Environment.NewLine;
                    _Error = _Error + "Req From         :" + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + Environment.NewLine;
                    _Error = _Error + "HostAddress      :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine;
                    if (_MailMessage != null)
                    {
                        for (int i = 0; i < _MailMessage.To.Count; i++)
                            _Error = _Error + "Mail To          :" + _MailMessage.To[i] + Environment.NewLine;
                        for (int i = 0; i < _MailMessage.CC.Count; i++)
                            _Error = _Error + "Mail CC          :" + _MailMessage.CC[i] + Environment.NewLine;
                        for (int i = 0; i < _MailMessage.Bcc.Count; i++)
                            _Error = _Error + "Mail BCC         :" + _MailMessage.Bcc[i] + Environment.NewLine;
                        _Error = _Error + "Mail Body        :" + _MailMessage.Body + Environment.NewLine;
                    }
                    _Error = _Error + "=============================";

                    w.WriteLine(_Error);
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void WriteLog(Hashtable hashtable)
        {
            string path = "~/ErrorLog/" + DateTime.UtcNow.ToString("dd-MM-yy") + "_Details.txt";
            string _Error = "";
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                    {
                        File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                    }
                    using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                    {
                        _Error = "New Log Entry" + Environment.NewLine;
                        _Error = "===========================" + Environment.NewLine;
                        _Error = _Error + "User Id          :" + "Guest <br/>";
                        _Error = _Error + "User Name        :" + "<br/>";
                        _Error = _Error + "Date             :" + DateTime.UtcNow + Environment.NewLine;
                        _Error = _Error + "URL              :" + HttpContext.Current.Request.ServerVariables["URL"] + Environment.NewLine;
                        _Error = _Error + "Query String     :" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"] + Environment.NewLine;
                        _Error = _Error + "Req From         :" + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + Environment.NewLine;
                        _Error = _Error + "HostAddress      :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine;
                        foreach (DictionaryEntry entry in hashtable)
                        {
                            if (entry.Value != null)
                            {
                                _Error = _Error + "Key               :" + entry.Key.ToString() + " \t AND \t Value: " + entry.Value.ToString().Trim() + Environment.NewLine;
                            }
                            else
                            {
                                _Error = _Error + "Key               :" + entry.Key.ToString() + " \t AND \t Value: Null" + Environment.NewLine;
                            }
                        }
                        _Error = _Error + "==========================";
                        w.WriteLine(_Error);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {

            }
        }

        public static void WriteLog(string Path, string Log)
        {
            try
            {
                if (System.Web.HttpContext.Current != null)//This check is added by Phil On 20/11/2014 just for if thread start then it is null.
                {
                    if (Path == null || Path == "")
                    {
                        Path = System.Web.HttpContext.Current.Server.MapPath("~/ErrorLog/" + DateTime.UtcNow.ToString("dd-MM-yy") + ".txt");
                    }
                    if (!File.Exists(Path))
                    {
                        File.Create(Path).Close();
                    }
                    using (StreamWriter w = File.AppendText(Path))
                    {
                        string _Error = "";
                        _Error = "New Log Entry<br/>";
                        _Error = "===========================<br/>";
                        _Error = _Error + "User Id          :" + "Guest <br/>";
                        _Error = _Error + "User Name        :" + "<br/>";
                        _Error = _Error + "Date             :" + DateTime.UtcNow + "<br/>";
                        _Error = _Error + "URL              :" + HttpContext.Current.Request.ServerVariables["URL"] + "<br/>";
                        _Error = _Error + "Query String     :" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"] + "<br/>";
                        _Error = _Error + "Req From         :" + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + "<br/>";
                        _Error = _Error + "HostAddress      :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "<br/>";
                        _Error = _Error + "Log              :" + Log + "<br/>";
                        _Error = _Error + "==========================";
                        w.WriteLine(_Error);
                        w.Flush();
                        w.Close();
                    }
                }
                else if (!string.IsNullOrEmpty(Path) && !string.IsNullOrEmpty(Log))// This section added by phil on 20/11/2014 for the time of thread
                {
                    if (!File.Exists(Path))
                    {
                        File.Create(Path).Close();
                    }
                    using (StreamWriter w = File.AppendText(Path))
                    {
                        w.WriteLine(Log);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public static void WriteBookingCreationError(string subject, Hashtable hashtable, string log)
        {
            string path = "~/ErrorLog/" + DateTime.UtcNow.ToString("dd-MM-yy") + "_BookingCreationError.txt";
            string _Error = "";
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                    {
                        File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                    }
                    using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                    {
                        _Error = "New Log Entry" + Environment.NewLine;
                        _Error = "===========================" + Environment.NewLine;
                        _Error = _Error + "User Id          :" + "Guest <br/>";
                        _Error = _Error + "User Name        :" + "<br/>";
                        _Error = _Error + "Date             :" + DateTime.UtcNow + Environment.NewLine;
                        _Error = _Error + "URL              :" + HttpContext.Current.Request.ServerVariables["URL"] + Environment.NewLine;
                        _Error = _Error + "Query String     :" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"] + Environment.NewLine;
                        _Error = _Error + "Req From         :" + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + Environment.NewLine;
                        _Error = _Error + "HostAddress      :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine;
                        foreach (DictionaryEntry entry in hashtable)
                        {
                            if (entry.Value != null)
                            {
                                _Error = _Error + "Key               :" + entry.Key.ToString() + " \t AND \t Value: " + entry.Value.ToString().Trim() + Environment.NewLine;
                            }
                            else
                            {
                                _Error = _Error + "Key               :" + entry.Key.ToString() + " \t AND \t Value: Null" + Environment.NewLine;
                            }
                        }
                        _Error = _Error + "==========================";
                        w.WriteLine(_Error);
                        w.Flush();
                        w.Close();
                        _Error = _Error + log;
                    }
                    using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                    {
                        w.WriteLine(log);
                        w.Flush();
                        w.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                EmailClass objEmailClass = new EmailClass();
                objEmailClass.MailTo = "info@flightoffice.co.uk";
                objEmailClass.SendCommonEmail(3, subject, _Error);
            }
        }
    }
}