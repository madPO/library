
using FluentNHibernate.Mapping;
using System;

namespace application.Models
{
    public class IssuedBooks
    {
        public virtual int Id { get; set; }
        public virtual Books Book { get; set; }
        public virtual Users User { get; set; }
        public virtual int Begin_time { get; set; }
        public virtual int End_time { get; set; }
        public virtual bool Relevance { get; set; }

        public IssuedBooks()
        {
            Relevance = true;
            Begin_time = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            //Magic number!
            End_time = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + 2592000;
        }

        public virtual string GetEndTime
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(End_time).ToLocalTime().ToString();
            }
        }

        public virtual string GetBeginTime
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(Begin_time).ToLocalTime().ToString();
            }
        }
    }

    public class IssuedBooksMap : ClassMap<IssuedBooks>
    {
        public IssuedBooksMap()
        {
            Id(x => x.Id);
            Map(x => x.Begin_time);
            Map(x => x.End_time);
            Map(x => x.Relevance).Column("[relevance]");
            References(x => x.User).Cascade.SaveUpdate();
            References(x => x.Book).Cascade.SaveUpdate();
        }
    }
}