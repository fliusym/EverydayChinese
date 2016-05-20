using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDCWebApp.Models
{
    public class EDCPhraseExample
    {
        public int ID { get; set; }

        public string Chinese { get; set; }
        public string Englisgh { get; set; }

        public int PhraseID { get; set; }
        public EDCPhrase Phrase { get; set; }
    }
}
