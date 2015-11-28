using PenYourPrayerServer.Supporting.HMAC;
using System;


namespace PenYourPrayerServer.Models.Struct
{
    public class Friends
    {

        public long UserID { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureURL { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        
        
    }
}