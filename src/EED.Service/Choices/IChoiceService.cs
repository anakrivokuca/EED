using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Choices
{
    public interface IChoiceService
    {
        IEnumerable<Choice> FindAllChoices();
        Choice FindChoice(int id);
        IEnumerable<Choice> FilterChoices(IEnumerable<Choice> choices, string searchText, int contestId);
        void SaveChoice(Choice choice);
        void DeleteChoice(Choice choice);
    }
}
