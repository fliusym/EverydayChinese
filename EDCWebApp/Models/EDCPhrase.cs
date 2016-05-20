using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EDCWebApp.Models
{
    public class EDCPhrase
    {
        public int ID { get; set; }


        public string Chinese { get; set; }
        public string English { get; set; }
        public string Pinyin { get; set; }
        public ICollection<EDCPhraseExample> Examples { get; set; }

        [ForeignKey("Word")]
        public int WordID { get; set; }
        public EDCWord Word { get; set; }
    }
}
