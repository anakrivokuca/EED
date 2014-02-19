using NHibernate;
using NHibernate.Cfg;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace EED.DAL
{
    public class SessionFactory
    {
        private static ISessionFactory _sessionFactory;

        public static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
            var config = new Configuration().Configure();
            config.AddAssembly("EED.Domain");

            config.Configure();

            _sessionFactory = config.BuildSessionFactory();
        }

        private static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
                Init();

            return _sessionFactory;
        }

        public static ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
        }
    }
}
