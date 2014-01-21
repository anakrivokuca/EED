using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Project
{
    public interface IProjectService
    {
        IEnumerable<ElectionProject> FindAllProjects();
        IEnumerable<ElectionProject> FindAllProjectsFromUser();
        IEnumerable<ElectionProject> FilterProjects(IEnumerable<ElectionProject> projects,
            string searchText);
        void SaveProject(ElectionProject project);
        void DeleteProject(ElectionProject project);
    }
}
