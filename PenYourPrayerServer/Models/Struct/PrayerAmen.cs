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
        
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedWhen;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TouchedWhen;
    }

    [XmlRootAttribute("AllAmen")]
    public class AllPrayerAmen
    {
        [XmlElement("Amen")]
        public List<PrayerAmen> amen { get; set; }
    }
}