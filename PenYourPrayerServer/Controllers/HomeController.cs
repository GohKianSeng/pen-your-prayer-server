using PenYourPrayerServer.Models;
using PenYourPrayerServer.Supporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PenYourPrayerServer.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            string date = "empty";
            using (DBDataContext db = new DBDataContext())
            {
                date = "UTC: " + db.usp_aaaaa_mustdeleteTemporaryTesting().ToList().ElementAt(0).Column1.ToString();
            }

            ViewData["utc"] = date;
            return View();
        }
    }
}