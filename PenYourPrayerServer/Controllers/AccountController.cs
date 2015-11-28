using PenYourPrayerServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PenYourPrayerServer.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public string Activation(string ID, string VerificationCode)
        {
            using (DBDataContext db = new DBDataContext())
            {
                string result = "";
                string FullName = "";
                long? tempid = (long?)long.Parse(ID);
                db.usp_ActivateUserAccount(tempid, VerificationCode, ref result, ref FullName);
                return result + "   " + FullName;
            }
            return "";
        }
    }
}