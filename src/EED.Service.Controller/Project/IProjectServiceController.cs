using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Project
{
    public interface IProjectServiceController
    {
        IEnumerable<ElectionType> FindAllElectionTypes();
        IEnumerable<JurisdictionType> FindAllJurisdictionTypes();
        JurisdictionType FindJurisdictionType(int id);
        IEnumerable<ElectionProject> FindAllProjects();
        IEnumerable<ElectionProject> FindAllProjectsFromUser();
        ElectionProject FindProject(int id);
        IEnumerable<ElectionProject> FilterProjects(IEnumerable<ElectionProject> projects,
            string searchText);
        void SaveProject(ElectionProject project);
        void DeleteProject(ElectionProject project);
    }
}
