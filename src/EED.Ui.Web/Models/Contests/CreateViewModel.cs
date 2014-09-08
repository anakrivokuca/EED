using EED.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Contests
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [DisplayName("Office")]
        [Required(ErrorMessage = "Office must be selected.")]
        public int OfficeId { get; set; }

        public IEnumerable<SelectListItem> Offices { get; set; }

        [DisplayName("District")]
        [Required(ErrorMessage = "District must be selected.")]
        public int DistrictId { get; set; }

        public IEnumerable<SelectListItem> Districts { get; set; }

        [DisplayName("Number Of Positions")]
        public int NumberOfPositions { get; set; }

        public Contest ConvertModelToContest(CreateViewModel model)
        {
            var contest = new Contest()
            {
                Id = model.Id,
                Name = model.Name,
                NumberOfPositions = model.NumberOfPositions,
                Office = new Office { Id = model.OfficeId },
                District = new District { Id = model.DistrictId }
            };

            return contest;
        }

        public CreateViewModel ConvertContestToModel(Contest contest)
        {
            var model = new CreateViewModel()
            {
                Id = contest.Id,
                Name = contest.Name,
                NumberOfPositions = contest.NumberOfPositions,
                OfficeId = contest.Office.Id,
                DistrictId = contest.District.Id,
            };

            return model;
        }
    }
}