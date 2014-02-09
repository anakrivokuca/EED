using EED.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.District_Type
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        public string Abbreviation { get; set; }
        
        [DisplayName("Parent District Type")]
        [Required(ErrorMessage = "Parent district type must be selected.")]
        public int ParentDistrictTypeId { get; set; }

        public IEnumerable<SelectListItem> DistrictTypes { get; set; }

        public DistrictType ConvertModelToDistrictType(CreateViewModel model)
        {
            var districtType = new DistrictType()
            {
                Id = model.Id,
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                ParentDistrictType = new DistrictType { Id = model.ParentDistrictTypeId }
            };

            return districtType;
        }

        public CreateViewModel ConvertDistrictTypeToModel(DistrictType districtType)
        {
            var model = new CreateViewModel()
            {
                Id = districtType.Id,
                Name = districtType.Name,
                Abbreviation = districtType.Abbreviation,
                ParentDistrictTypeId = districtType.ParentDistrictType.Id
            };

            return model;
        }
    }
}