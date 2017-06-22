
using FluentNHibernate.Mapping;

namespace application.Models
{
    public class Books
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Author { get; set; }
        public virtual string Publisher { get; set; }
        public virtual string Date { get; set; }
        public virtual string Description { get; set; }
        public virtual int Count { get; set; }
    }

    public class BooksMap : ClassMap<Books>
    {
        public BooksMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Author);
            Map(x => x.Publisher);
            Map(x => x.Date);
            Map(x => x.Description);
            Map(x => x.Count);
        }
    }
}