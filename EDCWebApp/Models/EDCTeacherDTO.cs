using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class EDCTeacherDTO
    {
        public string TeacherName { get; set; }
        public IEnumerable<EDCLearnRequestDTO> LearnRequests { get; set; }
    }
}