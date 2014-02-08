using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Project
{
    public interface IProjectService
    {
        IEnumerable<ElectionProject> FindAllProjects();
        IEnumerable<ElectionProject> FindAllProjectsFromUser();
        ElectionProject FindProject(int id);
        IEnumerable<ElectionProject> FilterProjects(IEnumerable<ElectionProject> projects,
            string searchText);
        void SaveProject(ElectionProject project);
        void DeleteProject(ElectionProject project);
    }
}
