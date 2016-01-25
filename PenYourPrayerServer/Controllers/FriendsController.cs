using PenYourPrayerServer.Models;
using PenYourPrayerServer.Models.Struct;
using PenYourPrayerServer.Supporting;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PenYourPrayerServer.Controllers
{
    [HMACAuthorizeAttribute]
    [RoutePrefix("api/Friends")]
    public class FriendsController : ApiController
    {
        [HttpPost]
        [Route("GetLatestFriends")]
        public HttpResponseMessage GetLatestFriends(List<Friends> friend)
        {
            String userid = "";
            foreach (Friends f in friend)
            {
                userid += f.UserID.ToString() + ";";
            }
            
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            List<Friends> newfriend = new List<Friends>();
            using (DBDataContext db = new DBDataContext())
            {
                List<usp_GetLatestFriendsResult> res = db.usp_GetLatestFriends(user.Id, userid).ToList();
                foreach (usp_GetLatestFriendsResult f in res)
                {
                    newfriend.Add((Friends)f);
                }                  
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, newfriend);
        }
        
    }
}
