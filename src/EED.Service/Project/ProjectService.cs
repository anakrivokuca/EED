using EED.DAL;
using EED.Domain;
using EED.Service.Districts;
using EED.Service.District_Type;
using EED.Service.Jurisdiction_Type;
using EED.Service.Membership_Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EED.Service.Project
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<ElectionProject> _repository;
        private readonly IAuthProvider _provider;
        private readonly IJurisdictionTypeService _jurisdictionTypeService;

        public ProjectService(IRepository<ElectionProject> repository, IAuthProvider provider,
            IJurisdictionTypeService jurisdictionTypeService)
        {
            _repository = repository;
            _provider = provider;
            _jurisdictionTypeService = jurisdictionTypeService;
        }

        public IEnumerable<ElectionProject> FindAllProjects()
        {
            return _repository.FindAll();
        }

        public IEnumerable<ElectionProject> FindAllProjectsFromUser()
        {
            int userId = _provider.GetUserFromCookie().Id;
            var projects = FindAllProjects().Where(p => p.User.Id == userId);
            
            return projects;
        }

        public ElectionProject FindProject(int id)
        {
            return _repository.Find(id);
        }

        public IEnumerable<ElectionProject> FilterProjects(IEnumerable<ElectionProject> projects,
            string searchText)
        {
            string[] keywords = searchText.Trim().Split(' ');
            foreach (var k in keywords.Where(k => !String.IsNullOrEmpty(k)))
            {
                string keyword = k;
                projects = projects
                    .Where(p => (String.Equals(p.Name, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(p.JurisdictionName, keyword, StringComparison.CurrentCultureIgnoreCase)));
            }
            return projects;
        }

        public void SaveProject(ElectionProject project)
        {
            try
            {
                var jurisdictionType = _jurisdictionTypeService
                        .FindJurisdictionType(project.JurisdictionType.Id);

                if (project.Id == 0)
                {
                    int userId = _provider.GetUserFromCookie().Id;
                    project.User = new User { Id = userId };

                    var districtType = new DistrictType
                    {
                        Name = jurisdictionType.Name,
                        Project = project
                    };

                    var district = new Domain.District
                    {
                        Name = project.JurisdictionName,
                        DistrictType = districtType,
                        Project = project
                    };

                    project.DistrictTypes = new List<DistrictType>();
                    project.DistrictTypes.Add(districtType);
                    project.Districts = new List<Domain.District>();
                    project.Districts.Add(district);
                }
                else
                {
                    var existingProject = FindProject(project.Id);
                    existingProject.Name = project.Name;
                    existingProject.Date = project.Date;
                    existingProject.Description = project.Description;
                    existingProject.JurisdictionName = project.JurisdictionName;
                    existingProject.JurisdictionType = project.JurisdictionType;

                    var districtType = existingProject.DistrictTypes.Single(dt => dt.ParentDistrictType == null);
                    //existingProject.DistrictTypes.Remove(districtType);
                    districtType.Name = jurisdictionType.Name;
                    //existingProject.DistrictTypes.Add(districtType);

                    var district = existingProject.Districts.Single(d => d.ParentDistrict == null);
                    //existingProject.Districts.Remove(district);
                    district.Name = project.JurisdictionName;
                    //existingProject.Districts.Add(district);

                    project = existingProject;
                }

                _repository.Save(project);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }

        public void DeleteProject(ElectionProject project)
        {
            try
            {
                _repository.Delete(project);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }
    }
}
