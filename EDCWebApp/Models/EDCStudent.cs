using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class EDCStudent
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public string StudentName { get; set; }

        public ICollection<EDCWord> Words { get; set; }
        public ICollection<EDCScenarioContent> Scenarios { get; set; }
        public ICollection<EDCLearnRequest> LearnRequests { get; set; }

    }
}