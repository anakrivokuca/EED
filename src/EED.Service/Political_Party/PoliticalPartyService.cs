﻿using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Political_Party
{
    public class PoliticalPartyService : IPoliticalPartyService
    {
        private readonly IRepository<PoliticalParty> _repository;

        public PoliticalPartyService(IRepository<PoliticalParty> repository)
        {
            _repository = repository;
        }

        public IEnumerable<PoliticalParty> FindAllPoliticalParties()
        {
            return _repository.FindAll();
        }

        public PoliticalParty FindPoliticalParty(int id)
        {
            return _repository.Find(id);
        }

        public IEnumerable<PoliticalParty> FilterPoliticalParties(IEnumerable<PoliticalParty> politicalParties, string searchText)
        {
            string text = searchText.Trim();
            if (!String.IsNullOrEmpty(text))
            {
                politicalParties = politicalParties
                    .Where(o => (String.Equals(o.Name, text, StringComparison.CurrentCultureIgnoreCase)));
            }
            return politicalParties;
        }

        public void SavePoliticalParty(PoliticalParty politicalParty)
        {
            throw new NotImplementedException();
        }

        public void DeletePoliticalParty(PoliticalParty politicalParty)
        {
            throw new NotImplementedException();
        }
    }
}