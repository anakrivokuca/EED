using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Domain
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Email { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
    }
}
