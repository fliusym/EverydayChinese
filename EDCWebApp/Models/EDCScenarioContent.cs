using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EDCWebApp.Models
{
    public class EDCScenarioContent
    {
        public int ID { get; set; }

        public ICollection<EDCScenarioImage> Images { get; set; }
        public string Date { get; set; }
        public string ThemeEnglish { get; set; }
        public string ThemeChinese { get; set; }

        //public int LoginUserId { get; set; }
        //public LoginUser LoginUser { get; set; }
        [ForeignKey("Student")]
        public string StudentName { get; set; }
        public EDCStudent Student { get; set; }
    }
}
