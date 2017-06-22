using application.Models;
using application.Models.NHibernate;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NHibernate;
using NHibernate.SqlCommand;
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

        [HttpGet]
        public ActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Groups groupAl = null;
                var users = session.QueryOver<Users>()
                    .JoinAlias(p => p.Group, () => groupAl, JoinType.LeftOuterJoin)
                    .List();
                return View(users);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Groups groupAl = null;
                Users user = session.QueryOver<Users>().Where(x => x.Id == id)
                    .JoinAlias(p => p.Group, () => groupAl, JoinType.LeftOuterJoin)
                    .SingleOrDefault();
                RegisterViewModel model = new RegisterViewModel() { Address = user.Address, Group = user.Group, group_id = user.Group.Id.ToString(), Name = user.Name, Phone = user.Phone, UserName = user.Login};
                model.Groups = session.QueryOver<Groups>().List().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, RegisterViewModel model)
        {
            try
            {
                using (ISession session = new NHibernateHelper().OpenSession())
                {
                    Groups groupAl = null;
                    Users user = session.QueryOver<Users>().Where(x => x.Id == id)
                                        .JoinAlias(p => p.Group, () => groupAl, JoinType.LeftOuterJoin)
                                        .SingleOrDefault();
                    user.Address = model.Address;
                    user.Name = model.Name;
                    user.Phone = model.Phone;
                    user.Group = session.QueryOver<Groups>().List().Where(x => x != null && x.Id.ToString() == model.group_id).SingleOrDefault();
                    session.Update(user);
                    session.Flush();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Users user = session.QueryOver<Users>().Where(x => x.Id == id).SingleOrDefault();
                session.Delete(user);
                session.Flush();
            }
            return RedirectToAction("Index");
        }

        public SignInManager SignInManager => HttpContext.GetOwinContext().Get<SignInManager>();
        public UserManager UserManager => HttpContext.GetOwinContext().GetUserManager<UserManager>();
    }
}