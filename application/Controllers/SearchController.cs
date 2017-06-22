using application.Models;
using application.Models.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System.Web.Mvc;

namespace application.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public ActionResult Books()
        {
            return View(new SearchViewBooks());
        }

        [HttpPost]
        public ActionResult Books(SearchViewBooks model)
        {
            using(ISession session = new NHibernateHelper().OpenSession())
            {
                switch (model.Type_id)
                {
                    case "0":
                        model.Result = session.QueryOver<Books>().Where(x => x.Author.IsLike(model.Text, MatchMode.Anywhere)).List();
                        break;
                    case "1":
                        model.Result = session.QueryOver<Books>().Where(x => x.Name.IsLike(model.Text, MatchMode.Anywhere)).List();
                        break;
                    case "2":
                        model.Result = session.QueryOver<Books>().Where(x => x.Publisher.IsLike(model.Text, MatchMode.Anywhere)).List();
                        break;
                }
                return View(model);
            }
        }
    }
}