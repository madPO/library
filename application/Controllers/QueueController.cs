using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.Linq;
using application.Models.NHibernate;
using application.Models;
using System.Collections.Generic;
using System.Linq;

namespace application.Controllers
{
    [Authorize]
    public class QueueController : Controller
    {
        public ActionResult Add(int id)
        {
            using(ISession session = new NHibernateHelper().OpenSession())
            {
                QueueForBooks model = new QueueForBooks();
                model.Book = session.QueryOver<Books>().Where(b => b.Id == id).SingleOrDefault();
                model.User = session.QueryOver<Users>().List()
                    .Where(u => u.Id.ToString() == User.Identity.GetUserId())
                    .SingleOrDefault();
                session.Save(model);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        public ActionResult Index()
        {
            using(ISession session = new NHibernateHelper().OpenSession())
            {
                Books book = null;
                Users user =  session.QueryOver<Users>().List().Where(u => u.Id.ToString() == User.Identity.GetUserId()).SingleOrDefault();
                IEnumerable<QueueForBooks> books = session.QueryOver<QueueForBooks>()
                    .Where(b => b.User == user && b.Relevance)
                    .JoinAlias(x => x.Book, () => book)
                    .List();
                return View(books);
            }
        }

        public ActionResult Delete(int id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Books book = session.QueryOver<Books>().Where(b => b.Id == id).SingleOrDefault();
                Users user = session.QueryOver<Users>().List().Where(u => u.Id.ToString() == User.Identity.GetUserId()).SingleOrDefault();
                QueueForBooks model = session.QueryOver<QueueForBooks>()
                    .Where(b => b.User == user && b.Book == book && b.Relevance)
                    .SingleOrDefault();
                model.Relevance = false;
                session.Update(model);
                session.Flush();
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}