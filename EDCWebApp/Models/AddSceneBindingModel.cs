using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EDCWebApp.Models
{
    public class SceneImageWordBindingModel
    {
        [JsonProperty("wordChinese")]
        public string WordChinese { get; set; }
        [JsonProperty("wordPinyin")]
        public string WordPinyin { get; set; }
        [JsonProperty("wordAudio")]
        public string WordAudio { get; set; }
    }
    public class SceneImageBindingModel
    {
        [JsonProperty("imageSrc")]
        public string ImageSrc { get; set; }
        [JsonProperty("words")]
        public IEnumerable<SceneImageWordBindingModel> SceneWords { get; set; }
    }
    public class AddSceneBindingModel
    {
        [JsonProperty("titleChinese")]
        [Required]
        public string TitleChinese { get; set; }
        [JsonProperty("titleEnglish")]
        [Required]
        public string TitleEnglish { get; set; }
        [JsonProperty("date")]
        [Required]
        public string Date { get; set; }
        [JsonProperty("images")]
        [Required]
        public IEnumerable<SceneImageBindingModel> Images { get; set; }
    }
}