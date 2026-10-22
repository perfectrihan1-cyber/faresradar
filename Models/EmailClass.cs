using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace Viman.Models
{
    public enum AttachementType
    {
        Image,
        File,
        Empty
    }

    public class EmailClass
    {
        DataTable dtEmailOptions;
        string _UserName = string.Empty;
        string _MailTo = string.Empty;
        string _MailCC = string.Empty;
        string _MailBCC = string.Empty;
        string CompanyAddress = "";

        public string UserName { get { return _UserName; } set { _UserName = value; } }
        public string MailTo { get { return _MailTo; } set { _MailTo = value.Replace(',', ';'); } }
        public string MailCC { get { return _MailCC; } set { _MailCC = value.Replace(',', ';'); } }
        public string MailBCC { get { return _MailBCC; } set { _MailBCC = value.Replace(',', ';'); } }

        public EmailClass()
        {
            try
            {
                //dtEmailOptions = SqlHelper.ExecuteDataset(SqlHelper.TravelExpressCon, CommandType.Text, "Select * From MailingDetail").Tables[0];
                dtEmailOptions = SqlHelper.ExecuteDataset("Select * From MailingDetail").Tables[0];
            }
            catch (Exception)
            {
            }
        }

        public bool SendCommonEmail(string Subject, string Body)
        {
            return SendEmail(1, "Viman", Subject, Body, string.Empty, AttachementType.Empty);
        }

        public bool SendCommonEmail(int Option, string Subject, string Body)
        {
            return SendEmail(Option, "Flyglobe", Subject, Body, string.Empty, AttachementType.Empty);
        }

        public bool SendCommonEmail(int EmailFromID, string Subject, string Body, string Attachement, AttachementType Type)
        {
            return SendEmail(EmailFromID, "Flyglobe", Subject, Body, Attachement, Type);
        }

        bool SendEmail(int OptionType, string DisplayMessage, string MailSubject, string MailBody, string AttachementPath, AttachementType _AttachementType)
        {
            DataTable dtEmail;
            if (dtEmailOptions != null && dtEmailOptions.Rows.Count > 0)
            {
                dtEmailOptions.DefaultView.RowFilter = "OptionType=" + OptionType;
                dtEmail = dtEmailOptions.DefaultView.ToTable();
            }
            else
            {
                dtEmail = GetBackupEmail();
            }
            AttachementPath = AttachementPath.Replace(',', ';');
            bool flag = false;
            MailMessage _MailMessage = new MailMessage();
            try
            {
                string[] ID;
                _MailMessage.IsBodyHtml = true;

                if (dtEmail.Rows[0]["Bcc1"].ToString() != "")
                {
                    _MailMessage.Bcc.Add(dtEmail.Rows[0]["Bcc1"].ToString().Trim());
                }
                if (dtEmail.Rows[0]["Bcc2"].ToString() != "")
                {
                    _MailMessage.Bcc.Add(dtEmail.Rows[0]["Bcc2"].ToString().Trim());
                }
                if (dtEmail.Rows[0]["Bcc3"].ToString() != "")
                {
                    _MailMessage.Bcc.Add(dtEmail.Rows[0]["Bcc3"].ToString().Trim());
                }
                if (dtEmail.Rows[0]["Bcc4"].ToString() != "")
                {
                    _MailMessage.Bcc.Add(dtEmail.Rows[0]["Bcc4"].ToString().Trim());
                }

                if (_MailTo != "")
                {
                    ID = _MailTo.Split(';');
                    for (int i = 0; i < ID.Count(); i++)
                    {
                        _MailMessage.To.Add(ID[i]);
                    }
                }
                if (_MailCC != "")
                {
                    ID = _MailCC.Split(';');
                    for (int i = 0; i < ID.Count(); i++)
                    {
                        _MailMessage.CC.Add(ID[i]);
                    }
                }
                if (_MailBCC != "")
                {
                    ID = _MailBCC.Split(';');
                    for (int i = 0; i < ID.Count(); i++)
                    {
                        _MailMessage.Bcc.Add(ID[i]);
                    }
                }
                _MailMessage.From = new MailAddress(dtEmail.Rows[0]["FromAddress"].ToString(), DisplayMessage);
                _MailMessage.Subject = MailSubject;
                _MailMessage.Body = MailBody + CompanyAddress;

                if (AttachementPath != "")
                {
                    ID = AttachementPath.Split(';');
                    switch (_AttachementType)
                    {
                        case AttachementType.File:
                            for (int i = 0; i < ID.Count(); i++)
                            {
                                _MailMessage.Attachments.Add(new Attachment(ID[i]));
                            }
                            break;
                        case AttachementType.Image:
                            for (int i = 0; i < ID.Count(); i++)
                            {
                                string attachment = ID[i];
                                string contentID = System.IO.Path.GetFileName(attachment).Replace(".", "") + "@zofm";
                                Attachment inline = new Attachment(HttpContext.Current.Server.MapPath(attachment));
                                inline.ContentDisposition.Inline = true;
                                inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                                inline.ContentId = contentID;
                                inline.ContentType.Name = System.IO.Path.GetFileName(attachment);
                                _MailMessage.Attachments.Add(inline);
                                _MailMessage.Body = _MailMessage.Body.Replace(attachment, "cid:" + contentID);
                            }
                            break;
                    }
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = dtEmail.Rows[0]["Host"].ToString();
                smtp.Port = int.Parse(dtEmail.Rows[0]["Port"].ToString());
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                if (dtEmail.Rows[0]["Host"].ToString() == "auth.smtp.1and1.co.uk" || dtEmail.Rows[0]["Host"].ToString() == "smtp.gmail.com")
                    smtp.EnableSsl = true;
                NetworkCredential objCred = new NetworkCredential(dtEmail.Rows[0]["FromAddress"].ToString(), dtEmail.Rows[0]["Password"].ToString());
                smtp.Credentials = objCred;
                smtp.Send(_MailMessage);
                flag = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _MailMessage.Dispose();
                _UserName = string.Empty;
                _MailTo = string.Empty;
                _MailCC = string.Empty;
                _MailBCC = string.Empty;
            }
            return flag;
        }

        bool BackupEmail(MailMessage _MailMessage)
        {
            bool flag = false;
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "auth.smtp.1and1.co.uk";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential objCred = new NetworkCredential("", "");
                    smtp.Credentials = objCred;
                    smtp.Send(_MailMessage);
                    flag = true;
                }
            }
            catch (Exception)
            {

            }
            return flag;
        }

        bool LastBackupEmail(MailMessage _MailMessage)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "auth.smtp.1and1.co.uk";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential objCred = new NetworkCredential("", "");
                    smtp.Credentials = objCred;
                    smtp.Send(_MailMessage);
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        private DataTable GetBackupEmail()
        {
            dtEmailOptions = new DataTable();
            dtEmailOptions.Columns.Add("OptionType", typeof(int));
            dtEmailOptions.Columns.Add("Host", typeof(string));
            dtEmailOptions.Columns.Add("Port", typeof(int));
            dtEmailOptions.Columns.Add("FromAddress", typeof(string));
            dtEmailOptions.Columns.Add("Password", typeof(string));
            dtEmailOptions.Columns.Add("Bcc1", typeof(string));
            dtEmailOptions.Columns.Add("Bcc2", typeof(string));
            dtEmailOptions.Columns.Add("Bcc3", typeof(string));
            dtEmailOptions.Columns.Add("Bcc4", typeof(string));

            //DataRow dr = dtEmailOptions.NewRow();
            //dr["OptionType"] = 1;
            //dr["Host"] = "auth.smtp.1and1.co.uk";
            //dr["Port"] = 587;
            //dr["FromAddress"] = "info@viman.co.uk";
            //dr["Password"] = "Metr0P@rlament#5678";
            //dr["Bcc1"] = "";
            //dr["Bcc2"] = "";
            //dr["Bcc3"] = "";
            //dr["Bcc4"] = "";
            //dtEmailOptions.Rows.Add(dr);
            //return dtEmailOptions;

            DataRow dr = dtEmailOptions.NewRow();
            dr["OptionType"] = 1;
            dr["Host"] = "webmail.flyglobe.co.uk";
            dr["Port"] = 465;
            dr["FromAddress"] = "sales@theflyglobe.com";
            dr["Password"] = "Mover@123";
            dr["Bcc1"] = "";
            dr["Bcc2"] = "";
            dr["Bcc3"] = "";
            dr["Bcc4"] = "";
            dtEmailOptions.Rows.Add(dr);
            return dtEmailOptions;
        }
    }
}