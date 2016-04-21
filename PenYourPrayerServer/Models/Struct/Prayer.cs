using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace PenYourPrayerServer.Models.Struct
{
    public class Prayer
    {
        public long UserID { get; set; }

        public long PrayerID { get; set; }
        
        public long TouchedWhen { get; set; }
      
        public long CreatedWhen { get; set; }

        public string Content { get; set; }

        public List<Friends> selectedFriends { get; set; }

        public bool publicView { get; set; }

        public List<PrayerAttachment> attachments { get; set; }

        public List<PrayerComment> comments { get; set; }

        public List<PrayerAnswered> answers { get; set; }

        public List<PrayerAmen> amen { get; set; }

        public String IfExecutedGUID { get; set; }
    }

    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss'GMT'zzzz";           
        }
    }
}