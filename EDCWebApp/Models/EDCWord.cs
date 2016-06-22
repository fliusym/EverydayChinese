using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EDCWebApp.Models
{
    public class EDCWord
    {
        public int ID { get; set; }

        public string Character { get; set; }
        public string Pinyin { get; set; }
        public string Audio { get; set; }
        public string Explanation { get; set; }
        public string BasicMeanings { get; set; }
        public ICollection<EDCPhrase> Phrases { get; set; }
        public ICollection<EDCSlang> Slangs { get; set; }
        public string Date { get; set; }

        [ForeignKey("Student")]
        public string StudentName { get; set; }
        public EDCStudent Student { get; set; }

    }
}
