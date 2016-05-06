using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PenYourPrayerServer.Models.Struct
{
    public class PrayerAmen
    {
        public string AmenID { get; set; }
        public string UserID { get; set; }
        public string WhoID { get; set; }
        public string WhoName { get; set; }
        public string WhoProfilePicture { get; set; }
        public long CreatedWhen;
        public long TouchedWhen;
    }

    [XmlRootAttribute("AllAmen")]
    public class AllPrayerAmen
    {
        [XmlElement("Amen")]
        public List<PrayerAmen> amen { get; set; }
    }
}