using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Choices
{
    public interface IChoiceServiceController
    {
        ElectionProject FindProject(int id);
        Choice FindChoice(int id);
        IEnumerable<Choice> FilterChoices(IEnumerable<Choice> choices, string searchText, int contestId);
        void SaveChoice(Choice choice);
        void DeleteChoice(Choice choice);
    }
}
