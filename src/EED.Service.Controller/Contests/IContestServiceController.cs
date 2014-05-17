using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Controller.Contests
{
    public interface IContestServiceController
    {
        ElectionProject FindProject(int id);
        Contest FindContest(int id);
        IEnumerable<Contest> FilterContests(IEnumerable<Contest> contests, string searchText);
        void SaveContest(Contest contest);
        void DeleteContest(Contest contest);
    }
}
