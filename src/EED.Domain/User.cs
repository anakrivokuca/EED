using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace EED.Domain
{
    public class User : MembershipUser
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        //public virtual string Email { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string PhoneNumber { get; set; }
        public new virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public new virtual DateTime? LastLoginDate { get; set; }
        public new virtual DateTime? CreationDate { get; set; }
        public new virtual bool IsOnline { get; set; }
        public new virtual bool IsLockedOut { get; set; }
    }
}
