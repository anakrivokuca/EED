using EED.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Precincts
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [DisplayName("Number Of Eligible Voters")]
        [Range(0, Int32.MaxValue)]
        public int NumberOfEligibleVoters { get; set; }

        [DisplayName("Available Districts")]
        public int[] DistrictIds { get; set; }

        public List<Domain.District> Districts { get; set; }

        [DisplayName("Selected Districts")]
        public int[] SelectedDistrictIds { get; set; }

        public List<Domain.District> SelectedDistricts { get; set; }

        public string SavedSelected { get; set; }

        public Precinct ConvertModelToPrecinct(CreateViewModel model)
        {
            var precinct = new Precinct()
            {
                Id = model.Id,
                Name = model.Name,
                NumberOfEligibleVoters = model.NumberOfEligibleVoters
            };

            return precinct;
        }

        public CreateViewModel ConvertPrecinctToModel(Precinct precinct)
        {
            var model = new CreateViewModel()
            {
                Id = precinct.Id,
                Name = precinct.Name,
                NumberOfEligibleVoters = precinct.NumberOfEligibleVoters
            };

            return model;
        }
    }
}