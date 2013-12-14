using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EED.Domain;

namespace EED.Service
{
    public interface IUserService
    {
        IEnumerable<User> FindAllUsers();
    }
}
