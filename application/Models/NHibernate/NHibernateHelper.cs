using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace application.Models.NHibernate
{
    public class NHibernateHelper
    {
        private ISessionFactory sessionFactory;

        public NHibernateHelper()
        {
            sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql7.ConnectionString(@"Server=.\MSSQL;DataBase=C:\LIBRARY\APPLICATION\APP_DATA\LIBRARY.MDF; Integrated Security=SSPI;")
                .ShowSql()
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Users>())
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            .BuildSessionFactory();
        }
        public ISession OpenSession()
        {
            return sessionFactory.OpenSession();
        }

        public IUserStore<Users, int> Users
        {
            get { return new IdentityStore(OpenSession()); }
        }
    }
}