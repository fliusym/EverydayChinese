using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
namespace EDCWebApp.Models
{
    public class PhraseExampleBindingModel
    {
        [JsonProperty("english")]
        public string English { get; set; }
        [JsonProperty("chinese")]
        public string Chinese { get; set; }
    }
    public class PhraseBindingModel
    {
        [JsonProperty("english")]
        public string English { get; set; }
        [JsonProperty("chinese")]
        public string Chinese { get; set; }
        [JsonProperty("pinyin")]
        public string Pinyin { get; set; }
        [JsonProperty("examples")]
        public IEnumerable<PhraseExampleBindingModel> Examples { get; set; }
    }
    public class SlangBindingModel
    {
        [JsonProperty("slangEnglish")]
        public string SlangEnglish { get; set; }
        [JsonProperty("slangChinese")]
        public string SlangChinese { get; set; }
        [JsonProperty("slangExampleEnglish")]
        public string SlangExampleEnglish { get; set; }
        [JsonProperty("slangExampleChinese")]
        public string SlangExampleChinese { get; set; }
    }
    [ModelBinder(typeof(EDCLearnRequestBinderProvider))]
    public class AddWordBindingModel
    {
        [Required]
        [JsonProperty("character")]
        public string Character { get; set; }
        [Required]
        [JsonProperty("pinyin")]
        public string Pinyin { get; set; }
        [Required]
        [JsonProperty("audio")]
        public string Audio { get; set; }
        [Required]
        [JsonProperty("basicMeanings")]
        public string BasicMeanings { get; set; }
        [JsonProperty("explanation")]
        public string Explanation { get; set; }
        [Required]
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("phrases")]
        public IEnumerable<PhraseBindingModel> Phrases { get; set; }
        [JsonProperty("slangs")]
        public IEnumerable<SlangBindingModel> Slangs { get; set; }

    }
}