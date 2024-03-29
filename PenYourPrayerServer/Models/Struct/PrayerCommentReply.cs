﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace PenYourPrayerServer.Models.Struct
{
    public class PrayerCommentReply
    {

        public string CommentReplyID { get; set; }
        public string MainCommentID { get; set; }
        public string OwnerPrayerID { get; set; }
        public string WhoID { get; set; }
        public string WhoName { get; set; }
        public string WhoProfilePicture { get; set; }
        public string CommentReply { get; set; }
        public long CreatedWhen;
        public long TouchedWhen;
    }

    [XmlRootAttribute("AllCommentReply")]
    public class AllPrayerCommentReply
    {
        [XmlElement("CommentReply")]
        public List<PrayerCommentReply> commentreply { get; set; }
    }
}