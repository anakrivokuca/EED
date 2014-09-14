using EED.Domain;
using EED.Service.Contests;
using EED.Service.Project;
using System;
using System.Collections.Generic;

namespace EED.Service.Controller.Contests
{
    public class ContestServiceController : IContestServiceController
    {
        private readonly IContestService _service;
        private readonly IProjectService _projectService;

        public ContestServiceController(IContestService service, IProjectService projectService)
        {
            _service = service;
            _projectService = projectService;
        }

        public ElectionProject FindProject(int id)
        {
            return _projectService.FindProject(id);
        }

        public Contest FindContest(int id)
        {
            return _service.FindContest(id);
        }

        public IEnumerable<Contest> FilterContests(IEnumerable<Contest> contests, string searchText, int officeId)
        {
            return _service.FilterContests(contests, searchText, officeId);
        }

        public void SaveContest(Contest contest)
        {
            _service.SaveContest(contest);
        }

        public void DeleteContest(Contest contest)
        {
            _service.DeleteContest(contest);
        }
    }
}
