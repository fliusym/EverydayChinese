using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDCWebApp.Models
{
    public class EDCLearnRequest
    {
        public int ID { get; set; }

        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public ICollection<EDCStudent> RegisteredStudents { get; set; }
    }
}
