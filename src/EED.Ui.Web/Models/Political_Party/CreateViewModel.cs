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

        public System.Web.HttpPostedFileBase Image { get; set; }

        public bool HasImage { get; set; }

        public PoliticalParty ConvertModelToPoliticalParty(CreateViewModel model)
        {
            var politicalParty = new PoliticalParty()
            {
                Id = model.Id,
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                //Image = model.Image
            };

            if (model.Image != null)
            {
                var target = new System.IO.MemoryStream();
                model.Image.InputStream.CopyTo(target);
                politicalParty.Image = target.ToArray();
            }

            return politicalParty;
        }

        public CreateViewModel ConvertPoliticalPartyToModel(PoliticalParty politicalParty)
        {
            var model = new CreateViewModel()
            {
                Id = politicalParty.Id,
                Name = politicalParty.Name,
                Abbreviation = politicalParty.Abbreviation,
            };

            if (politicalParty.Image != null)
            {
                model.HasImage = true;
            }

            return model;
        }
    }
}