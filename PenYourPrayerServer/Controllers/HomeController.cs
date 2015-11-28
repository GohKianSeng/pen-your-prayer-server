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
            //CommonMethod.sendAccountActiviationEmail("mail@pyptesting.com", "sdfdfdsfsdf", "test", "123123");
            return View();
        }
    }
}