using FluentNHibernate.Mapping;
using System;

namespace application.Models
{
    public class QueueForBooks
    {
        public virtual int Id { get; set; }
        public virtual Books Book { get; set; }
        public virtual Users User { get; set; }
        public virtual int Booking_time { get; set; }
        public virtual bool Relevance { get; set; }

        public QueueForBooks()
        {
            Relevance = true;
            Booking_time = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public virtual string GetDate
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(Booking_time).ToLocalTime().ToString();
            }
        }
    }

    public class QueueForBooksMap : ClassMap<QueueForBooks>
    {
        public QueueForBooksMap()
        {
            Id(x => x.Id);
            Map(x => x.Booking_time);
            Map(x => x.Relevance).Column("[relevance]");
            References(x => x.User).Cascade.SaveUpdate();
            References(x => x.Book).Cascade.SaveUpdate();
        }
    }
}