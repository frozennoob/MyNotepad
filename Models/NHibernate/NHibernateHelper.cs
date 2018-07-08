using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Configuration;

namespace MyNotepad
{
    public class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure().Database(
                PostgreSQLConfiguration.PostgreSQL82.ConnectionString(
                cs => cs.Host("localhost").
                Port(5432).
                Database("notes_db").
                Username("postgres"))
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>())
            .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
