using PenYourPrayerServer.Supporting.HMAC;
using PenYourPrayerServer.Models;
using PenYourPrayerServer.Models.Struct;
using PenYourPrayerServer.Supporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Linq;

namespace PenYourPrayerServer.Controllers
{
    [HMACAuthorizeAttribute]
    [RoutePrefix("api/UserAccount")]
    public class UserAccountController : ApiController
    {
        [Route("CheckUserNameExists")]
        public HttpResponseMessage CheckUserNameExists(String LoginType, String UserName)
        {
            using (DBDataContext db = new DBDataContext())
            {
                List<usp_GetUserInformationResult> res = db.usp_GetUserInformation(LoginType, UserName).ToList();
                if (res.Count() == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.Accepted, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Accepted, Description = "NOT EXIST" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Accepted, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Accepted, Description = "EXISTS" });
                }
            }
        }

        [Route("RegisterNewUser")]
        public HttpResponseMessage RegisterNewUser(string LoginType, string UserName, string Name, string ProfilePictureURL, string Password, string MobilePlatform, string PushNotificationID,
                                        string Country, string Region, string City)
        {
            PenYourPrayerUser user = new PenYourPrayerUser();
            user.LoginType = LoginType;
            user.UserName = UserName.Trim();
            user.DisplayName = Name.Trim();
            user.ProfilePictureURL = ProfilePictureURL;
            user.Password = CustomPasswordHasher.HashPassword(Password);
            user.MobilePlatform = MobilePlatform;
            user.PushNotificationID = PushNotificationID;
            user.City = City;
            user.Region = Region;
            user.Country = Country;

            using (DBDataContext db = new DBDataContext())
            {
                string result = "";
                string HMACSecretKey = CustomPasswordHasher.HashPassword(Guid.NewGuid().ToString()) + CustomPasswordHasher.HashPassword(Guid.NewGuid().ToString());
                long? id = -1;
                string verificationCode = "";
                db.usp_AddNewUser(user.LoginType, user.UserName, user.DisplayName, user.ProfilePictureURL, user.Password, user.MobilePlatform, user.PushNotificationID, HMACSecretKey, user.Country, user.Region, user.City, ref result, ref id, ref verificationCode);
                user.Id = (long)id;

                if (result.ToUpper() != "OK")
                {                    
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.BadRequest, Description = result });
                }
                //send email to verify email address.
                CommonMethod.sendAccountActiviationEmail(user.UserName, user.DisplayName, verificationCode, user.Id.ToString());
                return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK });

            }
        }

        [Route("ResendAccountActivation")]
        public HttpResponseMessage ResendAccountActivation(String LoginType, String UserName)
        {
            using (DBDataContext db = new DBDataContext())
            {
                List<usp_GetUserActivationCodeResult> res = db.usp_GetUserActivationCode(LoginType, UserName).ToList();
                if (res.Count() == 1)
                {
                    CommonMethod.sendAccountActiviationEmail(res.ElementAt(0).UserName, res.ElementAt(0).DisplayName, res.ElementAt(0).ActivationCode, res.ElementAt(0).ID.ToString());
                    return Request.CreateResponse(HttpStatusCode.Accepted, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Accepted, Description = "OK" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.BadRequest, Description = "NOT EXISTS" });
                }
            }
        }

        [Route("ResetPassword")]
        public HttpResponseMessage ResetPassword(String LoginType, String UserName)
        {
            using (DBDataContext db = new DBDataContext())
            {
                long? ID = 1;
                string verficationCode = "";
                string result = "";
                string displayName = "";
                db.usp_ResetPassword(LoginType, UserName, ref result, ref ID, ref verficationCode, ref displayName);

                if (result == "OK")
                {
                    CommonMethod.sendResetPasswordEmail(UserName, displayName, verficationCode, ID.ToString());
                    return Request.CreateResponse(HttpStatusCode.Accepted, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Accepted, Description = "OK" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.BadRequest, Description = "NOT EXISTS" });
                }
                
            }
        }

        [Route("Login")]
        public HttpResponseMessage Login(string LoginType, string UserName, string Password_Secret, string AccessToken)
        {
            using (DBDataContext db = new DBDataContext())
            {
                List<usp_GetUserInformationResult> res = db.usp_GetUserInformation(LoginType, UserName).ToList();
                if (res.Count() > 0 && LoginType.ToUpper() == "EMAIL" && CustomPasswordHasher.VerifyHashedPassword(res.ElementAt(0).Password, Password_Secret))
                {

                    usp_GetUserInformationResult t = res.ElementAt(0);
                    PenYourPrayerUser user = new PenYourPrayerUser();
                    user.Id = t.ID;
                    user.DisplayName = t.DisplayName;
                    user.LoginType = t.LoginType;
                    user.UserName = t.UserName;
                    user.MobilePlatform = t.MobilePlatform;
                    user.ProfilePictureURL = t.ProfilePictureURL;
                    user.PushNotificationID = t.PushNotificationID;
                    user.HMACHashKey = t.HMACHashKey;
                    user.EmailVerification = t.EmailVerification;
                    if (!t.EmailVerification)
                        user.HMACHashKey = "";
                    return Request.CreateResponse(HttpStatusCode.OK, user);                        
                }
                else if (res.Count() > 0 && LoginType.ToUpper() == "FACEBOOK")
                {
                    bool result = SocialMediaAuthentication.CheckFacebookAccessToken("CAAXXIYv53qcBABWf4lQvRT0Rm3UgBXcF1foQ4SRTNDp7eaSvDFLe4fZC4BFqsE1YYTcdUQw3UvZCRkmdWZAFbu2hav9UuHZAoE9VcpLkKvsSZC3IfLUrHglCygQ5XbZBcH0ORI9t2QzKAjggPsrORxmVgovoHZCzl4wV56mv9cQPxvZBxTCiOJlrcdbh5JigAxXnQ2h5Yc0WinZAjcypHhrgZAL8BnwiKOECTDNFXgOtfbDQZDZD");
                    
                }
                else if (res.Count() > 0 && LoginType.ToUpper() == "TWITTER")
                {
                    bool result = SocialMediaAuthentication.checkTwitterAccessToken(UserName, "806837785-trTr0ObdqaW0owy1N0WXJFh6OGSlgUH74nh3qoHO", "w5j7WPwHWwY4DSfJ82tRVZF7SBogZJ6XABptVt431uOowvwFKC");
                }
                else if (res.Count() > 0 && LoginType.ToUpper() == "GOOGLEPLUS")
                {
                    bool result = SocialMediaAuthentication.CheckGooglePlusAccessToken("eyJhbGciOiJSUzI1NiIsImtpZCI6Ijk2MmM2NTc0MjVhNGE3YWE0ZGFhM2FiNGNlNjU0NWZhOGM0ZTAxYmYifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXVkIjoiMTAzNjE4MjAxODU4OS1xcTVlNDlhNzNzYzRwMHE5ZjAyaXNmaW41NnNuYmNzZC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjExNzg4NzA0NTM3ODc4ODY4NTMyOCIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhenAiOiIxMDM2MTgyMDE4NTg5LWtxMjZmMnFpM2dhZGlvMWFnZ3QwcWFvYzEzZ3Z2NjQ2LmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiZW1haWwiOiJ6bml0ZXI4MUBnbWFpbC5jb20iLCJpYXQiOjE0NDYxNzcxMDgsImV4cCI6MTQ0NjE4MDcwOCwibmFtZSI6IktpYW4gU2VuZyIsInBpY3R1cmUiOiJodHRwczovL2xoNi5nb29nbGV1c2VyY29udGVudC5jb20vLWNKRVp5aUk5N05VL0FBQUFBQUFBQUFJL0FBQUFBQUFBQnkwLzdGU2dMYmIxd21ZL3M5Ni1jL3Bob3RvLmpwZyIsImdpdmVuX25hbWUiOiJLaWFuIiwiZmFtaWx5X25hbWUiOiJTZW5nIn0.Ja-18lzCKorBORYExsjLcZpjhgMzYKLB4Vx9QCzyEt1dqPlg7uzAVmqy0O6i3CzKB2i5bt6jCarBTh5Vnt4OdaVjeDyqAu1sz1v9r6VBCzqmtgDsJa1HLs_NZUK19uLPIIIPobAlAcryPGIDBsnIoDe0sVcs57dkbZXjpohnc8M8nnPNrYkFMQaG1yEuz8MwbgoXRqEKjt0gCetavSU2stAR21QrC4ojfXeAcF1EHvrZgv3UceejtI5Qu3ytajc2YYPCvRcPX6iE5JJUz4sHIu0GMfG-fri5CLgP9PkgpH36-uJpo14gqFMSeg21yXBJnQBhCDmrc4MLm0-I-w9E_g");                    

                }
                
                return Request.CreateResponse(HttpStatusCode.BadRequest, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.BadRequest, Description = "Invalid UserID/Password" });
            }
        }        
    }
}
