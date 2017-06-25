using application.Models;
using application.Models.NHibernate;
using Microsoft.AspNet.Identity;
using NHibernate;
using System.Linq;
using System.Web.Mvc;


namespace application.Filters
{
    public class LibrarianActions: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using(ISession session = new NHibernateHelper().OpenSession())
            {
                Users user = session.QueryOver<Users>().List()
                    .Where(u => u.Id.ToString() == filterContext.HttpContext.User.Identity.GetUserId())
                    .SingleOrDefault();
                if (user.Group.Id != 2)
                    filterContext.Result = new HttpStatusCodeResult(403);
            }
        }
    }
}