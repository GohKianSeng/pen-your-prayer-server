using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PenYourPrayerServer.Models.Struct
{
    public class CustomResponseMessage
    {
        public int StatusCode;
        public string MessageType;
        public string Description;
    }
}