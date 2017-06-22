using FluentNHibernate.Mapping;
using Microsoft.AspNet.Identity;
using System;

namespace application.Models
{
    public class Users: IUser<int>
    {
        public virtual int Id { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual Groups Group { get; set; }

        public virtual string Address { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Name { get; set; }

        public virtual string UserName { get { return Login; } set { Login = value; } }
    }

    public class UsersMap: ClassMap<Users>
    {
        public UsersMap()
        {
            Id(x => x.Id);
            Map(x => x.Login);
            Map(x => x.Password);
            Map(x => x.Address);
            Map(x => x.Phone);
            Map(x => x.Name);
            References(x => x.Group, "[group]").Cascade.SaveUpdate();
        }
    }
}