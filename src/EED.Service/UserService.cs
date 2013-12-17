using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EED.DAL;
using EED.Domain;

namespace EED.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> FindAllUsers()
        {
            return _repository.FindAll();
        }
    }
}
