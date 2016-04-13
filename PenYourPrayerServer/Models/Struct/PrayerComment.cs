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
        public long CreatedWhen;
        public long TouchedWhen;
    }

    [XmlRootAttribute("AllComments")]
    public class AllPrayerComments
    {
        [XmlElement("Comment")]
        public List<PrayerComment> comments { get; set; }
    }
}