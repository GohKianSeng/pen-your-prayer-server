// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNet.Identity.EntityFramework.IdentityUser`4
// Assembly: Microsoft.AspNet.Identity.EntityFramework, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 253142B1-7A82-4B45-A7FE-44CE9E771591
// Assembly location: D:\LocalAccountsApp-master - Copy\packages\Microsoft.AspNet.Identity.EntityFramework.2.1.0\lib\net45\Microsoft.AspNet.Identity.EntityFramework.dll

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace PenYourPrayer.Models.UserAccount
{
    public class CustomUserAccount : IUser
    {
        public string UserName { get; set; }
        public string Id { get; set; }

        public string LoginType { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string ProfilePictureURL { get; set; }
        public string MobilePlatform { get; set; }
        public string PushNotificationID { get; set; }
        public DateTime CreatedWhen { get; set; }
        public DateTime TouchedWhen { get; set; }
    }
}
