using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace EED.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private ISession _session;

        public IEnumerable<TEntity> FindAll()
        {
            using (_session = SessionFactory.OpenSession())
            {
                return _session.CreateCriteria(typeof (TEntity)).List<TEntity>();
            }
        }
    }
}
