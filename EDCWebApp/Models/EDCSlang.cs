using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDCWebApp.Models
{
    public class EDCSlang
    {
        public int ID { get; set; }

        //public string Where { get; set; }
        //public string What { get; set; }
        //public string Who { get; set; }
        public string SlangEnglish { get; set; }
        public string SlangChinese { get; set; }
        public string SlangExampleEnglish { get; set; }
        public string SlangExampleChinese { get; set; }

        public int WordID { get; set; }
        public EDCWord Word { get; set; }
    }
}
