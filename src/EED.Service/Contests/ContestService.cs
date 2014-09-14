using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EED.Service.Contests
{
    public class ContestService : IContestService
    {
        private readonly IRepository<Contest> _repository;

        public ContestService(IRepository<Contest> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Contest> FindAllContests()
        {
            return _repository.FindAll();
        }

        public Contest FindContest(int id)
        {
            return _repository.Find(id);
        }

        public IEnumerable<Contest> FilterContests(IEnumerable<Contest> contests, string searchText, int officeId)
        {
            string text = searchText.Trim();
            if (!String.IsNullOrEmpty(text))
            {
                contests = contests
                    .Where(c => (String.Equals(c.Name, text, StringComparison.CurrentCultureIgnoreCase)));
            }

            if (officeId != 0)
            {
                contests = contests
                    .Where(c => c.Office != null && c.Office.Id == officeId);
            }

            return contests;
        }

        public void SaveContest(Contest contest)
        {
            try
            {
                if (contest.Id != 0)
                {
                    var existingContest = FindContest(contest.Id);
                    existingContest.Name = contest.Name;
                    existingContest.NumberOfPositions = contest.NumberOfPositions;
                    existingContest.Office = contest.Office;
                    existingContest.District = contest.District;
                    contest = existingContest;
                }
                _repository.Save(contest);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }

        public void DeleteContest(Contest contest)
        {
            _repository.Delete(contest);
        }
    }
}
