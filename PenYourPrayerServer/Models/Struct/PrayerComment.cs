using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PenYourPrayerServer.Supporting.HMAC;
using System;


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

}