
using System.Collections.Generic;
using System.Web.Mvc;

namespace application.Models
{
    public class SearchViewBooks
    {
        public string Text { get; set; }
        public string Type_id { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<Books> Result { get; set; }

        public SearchViewBooks()
        {
            Types = new List<SelectListItem>(){
                new SelectListItem(){Value = "0", Text = "По автору"},
                new SelectListItem(){Value = "1", Text = "По названию"},
                new SelectListItem(){Value = "2", Text = "По издательству"}
            };
            Result = new List<Books>();
            Type_id = "0";
        }
    }
}