using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using application.Models.NHibernate;
using NHibernate;
using application.Models;
using System.Linq;

namespace application.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (ISession session = new NHibernateHelper().OpenSession())
                {
                    Users user = session.QueryOver<Users>().List()
                        .Where(u => u.Id.ToString() == User.Identity.GetUserId())
                        .SingleOrDefault();
                    if(user.Group.Id == 1)
                    {
                        return RedirectToAction("Received", "Books", new { id = user.Id });
                    }
                    else
                    {
                        return RedirectToAction("Books", "Search");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login","Account");
            } 
        }
    }
}