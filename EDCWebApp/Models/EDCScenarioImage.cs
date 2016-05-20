using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDCWebApp.Models
{
    public class EDCScenarioImage
    {
        public int ID { get; set; }

        public string Image { get; set; }

        public int ContentID { get; set; }
        public EDCScenarioContent Content { get; set; }
    }
}
