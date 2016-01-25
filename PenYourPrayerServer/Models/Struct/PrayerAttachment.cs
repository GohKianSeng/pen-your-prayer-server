using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace PenYourPrayerServer.Models.Struct
{
    public class PrayerAttachment
    {
        public long AttachmentID { get; set; }
        public long PrayerID { get; set; }
        public string GUID { get; set; }
        public string OriginalFilePath { get; set; }
        public string URLPath { get; set; }
        public string UserID { get; set; }
        public string FileName { get; set; }
        
    }

    [XmlRootAttribute("AllAttachments")]
    public class AllPrayerAttachment
    {
        [XmlElement("Attachment")]
        public List<PrayerAttachment> attachments { get; set; }
    }
}