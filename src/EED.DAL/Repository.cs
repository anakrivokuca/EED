using System.Collections.Generic;
using NHibernate;

namespace EED.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private ISession _session;
        private ITransaction _transaction;

        public IEnumerable<TEntity> FindAll()
        {
            using (_session = SessionFactory.OpenSession())
            {
                return _session.CreateCriteria(typeof (TEntity)).List<TEntity>();
            }
        }


        public void Save(TEntity entity)
        {
            using (_session = SessionFactory.OpenSession())
            {
                using (_transaction = _session.BeginTransaction())
                {
                    _session.SaveOrUpdate(entity);
                    _transaction.Commit();
                }
            }
        }
    }
}
