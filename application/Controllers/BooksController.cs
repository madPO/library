using NHibernate;
using System.Web.Mvc;
using application.Models.NHibernate;
using application.Models;
using System.Collections.Generic;

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
                        session.SaveOrUpdate(model);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Books/Delete/5
        public string Delete(int id)
        {
            return "Без колекшена";
        }

        // POST: Books/Delete/5
        [HttpPost]
        public string Delete(int id, FormCollection collection)
        {
            return "С колекшеном";
        }
    }
}
