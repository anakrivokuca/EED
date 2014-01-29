using System.Collections.Generic;

namespace EED.DAL
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IEnumerable<TEntity> FindAll();
        void Save(TEntity entity);
        void Delete(TEntity entity);
    }
}
