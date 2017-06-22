using application.Models;
using application.Models.NHibernate;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NHibernate;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace application.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = SignInManager.PasswordSignIn(model.UserName, model.Password, false, false);
                if (result == SignInStatus.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            ISession session = new NHibernateHelper().OpenSession();
            model.Groups = session.QueryOver<Groups>().List().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            ISession session = new NHibernateHelper().OpenSession();
            model.Group = session.QueryOver<Groups>().List().Where(x =>x != null && x.Id.ToString() == model.group_id).SingleOrDefault();
            if (ModelState.IsValid)
            {
                session.Close();
                var user = new Users() { UserName = model.UserName, Address = model.Address, Name = model.Name, Phone = model.Phone, Group = model.Group };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    SignInManager.SignIn(user, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            session = new NHibernateHelper().OpenSession();
            model.Groups = session.QueryOver<Groups>().List().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            return View(model);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public SignInManager SignInManager => HttpContext.GetOwinContext().Get<SignInManager>();
        public UserManager UserManager => HttpContext.GetOwinContext().GetUserManager<UserManager>();
    }
}