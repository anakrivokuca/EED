using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EED.Domain
{
    public class User
    {
        [HiddenInput(DisplayValue = false)]
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Email { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        [DisplayName("Phone Number")]
        public virtual string PhoneNumber { get; set; }
        public virtual string Username { get; set; }
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
    }
}
