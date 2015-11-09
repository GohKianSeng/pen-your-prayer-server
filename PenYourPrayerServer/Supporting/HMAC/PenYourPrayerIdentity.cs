using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PenYourPrayerServer.Supporting.HMAC
{

    public class PenYourPrayerIdentity : GenericIdentity
    {
        public long Id { get; set; }
        public string LoginType { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureURL { get; set; }
        public string MobilePlatform { get; set; }
        public string PushNotificationID { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public PenYourPrayerIdentity(long GUID, string LoginType, string UserName)
            : base(GUID.ToString())
        {
            this.LoginType = LoginType;
            this.UserName = UserName;
            this.Id = GUID;
        }        

    }
}