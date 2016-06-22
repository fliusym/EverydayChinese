using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class ScenarioWord
    {
        public string Word { get; set; }
        public string Pinyin { get; set; }
        public string Audio { get; set; }
    }
    public class ScenarioImage
    {
        public string Image { get; set; }
        public IEnumerable<ScenarioWord> Words { get; set; }
    }
    public class EDCScenarioContentDTO
    {
        public int Id { get; set; }
        public string ThemeChinese { get; set; }
        public string ThemeEnglish { get; set; }
        public IEnumerable<ScenarioImage> Images { get; set; }
    }
}