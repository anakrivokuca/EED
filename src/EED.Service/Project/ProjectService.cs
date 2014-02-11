using EED.DAL;
using EED.Domain;
using EED.Service.District;
using EED.Service.District_Type;
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
        private readonly IDistrictTypeService _districtTypeService;
        private readonly IDistrictService _districtService;

        public ProjectService(IRepository<ElectionProject> repository, IAuthProvider provider,
            IDistrictTypeService districtTypeService, IDistrictService districtService)
        {
            _repository = repository;
            _provider = provider;
            _districtTypeService = districtTypeService;
            _districtService = districtService;
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
                int userId = _provider.GetUserFromCookie().Id;
                project.User = new User { Id = userId };

                _repository.Save(project);

                if (project.Id == 0)
                {
                    var districtType = new DistrictType
                    {
                        Name = project.JurisdictionType.Name,
                        Project = project
                    };
                    _districtTypeService.SaveDistrictType(districtType);

                    var district = new Domain.District
                    {
                        Name = project.JurisdictionName,
                        DistrictType = districtType,
                        Project = project
                    };
                    _districtService.SaveDistrict(district);
                }
                else
                {
                    var district = _districtService.FindAllDistrictsFromProject(project.Id)
                        .Single(d => d.ParentDistrict == null);
                    district.Name = project.JurisdictionName;
                    _districtService.SaveDistrict(district);
                }
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
