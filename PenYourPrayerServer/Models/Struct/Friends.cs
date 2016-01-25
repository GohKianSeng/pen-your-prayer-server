using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PenYourPrayerServer.Models;

namespace PenYourPrayerServer.Models.Struct
{
    public class Friends
    {
        public long UserID { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureURL { get; set; }        
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public static explicit operator Friends(usp_GetLatestFriendsResult c)
        {
            Friends fr = new Friends();
            fr.UserID = c.ID;
            fr.DisplayName = c.DisplayName;
            fr.ProfilePictureURL = c.ProfilePictureURL;
            return fr;
        }
    }

    [XmlRootAttribute("AllTagFriends")]
    public class AllFriends
    {
        [XmlElement("TagFriend")]
        public List<Friends> friends { get; set; }
    }

    
}