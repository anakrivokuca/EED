﻿using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;

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

        public IEnumerable<Contest> FilterContests(IEnumerable<Contest> contests, string searchText)
        {
            throw new NotImplementedException();
            //string text = searchText.Trim();
            //if (!String.IsNullOrEmpty(text))
            //{
            //    contests = contests
            //        .Where(o => (String.Equals(o.Name, text, StringComparison.CurrentCultureIgnoreCase)));
            //}
            //return contests;
        }

        public void SaveContest(Contest contest)
        {
            throw new NotImplementedException();
        }

        public void DeleteContest(Contest contest)
        {
            throw new NotImplementedException();
        }
    }
}
