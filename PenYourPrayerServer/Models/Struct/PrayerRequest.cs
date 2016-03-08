using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PenYourPrayerServer.Models.Struct
{
    public class PrayerRequest
    {

        public string PrayerRequestID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool Answered { get; set; }
        public string AnswerComment { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime AnsweredWhen;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedWhen;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TouchedWhen;

        public List<PrayerRequestAttachment> attachments { get; set; }

        public static explicit operator PrayerRequest(usp_GetLatestPrayerRequestResult c)
        {
            PrayerRequest pr = new PrayerRequest();
            
            pr.PrayerRequestID = c.PrayerRequestID.ToString();
            pr.Subject = c.Subject;
            pr.Description = c.Description;
            pr.CreatedWhen = c.CreatedWhen;
            pr.TouchedWhen = c.TouchedWhen;
            pr.AnswerComment = c.AnswerComment;
            pr.Answered = c.Answered;
            if (pr.AnsweredWhen != null && pr.Answered)
            {
                pr.AnsweredWhen = (DateTime)c.AnsweredWhen;
            }
            return pr;
        }
    }

    [XmlRootAttribute("AllPrayerRequest")]
    public class AllPrayerRequest
    {
        [XmlElement("PrayerRequest")]
        public List<PrayerRequest> prayerRequest { get; set; }
    }
}