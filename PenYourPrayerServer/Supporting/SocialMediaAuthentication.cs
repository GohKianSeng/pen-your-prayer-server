using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace PenYourPrayerServer.Supporting
{
    public class SocialMediaAuthentication
    {
        public static bool CheckFacebookAccessToken(string accessToken, ref object fbToken)
        {
            using (WebClient client = new WebClient())
            {
                string data = client.DownloadString("https://graph.facebook.com/debug_token?access_token=1643913965854375|k20-5EWrMw-Y3HQK9KpriMvqtaI&input_token=" + accessToken);
                string data2 = client.DownloadString("https://graph.facebook.com/me?fields=id,name,email,picture.type(large)&access_token=" + accessToken);

                
                JavaScriptSerializer serializer1 = new JavaScriptSerializer();
                FacebookDebugToken token = serializer1.Deserialize<FacebookDebugToken>(data);

                JavaScriptSerializer serializer2 = new JavaScriptSerializer();
                token.data.usertoken = serializer2.Deserialize<FacebookUserToken>(data2);

                if (token == null)
                    return false;

                fbToken = token;
                return token.data.is_valid;                
            }            
        }

        public static bool CheckGooglePlusAccessToken(string accessToken, ref object gpToken)
        {
            //https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=
            using (WebClient client = new WebClient())
            {
                try
                {
                    string data = client.DownloadString("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + accessToken);


                    JavaScriptSerializer serializer1 = new JavaScriptSerializer();
                    GoogleTokenInfo token = serializer1.Deserialize<GoogleTokenInfo>(data);

                    if (token.aud != null)
                    {
                        gpToken = token;
                        return token.aud.ToUpper().StartsWith("1036182018589");
                    }
                    else
                        return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        
        }

        public static bool checkTwitterAccessToken(string id, string oauthtoken, string oauthtokensecret)
        {
            try{
                string data = TwitterVerifyCredentials(oauthtoken, oauthtokensecret);
                JavaScriptSerializer serializer1 = new JavaScriptSerializer();
                TwitterTokenInfo token = serializer1.Deserialize<TwitterTokenInfo>(data);

                return token.id == id;
                
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        private static string TwitterVerifyCredentials(string oauthtoken, string oauthtokensecret)
        {
            string oauthconsumerkey = "jSBnTpknelOuZX6e4Cg101oue";
            string oauthconsumersecret = "w5j7WPwHWwY4DSfJ82tRVZF7SBogZJ6XABptVt431uOowvwFKC";
            string oauthsignaturemethod = "HMAC-SHA1";
            string oauthversion = "1.0";
            oauthtoken = "806837785-trTr0ObdqaW0owy1N0WXJFh6OGSlgUH74nh3qoHO";
            oauthtokensecret = "AhkVPH5D1aKfUM7POHMizOf8HHbIaYuEtEo5KCCt5kUBk";
            string oauthnonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string oauthtimestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            SortedDictionary<string, string> basestringParameters = new SortedDictionary<string, string>();
            basestringParameters.Add("oauth_version", "1.0");
            basestringParameters.Add("oauth_consumer_key", oauthconsumerkey);
            basestringParameters.Add("oauth_nonce", oauthnonce);
            basestringParameters.Add("oauth_signature_method", "HMAC-SHA1");
            basestringParameters.Add("oauth_timestamp", oauthtimestamp);
            basestringParameters.Add("oauth_token", oauthtoken);
            //GS - Build the signature string
            StringBuilder baseString = new StringBuilder();
            baseString.Append("GET" + "&");
            baseString.Append(EncodeCharacters(Uri.EscapeDataString("https://api.twitter.com/1.1/account/verify_credentials.json") + "&"));
            foreach (KeyValuePair<string, string> entry in basestringParameters)
            {
                baseString.Append(EncodeCharacters(Uri.EscapeDataString(entry.Key + "=" + entry.Value + "&")));
            }

            //Since the baseString is urlEncoded we have to remove the last 3 chars - %26
            string finalBaseString = baseString.ToString().Substring(0, baseString.Length - 3);

            //Build the signing key
            string signingKey = EncodeCharacters(Uri.EscapeDataString(oauthconsumersecret)) + "&" +
            EncodeCharacters(Uri.EscapeDataString(oauthtokensecret));

            //Sign the request
            HMACSHA1 hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));
            string oauthsignature = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(finalBaseString)));

            //Tell Twitter we don't do the 100 continue thing
            ServicePointManager.Expect100Continue = false;

            //authorization header
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(@"https://api.twitter.com/1.1/account/verify_credentials.json");
            StringBuilder authorizationHeaderParams = new StringBuilder();
            authorizationHeaderParams.Append("OAuth ");
            authorizationHeaderParams.Append("oauth_nonce=" + "\"" + Uri.EscapeDataString(oauthnonce) + "\",");
            authorizationHeaderParams.Append("oauth_signature_method=" + "\"" + Uri.EscapeDataString(oauthsignaturemethod) + "\",");
            authorizationHeaderParams.Append("oauth_timestamp=" + "\"" + Uri.EscapeDataString(oauthtimestamp) + "\",");
            authorizationHeaderParams.Append("oauth_consumer_key=" + "\"" + Uri.EscapeDataString(oauthconsumerkey) + "\",");
            if (!string.IsNullOrEmpty(oauthtoken))
                authorizationHeaderParams.Append("oauth_token=" + "\"" + Uri.EscapeDataString(oauthtoken) + "\",");
            authorizationHeaderParams.Append("oauth_signature=" + "\"" + Uri.EscapeDataString(oauthsignature) + "\",");
            authorizationHeaderParams.Append("oauth_version=" + "\"" + Uri.EscapeDataString(oauthversion) + "\"");
            hwr.Headers.Add("Authorization", authorizationHeaderParams.ToString());
            hwr.Method = "GET";
            hwr.ContentType = "application/x-www-form-urlencoded";

            //Allow us a reasonable timeout in case Twitter's busy
            hwr.Timeout = 3 * 60 * 1000;
            try
            {
                HttpWebResponse rsp = hwr.GetResponse() as HttpWebResponse;
                Stream dataStream = rsp.GetResponseStream();
                //Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                //Read the content.
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {

            }

            return "";
        }

        private static string EncodeCharacters(string data)
        {
            //as per OAuth Core 1.0 Characters in the unreserved character set MUST NOT be encoded
            //unreserved = ALPHA, DIGIT, '-', '.', '_', '~'
            if (data.Contains("!"))
                data = data.Replace("!", "%21");
            if (data.Contains("'"))
                data = data.Replace("'", "%27");
            if (data.Contains("("))
                data = data.Replace("(", "%28");
            if (data.Contains(")"))
                data = data.Replace(")", "%29");
            if (data.Contains("*"))
                data = data.Replace("*", "%2A");
            if (data.Contains(","))
                data = data.Replace(",", "%2C");

            return data;
        }
    }

    public class FacebookUserToken
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public FacebookPicture picture { get; set; }

        public class FacebookPicture
        {
            public FacebookPictureData data;

        }

        public class FacebookPictureData
        {
            public bool is_silhouette { get; set; }
            public string url { get; set; }
        }
    }

    public class FacebookDebugToken
    {
        public FacebookDebugTokenData data { get; set; }

        public class FacebookDebugTokenData
        {
            public string app_id { get; set; }
            public string application { get; set; }
            public long expires_at { get; set; }
            public long issued_at { get; set; }
            public bool is_valid { get; set; }
            public string user_id { get; set; }
            public FacebookUserToken usertoken { get; set; }

        }
    }

    public class GoogleTokenInfo
    {
        public string iss { get; set; }
        public string aud { get; set; }
        
        //sub is user unique google plus id
        public string sub { get; set; }
        public bool email_verified { get; set; }
        public string email { get; set; }
        public long iat { get; set; }
        public long exp { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string alg { get; set; }
        public string kid { get; set; }
    }

    public class TwitterTokenInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string profile_background_image_url_https { get; set; }

    }
}