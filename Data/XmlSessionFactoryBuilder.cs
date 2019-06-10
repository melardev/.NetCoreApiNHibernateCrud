using System.IO;
using Microsoft.AspNetCore.Hosting;
using NHibernate;
using NHibernate.Cfg;

namespace ApiCoreNHibernateCrud.Data
{
    public class XmlSessionFactoryBuilder : ISessionFactoryBuilder
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private volatile object _sessionCreated = false;
        private ISessionFactory _sessionFactory;

        public XmlSessionFactoryBuilder(IHostingEnvironment hostingEnvironmentEnvironment)
        {
            _hostingEnvironment = hostingEnvironmentEnvironment;
        }

        public ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory != null) return _sessionFactory;
            lock (_sessionCreated)
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    // configuration.SetProperty("hbm2ddl.auto", "create");
                    var configurationPath = Path.Combine(_hostingEnvironment.ContentRootPath, "hibernate.cfg.xml");
                    configuration.Configure(configurationPath);


                    configuration.DataBaseIntegration(x =>
                    {
                        x.LogFormattedSql = true;
                        x.LogSqlInConsole = true;
                    });

                    var todoMappingFile =
                        Path.Combine(_hostingEnvironment.ContentRootPath, "Entities/Mappings/Todo.hbm.xml");
                    configuration.AddFile(todoMappingFile);
                    _sessionFactory = configuration.BuildSessionFactory();
                }
            }

            return _sessionFactory;
        }
    }
}