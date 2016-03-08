using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace PenYourPrayerServer.Models.Struct
{
    public class PrayerRequestAttachment
    {
        public long AttachmentID { get; set; }
        public long PrayerRequestID { get; set; }
        public string GUID { get; set; }
        public string OriginalFilePath { get; set; }
        public string URLPath { get; set; }
        public string UserID { get; set; }
        public string FileName { get; set; }
        
    }




    [XmlRootAttribute("AllAttachments")]
    public class AllPrayerRequestAttachment
    {
        [XmlElement("Attachment")]
        public List<PrayerRequestAttachment> attachments { get; set; }
    }
}