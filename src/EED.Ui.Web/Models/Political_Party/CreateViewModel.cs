using EED.Domain;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Political_Party
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public string Image { get; set; }

        public PoliticalParty ConvertModelToPoliticalParty(CreateViewModel model)
        {
            var politicalParty = new PoliticalParty()
            {
                Id = model.Id,
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                Image = model.Image
            };

            return politicalParty;
        }

        public CreateViewModel ConvertPoliticalPartyToModel(PoliticalParty politicalParty)
        {
            var model = new CreateViewModel()
            {
                Id = politicalParty.Id,
                Name = politicalParty.Name,
                Abbreviation = politicalParty.Abbreviation,
                Image = politicalParty.Image
            };

            return model;
        }
    }
}