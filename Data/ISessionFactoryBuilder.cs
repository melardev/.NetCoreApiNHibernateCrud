using NHibernate;

namespace ApiCoreNHibernateCrud.Data
{
    public interface ISessionFactoryBuilder
    {
        ISessionFactory GetSessionFactory();
    }
}