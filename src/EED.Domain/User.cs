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

        [Required(ErrorMessage = "Please enter name.")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Please enter surname.")]
        public virtual string Surname { get; set; }

        public virtual string Email { get; set; }

        public virtual string State { get; set; }

        [Required(ErrorMessage = "Please enter country.")]
        public virtual string Country { get; set; }

        [DisplayName("Phone Number")]
        public virtual string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter username.")]
        public virtual string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter password.")]
        public virtual string Password { get; set; }
    }
}
