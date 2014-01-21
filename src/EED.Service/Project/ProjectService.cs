﻿using EED.DAL;
using EED.Domain;
using EED.Service.Membership_Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Project
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<ElectionProject> _repository;
        private readonly IAuthProvider _provider;

        public ProjectService(IRepository<ElectionProject> repository, IAuthProvider provider)
        {
            _repository = repository;
            _provider = provider;
        }

        public IEnumerable<ElectionProject> FindAllProjects()
        {
            return _repository.FindAll();
        }

        public IEnumerable<ElectionProject> FindAllProjectsFromUser()
        {
            int id = _provider.GetUserFromCookie().Id;
            var projects = FindAllProjects().Where(p => p.User.Id == id);
            
            return projects;
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
            throw new NotImplementedException();
        }

        public void DeleteProject(ElectionProject project)
        {
            throw new NotImplementedException();
        }
    }
}
