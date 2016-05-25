using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class ScenarioImage
    {
        public string Image { get; set; }
    }
    public class EDCScenarioContentDTO
    {
        public int Id { get; set; }
        public IEnumerable<ScenarioImage> Images { get; set; }
    }
}