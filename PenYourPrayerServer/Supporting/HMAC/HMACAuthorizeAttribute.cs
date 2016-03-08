using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Security.Cryptography;
using PenYourPrayerServer.Models.Struct;
using PenYourPrayerServer.Models;
using System.Data.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace PenYourPrayerServer.Supporting.HMAC
{
    public class HMACAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            int HMAC_Valid_inMinutes = 10;

            /**
                Pen Your Prayer Authorization Specification
                Encoding before send.
                Authorization for Registered User
             
                Authorization: hmac_logintype="email";
                hmac_username="zniter81@gmail.com";
                hmac_signature="wOJIO9A2W5mFwDgiDvZbTSMK%2FPY%3D";
                hmac_timestamp="Sun, 01 Nov 2015 15:31:08 GMT";
                hmac_nonce="4572616e48616d6d65724c61686176" random int value 
                                     
                hashing order method + LoginType + UserName + tdate + nonce + query + content;                                       
             */
            if (System.Configuration.ConfigurationManager.AppSettings["DebugMode"] != null && System.Configuration.ConfigurationManager.AppSettings["DebugMode"].ToUpper() == "TRUE")
            {
                using (DBDataContext db = new DBDataContext())
                {
                    List<usp_GetUserInformationResult> d = db.usp_GetUserInformation("Email", "mail@pyptesting.com").ToList();
                    PenYourPrayerIdentity identity = new PenYourPrayerIdentity(d.ElementAt(0).ID, d.ElementAt(0).LoginType, d.ElementAt(0).UserName)
                    {

                        DisplayName = d.ElementAt(0).DisplayName,
                        ProfilePictureURL = d.ElementAt(0).ProfilePictureURL,
                        MobilePlatform = d.ElementAt(0).MobilePlatform,
                        PushNotificationID = d.ElementAt(0).PushNotificationID,
                        City = d.ElementAt(0).City,
                        Region = d.ElementAt(0).Region,
                        Country = d.ElementAt(0).Country
                    };

                    IPrincipal principal = new GenericPrincipal(identity, null);
                    actionContext.RequestContext.Principal = principal;
                    return true;
                }
                return false;
            }
            else if (actionContext.Request.Headers.Authorization != null)
            {
                try
                {
                    string method = actionContext.Request.Method.ToString().ToUpper();
                    string LoginType = "";
                    string UserName = "";
                    string tdate = "";
                    DateTime tDateTime = new DateTime(0);
                    string receivedHash = "";
                    string nonce = "";

                    string encodedAuthorizationHeader = actionContext.Request.Headers.Authorization.ToString();
                    string[] AuthorizationHeaders = HttpUtility.UrlDecode(encodedAuthorizationHeader).Split(';');
                    foreach(string s in AuthorizationHeaders){
                        string[] t = new string[2];
                        t[0] = s.Substring(0, s.IndexOf("=")).Trim();
                        t[1] = s.Substring(s.IndexOf("=") + 1).Trim();
                        if (t[0].ToUpper() == "HMAC_LOGINTYPE")
                        {
                            LoginType = t[1].Substring(1, t[1].Length - 2).ToUpper();
                        }                                                
                        else if (t[0].ToUpper() == "HMAC_USERNAME")
                        {
                            UserName = t[1].Substring(1, t[1].Length - 2).ToUpper();
                        }
                        else if (t[0].ToUpper() == "HMAC_SIGNATURE")
                        {
                            receivedHash = t[1].Substring(1, t[1].Length - 2);
                        }
                        else if (t[0].ToUpper() == "HMAC_NONCE")
                        {
                            nonce = t[1].Substring(1, t[1].Length - 2);
                        }
                        else if (t[0].ToUpper() == "HMAC_TIMESTAMP")
                        {
                            tdate = t[1].Substring(1, t[1].Length - 2);
                        }                        
                    }
                    //

                    string query = "";
                    if (actionContext.Request.RequestUri.Query.Length > 0)
                        query = HttpUtility.UrlDecode(HttpUtility.UrlDecode(actionContext.Request.RequestUri.Query.Substring(1)));
                    //string content = content = actionContext.Request.Content.ReadAsStringAsync().Result;

                    byte[] contentBytes = actionContext.Request.Content.ReadAsByteArrayAsync().Result;
                    string contentMD5 = "";
                    if (method != "GET")
                        contentMD5 = md5CheckSum(contentBytes).ToUpper();
                    //DBDataContext dda = new DBDataContext();
                    //dda.usp_AddLog(DateTime.Now.ToString() + " content md5: " + contentMD5);
                    //dda.Connection.Close();
                    
                    //if (content.StartsWith("\""))
                    //    content = content.Substring(1);
                    //if(content.EndsWith("\""))
                    //    content = content.Substring(0, content.Length-1);
                   
                    if (method == null || nonce == null || receivedHash == null || LoginType == null || UserName == null || tdate == null || LoginType.Length == 0 || UserName.Length == 0 || tdate.Length == 0 || receivedHash.Length == 0 || nonce.Length == 0 || method.Length == 0)
                    {
                        return false; 
                    }
                    else
                    {
                        //check time difference not more than 5sec
                        DateTime d = DateTime.UtcNow;
                        tDateTime = DateTime.ParseExact(tdate, "r", CultureInfo.InvariantCulture);
                        long timeDifference = (long)((d - tDateTime).TotalSeconds);

                        if (timeDifference > (HMAC_Valid_inMinutes * 60) || timeDifference < (-HMAC_Valid_inMinutes * 60))
                            return false;
                    }
                                       
                    PenYourPrayerIdentity identity;
                    string HMACHashKey = "";

                    if (LoginType != "ANONYMOUS")
                    {
                        using (DBDataContext db = new DBDataContext())
                        {
                            string result = "";
                            db.usp_AddNonce(LoginType, UserName, DateTime.ParseExact(tdate, "r", CultureInfo.InvariantCulture), (int?)int.Parse(nonce), ref result);
                            if (result.ToUpper() == "EXISTS")
                                return false;

                            List<usp_GetUserInformationResult> d = db.usp_GetUserInformation(LoginType, UserName).ToList();
                            if (d.Count() == 0)
                            {
                                return false;
                            }
                            else if (d.Count() == 1)
                            {



                                identity = new PenYourPrayerIdentity(d.ElementAt(0).ID, d.ElementAt(0).LoginType, d.ElementAt(0).UserName)
                                {

                                    DisplayName = d.ElementAt(0).DisplayName,
                                    ProfilePictureURL = d.ElementAt(0).ProfilePictureURL,
                                    MobilePlatform = d.ElementAt(0).MobilePlatform,
                                    PushNotificationID = d.ElementAt(0).PushNotificationID,
                                    City = d.ElementAt(0).City,
                                    Region = d.ElementAt(0).Region,
                                    Country = d.ElementAt(0).Country
                                };
                                HMACHashKey = d.ElementAt(0).HMACHashKey;
                            }
                            else
                                return false;
                        }

                        if (LoginType.ToUpper() != identity.LoginType.ToUpper() || UserName.ToUpper() != identity.UserName.ToUpper())
                        {
                            return false;
                        }

                    }
                    else
                    {
                        using(DBDataContext db = new DBDataContext()){
                            string result = "";
                            db.usp_AddNonce(LoginType, UserName, DateTime.ParseExact(tdate, "r", CultureInfo.InvariantCulture), (int?)int.Parse(nonce), ref result);
                            if (result.ToUpper() == "EXISTS")
                                return false;
                        }

                        identity = new PenYourPrayerIdentity(0, "", "");
                        HMACHashKey = QuickReference.AnonymousHMACKey;
                    }
                    byte[] key = Encoding.ASCII.GetBytes(HMACHashKey);
                    string contentToHash = method + LoginType + UserName + tdate + nonce + md5CheckSum(query).ToUpper() + contentMD5;
                                        
                    if (Encode(contentToHash, key) != receivedHash)
                    {
                        return false;
                    }

                    IPrincipal principal = new GenericPrincipal(identity, null);
                    actionContext.RequestContext.Principal = principal;

                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        private string md5CheckSum(string theString)
        {
            string hash = "";
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = BitConverter.ToString(
                  md5.ComputeHash(Encoding.UTF8.GetBytes(theString))
                ).Replace("-", String.Empty);
            }
            return hash;
        }

        private string md5CheckSum(byte[] content)
        {
            string hash = "";
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = BitConverter.ToString(
                  md5.ComputeHash(content)
                ).Replace("-", String.Empty);
            }
            return hash;
        }

        private string Encode(string input, byte[] key)
        {
            HMACSHA1 myhmacsha1 = new HMACSHA1(key);
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            byte[] s1 = myhmacsha1.ComputeHash(stream);
            return System.Convert.ToBase64String(s1);            
        }
    }
}