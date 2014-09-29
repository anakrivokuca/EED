using EED.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Choices
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [DisplayName("Contest")]
        [Required(ErrorMessage = "Contest must be selected.")]
        public int ContestId { get; set; }

        public IEnumerable<SelectListItem> Contests { get; set; }

        [DisplayName("Available Political Parties")]
        public int[] PoliticalPartyIds { get; set; }

        public List<PoliticalParty> PoliticalParties { get; set; }

        [DisplayName("Selected Political Parties")]
        public int[] SelectedPoliticalPartyIds { get; set; }

        public List<PoliticalParty> SelectedPoliticalParties { get; set; }

        public string SavedSelected { get; set; }

        public Choice ConvertModelToChoice(CreateViewModel model)
        {
            var contest = new Choice()
            {
                Id = model.Id,
                Name = model.Name,
                Contest = new Contest { Id = model.ContestId }
            };

            return contest;
        }

        public CreateViewModel ConvertChoiceToModel(Choice choice)
        {
            var model = new CreateViewModel()
            {
                Id = choice.Id,
                Name = choice.Name,
                ContestId = choice.Contest.Id
            };

            return model;
        }
    }
}