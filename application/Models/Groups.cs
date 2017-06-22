using FluentNHibernate.Mapping;
using System.Collections.Generic;

namespace application.Models
{
    public class Groups
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Users> Users { get; set; }

        public Groups()
        {
            Users = new List<Users>();
        }
    }

    public class GroupsMap: ClassMap<Groups>
    {
        public GroupsMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            HasMany(x => x.Users)
            .Inverse();
        }
    }
}