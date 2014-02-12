using EED.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.District
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        [DisplayName("District Type")]
        [Required(ErrorMessage = "District type must be selected.")]
        public int DistrictTypeId { get; set; }
        
        public IEnumerable<SelectListItem> DistrictTypes { get; set; }

        [DisplayName("Parent District")]
        [Required(ErrorMessage = "Parent district must be selected.")]
        public int ParentDistrictId { get; set; }

        public IEnumerable<SelectListItem> ParentDistricts { get; set; }

        public Domain.District ConvertModelToDistrict(CreateViewModel model)
        {
            var district = new Domain.District()
            {
                Id = model.Id,
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                DistrictType = new DistrictType { Id = model.DistrictTypeId },
                ParentDistrict = new Domain.District { Id = model.ParentDistrictId }
            };

            return district;
        }

        public CreateViewModel ConvertDistrictToModel(Domain.District district)
        {
            var model = new CreateViewModel()
            {
                Id = district.Id,
                Name = district.Name,
                Abbreviation = district.Abbreviation,
                DistrictTypeId = district.DistrictType.Id
            };

            if (district.ParentDistrict != null)
                model.ParentDistrictId = district.ParentDistrict.Id;

            return model;
        }
    }
}