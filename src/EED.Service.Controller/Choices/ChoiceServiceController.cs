using EED.Domain;
using EED.Service.Choices;
using EED.Service.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Choices
{
    public class ChoiceServiceController : IChoiceServiceController
    {
        private readonly IChoiceService _service;
        private readonly IProjectService _projectService;

        public ChoiceServiceController(IChoiceService service, IProjectService projectService)
        {
            _service = service;
            _projectService = projectService;
        }

        public ElectionProject FindProject(int id)
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

        public void SaveChoice(Choice choice)
        {
            throw new NotImplementedException();
        }

        public void DeleteChoice(Choice choice)
        {
            throw new NotImplementedException();
        }
    }
}
