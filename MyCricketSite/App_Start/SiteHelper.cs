using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyCricketSite
{
    public static class SiteHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public static string UrlDomain()
        {
            string fullUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToString(CultureInfo.InvariantCulture);
            int slashIndex = fullUrl.IndexOf('/', 8);
            string url = fullUrl.Substring(0, slashIndex);
            url = url.Replace("https://", "").Replace("http://", "");
            return url;
        }

        public static string GetSiteUrl()
        {
            var siteUrl = HttpContext.Current.Request.Url.Scheme + @"://" + HttpContext.Current.Request.Url.Host;
            if (siteUrl.ToLower().Contains("localhost"))
                siteUrl += ":" + HttpContext.Current.Request.Url.Port + "/";
            else
                siteUrl += "/";
            return siteUrl;
        }
        //public static void SendEmail(string emailTo, string emailBody, string emailSubject, string emailFrom = "", string emailCc = "", string emailBcc = "", MailPriority msgPriority = MailPriority.Normal)
        //{
        //    try
        //    {
        //        emailTo = emailTo.Replace(";", ",");
        //        emailCc = emailCc.Replace(";", ",");
        //        emailBcc = emailBcc.Replace(";", ",");
        //        if (emailFrom.Equals("")) emailFrom = GetAppSettingsValue(AppSettingsKey.EmailFrom);
        //        const string emailRegularExpression = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        //        MailMessage mm = new MailMessage
        //        {
        //            From = new MailAddress(emailFrom),
        //            Priority = msgPriority,
        //            Subject = emailSubject,
        //            Body = emailBody,
        //            IsBodyHtml = true
        //        };

        //        foreach (string to in emailTo.Split(','))
        //        {
        //            Match match = Regex.Match(to.Trim(), emailRegularExpression, RegexOptions.IgnoreCase);
        //            if (match.Success) mm.To.Add(to.Trim());

        //        }
        //        if (emailCc.Length > 0)
        //        {
        //            foreach (string cc in emailCc.Split(','))
        //            {
        //                Match match = Regex.Match(cc.Trim(), emailRegularExpression, RegexOptions.IgnoreCase);
        //                if (match.Success) mm.CC.Add(cc.Trim());

        //            }
        //        }
        //        if (emailBcc.Length > 0)
        //        {
        //            foreach (string bcc in emailBcc.Split(','))
        //            {
        //                Match match = Regex.Match(bcc.Trim(), emailRegularExpression, RegexOptions.IgnoreCase);
        //                if (match.Success) mm.Bcc.Add(bcc.Trim());

        //            }
        //        }

        //        SmtpClient client = new SmtpClient();

        //        client.Send(mm);

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("[SendEmail] Stack Trace:" + ex.StackTrace + Environment.NewLine + "Error Message:" + ex);

        //    }
        //}



        public static string RenderPartialViewAsString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.ToString();
            }
        }

        public static string Decrypt_TripleDES(string EncryptedData, string key, bool useHashing = true)
        {
            string returnVal = String.Empty;
            try
            {
                byte[] keyArray;
                // 64-bit encoding does not work well with spaces in the string for some odd reason.  workaround : replace space with "+"
                EncryptedData = EncryptedData.Replace(" ", "+");
                byte[] toDecryptArray = Convert.FromBase64String(EncryptedData);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd = new MD5CryptoServiceProvider();
                    keyArray = hashmd.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd.Clear();
                }
                else
                {
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }
                TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
                tDes.Key = keyArray;
                tDes.Mode = CipherMode.ECB;
                tDes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tDes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

                tDes.Clear();
                returnVal = UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);

            }
            catch (Exception ex) { }
            return returnVal;
        }

        public static string Encrypt_TripleDES(string data, string Key, bool useHashing = true)
        {
            string returnVal = String.Empty;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(data);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(Key);
            }
            TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tDes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tDes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }




        public static string GetRandomPassword()
        {
            string newPWD = null;
            ArrayList myarray = new ArrayList();
            myarray.Add("Welcome");
            myarray.Add("LetMeIn");
            myarray.Add("P@ssword");
            myarray.Add("pa$$word");
            myarray.Add("LogMeIn");
            myarray.Add("Incentives");
            myarray.Add("Money4Me");
            myarray.Add("Go$hopping");
            myarray.Add("CreditCard");
            myarray.Add("CodeMe");
            myarray.Add("P@ssword");

            newPWD = myarray[(new Random()).Next(0, 10)].ToString() + (new Random()).Next(10, 100).ToString();

            return newPWD;

        }



        public static string PrePathSlashes
        {
            get
            {
                string prepath = "";
                int pdeep = PathsDeep();
                if (pdeep > 0)
                {
                    for (int i = 1; i <= pdeep; i++)
                    {
                        prepath = prepath + "../";
                    }
                }

                return prepath;
            }

        }

        public static int PathsDeep()
        {
            string path = HttpContext.Current.Request.CurrentExecutionFilePath.Replace("https://", "").Replace("http://", "");
            int pathslevel = 0;
            string[] pathslashes = path.Split('/');
            try
            {
                if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsVirtualDirectory"].ToString())) { pathslevel = pathslashes.Length - 3; }
                else pathslevel = pathslashes.Length - 2;



            }
            catch (Exception err)
            {

            }


            return pathslevel;
        }





        public static string URLDomain()
        {
            string FullURL = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            int SlashIndex = FullURL.IndexOf('/', 8);
            string url = FullURL.Substring(0, SlashIndex);
            url = url.Replace("https://", "").Replace("http://", "");
            return url;
        }



        public static bool SendEmail(string EmailTo, string EmailBody, string EmailSubject, string EmailFrom = "", string EmailCC = "", string EmailBCC = "", System.Net.Mail.MailPriority msgPriority = System.Net.Mail.MailPriority.Normal)
        {
            EmailTo = EmailTo.Replace(";", ",");
            EmailCC = EmailCC.Replace(";", ",");
            EmailBCC = EmailBCC.Replace(";", ",");
            // if (EmailFrom.Equals("")) EmailFrom = GetAppSettingsValue("EmailFrom");
            string EmailRegularExpression = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage();
            mm.From = new System.Net.Mail.MailAddress(EmailFrom);
            mm.Priority = msgPriority;
            mm.Subject = EmailSubject;
            mm.Body = EmailBody;
            mm.IsBodyHtml = true;

            foreach (string to in EmailTo.Split(','))
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(to.Trim(), EmailRegularExpression, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (match.Success) mm.To.Add(to.Trim());

            }
            if (EmailCC.Length > 0)
            {
                foreach (string cc in EmailCC.Split(','))
                {
                    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(cc.Trim(), EmailRegularExpression, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (match.Success) mm.CC.Add(cc.Trim());

                }
            }
            if (EmailBCC.Length > 0)
            {
                foreach (string bcc in EmailBCC.Split(','))
                {
                    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(bcc.Trim(), EmailRegularExpression, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (match.Success) mm.Bcc.Add(bcc.Trim());

                }
            }

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            try
            {
                client.Send(mm);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }






        public static string ConvertDataTableToJSON(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);



        }
        public static string ConvertDataSetToJSON(DataSet ds)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (DataTable dt in ds.Tables)
            {
                object[] arr = new object[dt.Rows.Count];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    arr[i] = dt.Rows[i].ItemArray;
                }
                dict.Add(dt.TableName, arr);
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return serializer.Serialize(dict);
        }


        public static void DatatableToCSV(DataTable dt)
        {
            var result = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                result.Append(dt.Columns[i].ColumnName);
                result.Append(i == dt.Columns.Count - 1 ? "\n" : ",");
            }

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    result.Append(row[i].ToString());
                    result.Append(i == dt.Columns.Count - 1 ? "\n" : ",");
                }
            }
            File.WriteAllText("test.csv", result.ToString());

        }
    }





}