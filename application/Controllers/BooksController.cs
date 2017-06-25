using NHibernate;
using System.Web.Mvc;
using application.Models.NHibernate;
using application.Models;
using System.Collections.Generic;
using System.Linq;

namespace application.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        public ActionResult Index()
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                IEnumerable<Books> books = session.QueryOver<Books>().List();
                return View(books);
            }
        }

        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Books book = session.QueryOver<Books>().Where(x => x.Id == id).SingleOrDefault();
                return View(book);
            }
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        public ActionResult Create(BooksViewCreate model)
        {
            if (ModelState.IsValid)
            {
                if (model.Author == "" || model.Date == "" || model.Name == "" || model.Publisher == "")
                    ModelState.AddModelError("", "Все поля, кроме описания должны быть заполнены");
                else if (model.Count < 0)
                    ModelState.AddModelError("", "Количество книг не должно быть отрицательным");
                else
                {
                    using (ISession session = new NHibernateHelper().OpenSession())
                    {
                        Books book = new Books { Author = model.Author, Count = model.Count, Date = model.Date, Description = model.Description, Name = model.Name, Publisher = model.Publisher };
                        session.SaveOrUpdate(book);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Books book = session.QueryOver<Books>().Where(x => x.Id == id).SingleOrDefault();
                return View(book);
            }
        }

        // POST: Books/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Books model)
        {
            if (ModelState.IsValid)
            {
                if (model.Author == "" || model.Date == "" || model.Name == "" || model.Publisher == "")
                    ModelState.AddModelError("", "Все поля, кроме описания должны быть заполнены");
                else if (model.Count < 0)
                    ModelState.AddModelError("", "Количество книг не должно быть отрицательным");
                else
                {
                    using (ISession session = new NHibernateHelper().OpenSession())
                    {
                        session.Update(model);
                        session.Flush();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Books book = session.QueryOver<Books>().Where(x => x.Id == id).SingleOrDefault();
                session.Delete(book);
                session.Flush();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Received(int id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                Users user = session.QueryOver<Users>()
                    .List()
                    .Where(u => u.Id == id)
                    .SingleOrDefault();
                IEnumerable<IssuedBooks> books = session.QueryOver<IssuedBooks>()
                    .Where(b => b.User == user && b.Relevance)
                    .JoinQueryOver(b => b.Book)
                    .List();
                return View(books);
            }
        }

        public ActionResult Return(int issued_id)
        {
            using (ISession session = new NHibernateHelper().OpenSession())
            {
                IssuedBooks book = session.QueryOver<IssuedBooks>().Where(i => i.Id == issued_id).SingleOrDefault();
                book.Relevance = false;
                session.Update(book);
                session.Flush();
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}
