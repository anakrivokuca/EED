using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Choices
{
    public class ChoiceService : IChoiceService
    {
        private readonly IRepository<Choice> _repository;

        public ChoiceService(IRepository<Choice> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Choice> FindAllChoices()
        {
            throw new NotImplementedException();
        }

        public Choice FindChoice(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Choice> FilterChoices(IEnumerable<Choice> choices, string searchText, int contestId)
        {
            throw new NotImplementedException();
        }

        public void SaveChoice(Domain.Choice choice)
        {
            throw new NotImplementedException();
        }

        public void DeleteChoice(Choice choice)
        {
            throw new NotImplementedException();
        }
    }
}
