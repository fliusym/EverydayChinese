using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class EDCTeacher
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public string TeacherName { get; set; }

        public ICollection<EDCLearnRequest> LearnRequests { get; set; }
    }
}