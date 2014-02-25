using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Acceptance.Tests.Utils
{
    public static class DatabaseHelper
    {
        public static IEnumerable<TEntity> FindAll<TEntity>()
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.CreateCriteria(typeof(TEntity)).List<TEntity>();
            }
        }

        public static void SaveOrUpdate<TEntity>(TEntity entity)
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    trans.Commit();
                }
            }
        }

        public static void Delete<TEntity>(TEntity entity)
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.Delete(entity);
                    trans.Commit();
                }
            }
        }
    }
}
