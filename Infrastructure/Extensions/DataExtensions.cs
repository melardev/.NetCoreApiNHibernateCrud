using ApiCoreNHibernateCrud.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Dialect;

namespace ApiCoreNHibernateCrud.Infrastructure.Extensions
{
    public static class DataExtensions
    {
        public static void AddDb(this IServiceCollection services, IConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
        {
            // They both work, use one of them, I always prefer the FluentApi
            // services.AddSingleton<ISessionFactoryBuilder>(new XmlSessionFactoryBuilder(hostingEnvironment));
            services.AddSingleton<ISessionFactoryBuilder>(new FluentSessionFactoryBuilder(configuration));
        }


        public static FluentConfiguration AddMySql(this FluentConfiguration fluentConfiguration,
            IConfiguration configuration)
        {
            var dbConfig = configuration.GetSection("DataSources:MySQL");
            fluentConfiguration.Database(MySQLConfiguration.Standard.ConnectionString(csBuilder =>
                {
                    csBuilder.Server(dbConfig.GetSection("Host").Value)
                        .Database(dbConfig.GetSection("DbName").Value)
                        .Username(dbConfig.GetSection("User").Value)
                        .Password(dbConfig.GetSection("Password").Value);
                })
                .ShowSql()
                .Dialect<MySQL5Dialect>());
            return fluentConfiguration;
        }

        public static FluentConfiguration AddSqLite(this FluentConfiguration fluentConfiguration,
            IConfiguration configuration)
        {
            // fluentConfiguration.Database(SQLiteConfiguration.Standard.ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString));
            fluentConfiguration.Database(SQLiteConfiguration.Standard
                .ConnectionString(configuration.GetSection("DataSources:SQLite:ConnectionString").Value)
                .ShowSql().Dialect<SQLiteDialect>());

            return fluentConfiguration;
        }
    }
}