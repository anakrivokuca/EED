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
            return _repository.FindAll();
        }

        public Choice FindChoice(int id)
        {
            return _repository.Find(id);
        }

        public IEnumerable<Choice> FilterChoices(IEnumerable<Choice> choices, string searchText, int contestId)
        {
            string text = searchText.Trim();
            if (!String.IsNullOrEmpty(text))
            {
                choices = choices
                    .Where(c => (String.Equals(c.Name, text, StringComparison.CurrentCultureIgnoreCase)));
            }

            if (contestId != 0)
            {
                choices = choices
                    .Where(c => c.Contest != null && c.Contest.Id == contestId);
            }

            return choices;
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
