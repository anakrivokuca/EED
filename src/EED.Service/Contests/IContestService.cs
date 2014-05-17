using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Contests
{
    public interface IContestService
    {
        IEnumerable<Contest> FindAllContests();
        Contest FindContest(int id);
        IEnumerable<Contest> FilterContests(IEnumerable<Contest> contests, string searchText, int officeId);
        void SaveContest(Contest contest);
        void DeleteContest(Contest contest);
    }
}
