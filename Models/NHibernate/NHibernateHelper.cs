using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace MyNotepad
{
    // Настраиваем подключение к БД средствами FluenHibernate 
    public class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            // конфигигурируем и возвращаем
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
