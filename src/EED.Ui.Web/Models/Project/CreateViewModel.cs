using EED.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Project
{
    public class CreateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select election date.")]
        public DateTime? Date { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Jurisdiction Name")]
        [Required(ErrorMessage = "Please enter jurisdiction name.")]
        public string JurisdictionName { get; set; }

        [DisplayName("Jurisdiction Type")]
        [Required(ErrorMessage = "Jurisdiction type must be selected.")]
        public int JurisdictionTypeId { get; set; }

        public IEnumerable<SelectListItem> JurisdictionTypes { get; set; }

        [DisplayName("Election Type")]
        [Required(ErrorMessage = "Election type must be selected.")]
        public int ElectionTypeId { get; set; }

        public IEnumerable<SelectListItem> ElectionTypes { get; set; }


        public ElectionProject ConvertModelToProject(CreateViewModel model)
        {
            var project = new ElectionProject()
            {
                Id = model.Id,
                Name = model.Name,
                Date = (DateTime)model.Date,
                Description = model.Description,
                JurisdictionName = model.JurisdictionName,
                JurisdictionType = new JurisdictionType { Id = model.JurisdictionTypeId },
                ElectionType = new ElectionType { Id = model.ElectionTypeId }
            };

            return project;
        }

        public CreateViewModel ConvertProjectToModel(ElectionProject project)
        {
            var model = new CreateViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Date = project.Date,
                Description = project.Description,
                JurisdictionName = project.JurisdictionName,
                JurisdictionTypeId = project.JurisdictionType.Id,
                ElectionTypeId = project.ElectionType.Id
            };

            return model;
        }
    }


}