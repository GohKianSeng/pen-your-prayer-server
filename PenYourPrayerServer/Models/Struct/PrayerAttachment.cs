using PenYourPrayerServer.Supporting.HMAC;
using System;


namespace PenYourPrayerServer.Models.Struct
{
    public class PrayerAttachment
    {

        public long PrayerID { get; set; }
        public string GUID { get; set; }
        public string OriginalFilePath { get; set; }
        public string URLPath { get; set; }
        public string UserID { get; set; }
        public string FileName { get; set; }
        
    }
}