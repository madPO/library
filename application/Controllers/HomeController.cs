using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace application.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //return "Hello, Workd!";
            return View();
        }

        public String Test()
        {
            return User.Identity.GetUserId();
        }
    }
}