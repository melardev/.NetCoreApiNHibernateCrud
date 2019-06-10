using System;
using System.Collections.Generic;
using ApiCoreNHibernateCrud.Entities;
using ApiCoreNHibernateCrud.Infrastructure.Extensions;
using FluentNHibernate.Cfg;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace ApiCoreNHibernateCrud.Data
{
    public class FluentSessionFactoryBuilder : ISessionFactoryBuilder
    {
        private readonly IConfiguration _configuration;
        private volatile object _sessionCreated = false;
        private ISessionFactory _sessionFactory;

        public FluentSessionFactoryBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
            // You can use any, they all work
            // _connectionString = configuration.GetSection("DataSources:MySQL:ConnectionString").Value;
            // IConfigurationSection configSection = configuration.GetSection("DataSources:MySQL");
            // string user = configSection.GetValue<string>("User");
        }


        public ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory != null) return _sessionFactory;
            lock (_sessionCreated)
            {
                if (_sessionFactory == null)
                {
                    var mappings = new List<Type>();
                    mappings.Add(typeof(Todo));

                    var fluentConfiguration = Fluently.Configure()
                        // .Database(SQLiteConfiguration.Standard.ConnectionString(this._connectionString).ShowSql())
                        .AddSqLite(_configuration)
                        // To add Many mappings from a List<Type>
                        // .Mappings(m => mappings.ForEach(e => { m.FluentMappings.Add(e); }))
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernate.Cfg.Mappings>())
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Mappings.TodoMap>())
                        .CurrentSessionContext("call")
                        .ExposeConfiguration(cfg => BuildSchema(cfg, true, true));

                    _sessionFactory = fluentConfiguration.BuildSessionFactory();
                }
            }

            return _sessionFactory;
        }

        /// <summary>
        ///     Build the schema of the database.
        /// </summary>
        /// <param name="config">Configuration.</param>
        private static void BuildSchema(Configuration config, bool create = false, bool update = false)
        {
            if (create)
                new SchemaExport(config).Create(false, true);
            else
                new SchemaUpdate(config).Execute(false, update);
        }
    }
}