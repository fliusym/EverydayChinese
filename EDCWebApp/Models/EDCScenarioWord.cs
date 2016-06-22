using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class EDCScenarioWord
    {
        public int ID { get; set; }

        public string ChineseWord { get; set; }
        public string ChineseWordPinyin { get; set; }
        public string ChineseWordAudio { get; set; }

        [ForeignKey("Image")]
        public int ImageId { get; set; }
        public EDCScenarioImage Image { get; set; }
    }
}