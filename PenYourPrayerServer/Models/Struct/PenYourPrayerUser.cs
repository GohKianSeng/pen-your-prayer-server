using PenYourPrayerServer.Supporting.HMAC;
using System;


namespace PenYourPrayerServer.Models.Struct
{
    public class PenYourPrayerUser
    {

        public long Id { get; set; }
        public string LoginType { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureURL { get; set; }
        public string Password { get; set; }
        public string MobilePlatform { get; set; }
        public string PushNotificationID { get; set; }
        public string HMACHashKey { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public static explicit operator PenYourPrayerUser(PenYourPrayerIdentity c)
        {
            PenYourPrayerUser pyp = new PenYourPrayerUser();
            pyp.Id = c.Id;
            pyp.LoginType = c.LoginType;
            pyp.UserName = c.UserName;
            pyp.DisplayName = c.DisplayName;
            pyp.ProfilePictureURL = c.ProfilePictureURL;
            pyp.MobilePlatform = c.MobilePlatform;
            pyp.PushNotificationID = c.PushNotificationID;
            return pyp;
        }
    }
}