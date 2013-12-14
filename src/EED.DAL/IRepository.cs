using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.DAL
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IEnumerable<TEntity> FindAll();
    }
}
