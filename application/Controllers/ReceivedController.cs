using application.Models;
using application.Models.NHibernate;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace application.Controllers
{
    public class ReceivedController : Controller
    {
        [HttpGet]
        public ActionResult Index(int id)
        {
            using(ISession session = new NHibernateHelper().OpenSession())
            {
                ReceivedViewModel model = new ReceivedViewModel()
                {
                    Book = session.QueryOver<Books>().Where(b => b.Id == id).SingleOrDefault()
                };
                int c = session.QueryOver<IssuedBooks>().Where(i => i.Book == model.Book && i.Relevance).List().Count;
                model.Count = model.Book.Count - c;
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Index(int id, ReceivedViewModel model)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                model.Book = session.QueryOver<Books>().Where(b => b.Id == id).SingleOrDefault();
                model.Users = session.QueryOver<Users>().Where(u => u.Name.IsLike(model.Name, MatchMode.Anywhere)).List();
                int c = session.QueryOver<IssuedBooks>().Where(i => i.Book == model.Book && i.Relevance).List().Count;
                model.Count = model.Book.Count - c;
                return View(model);
            }
        }

        public ActionResult Add(int user_id, int books_id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
               Users user = session.QueryOver<Users>().List().Where(u => u.Id == user_id).SingleOrDefault();
                Books book = session.QueryOver<Books>().Where(b => b.Id == books_id).SingleOrDefault();
                int books = session.QueryOver<IssuedBooks>().Where(i => i.Book == book && i.Relevance).RowCount();
                QueueForBooks queue = session.Query<QueueForBooks>()
                    .Where(q => q.Book == book && q.User == user && q.Relevance).FirstOrDefault();
                int records = session.QueryOver<QueueForBooks>().List()
                    .Where(q => ( q.Relevance && q != queue && q.Book == book && (queue != null ? q.Booking_time < queue.Booking_time : true)))
                    .Count();
                if(book.Count - books - records - 1 < 0)
                {
                    ModelState.AddModelError("", "Невозможно выдать книгу, так как они все забронированны или выданы другим пользователям.");
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    IssuedBooks model = new IssuedBooks()
                                    {
                                        User = session.QueryOver<Users>().List().Where(u => u.Id == user_id).SingleOrDefault(),
                                        Book = session.QueryOver<Books>().Where(b => b.Id == books_id).SingleOrDefault()
                                    };
                    session.Save(model);
                    queue.Relevance = false;
                    session.Update(queue);
                    session.Flush();
                    return RedirectToAction("Books","Search");
                }              
            }
        }

        public ActionResult List()
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Users user = session.QueryOver<Users>()
                    .List()
                    .Where(u => u.Id.ToString() == User.Identity.GetUserId())
                    .SingleOrDefault();
                IEnumerable<IssuedBooks> books = session.QueryOver<IssuedBooks>()
                    .Where(b => b.User == user && b.Relevance)
                    .JoinQueryOver(b => b.Book)
                    .List();
                return View(books);
            }
        }
    }
}