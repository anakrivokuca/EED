using EED.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EED.Ui.Web.Models
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter surname.")]
        public string Surname { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter email.")]
        public string Email { get; set; }

        public string State { get; set; }

        [Required(ErrorMessage = "Please enter country.")]
        public string Country { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "Please enter username.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }

        [DisplayName("Is Approved")]
        public bool IsApproved { get; set; }

        
        public User ConvertModelToUser(CreateViewModel model)
        {
            var user = new User()
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                State = model.State,
                Country = model.Country,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                Password = model.Password,
                IsApproved = model.IsApproved
            };

            return user;
        }

        public CreateViewModel ConvertUserToModel(User user)
        {
            var model = new CreateViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                State = user.State,
                Country = user.Country,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Password = user.Password,
                IsApproved = user.IsApproved
            };

            return model;
        }
    }
}