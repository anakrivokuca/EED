﻿using EED.Domain;
using EED.Service.Precincts;
using EED.Service.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Precincts
{
    public class PrecinctServiceController : IPrecinctServiceController
    {
        private readonly IPrecinctService _service;
        private readonly IProjectService _projectService;

        public PrecinctServiceController(IPrecinctService service, IProjectService projectService)
        {
            _service = service;
            _projectService = projectService;
        }

        public ElectionProject FindProject(int id)
        {
            return _projectService.FindProject(id);
        }
        
        public Precinct FindPrecinct(int id)
        {
            return _service.FindPrecinct(id);
        }

        public IEnumerable<Precinct> FilterPrecincts(IEnumerable<Precinct> precincts, 
            string searchText, int districtId)
        {
            return _service.FilterPrecincts(precincts, searchText, districtId);
        }

        public void SavePrecinct(Precinct precinct)
        {
            _service.SavePrecinct(precinct);
        }

        public void DeletePrecinct(Precinct precinct)
        {
            _service.DeletePrecinct(precinct);
        }
    }
}
