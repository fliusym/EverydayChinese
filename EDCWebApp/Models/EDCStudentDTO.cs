using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class EDCStudentDTO
    {
        public string Name { get; set; }
        public IEnumerable<EDCWordDTO> Words { get; set; }
        public IEnumerable<EDCScenarioContentDTO> Scenarios { get; set; }
        public IEnumerable<EDCLearnRequestDTO> LearnRequests { get; set; }
    }
}