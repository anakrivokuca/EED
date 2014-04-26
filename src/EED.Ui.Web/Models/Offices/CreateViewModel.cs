using EED.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Offices
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [DisplayName("Office Type")]
        [Required(ErrorMessage = "Office type must be selected.")]
        public int OfficeTypeId { get; set; }

        public IEnumerable<SelectListItem> OfficeTypes { get; set; }

        [DisplayName("Number Of Positions")]
        [Required(ErrorMessage = "Please enter number of positions.")]
        [Range(1, Int32.MaxValue)]
        public int NumberOfPositions { get; set; }

        [DisplayName("District Type")]
        [Required(ErrorMessage = "District type must be selected.")]
        public int DistrictTypeId { get; set; }

        public IEnumerable<SelectListItem> DistrictTypes { get; set; }

        public Office ConvertModelToOffice(CreateViewModel model)
        {
            var office = new Office()
            {
                Id = model.Id,
                Name = model.Name,
                NumberOfPositions = model.NumberOfPositions,
                OfficeType = (OfficeType)model.OfficeTypeId,
                DistrictType = new DistrictType { Id = model.DistrictTypeId }
            };

            return office;
        }

        public CreateViewModel ConvertOfficeToModel(Office office)
        {
            var model = new CreateViewModel()
            {
                Id = office.Id,
                Name = office.Name,
                NumberOfPositions = office.NumberOfPositions,
                OfficeTypeId = (int)office.OfficeType,
                DistrictTypeId = office.DistrictType.Id
            };

            return model;
        }
    }
}