using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace PenYourPrayerServer.Models.Struct
{
    public class PrayerComment
    {

        public string CommentID { get; set; }
        public string OwnerPrayerID { get; set; }
        public string WhoID { get; set; }
        public string WhoName { get; set; }
        public string WhoProfilePicture { get; set; }
        public string Comment { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedWhen;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TouchedWhen;
    }

    [XmlRootAttribute("AllComments")]
    public class AllPrayerComments
    {
        [XmlElement("Comment")]
        public List<PrayerComment> comments { get; set; }
    }
}