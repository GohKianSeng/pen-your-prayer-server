﻿using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Xml.Serialization;


namespace PenYourPrayerServer.Models.Struct
{
    [XmlRootAttribute("OwnerProfile")]
    public class PenYourPrayerUser
    {

        public long ID { get; set; }
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
        public bool EmailVerification { get; set; }

        public static explicit operator PenYourPrayerUser(PenYourPrayerIdentity c)
        {
            PenYourPrayerUser pyp = new PenYourPrayerUser();
            pyp.ID = c.ID;
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