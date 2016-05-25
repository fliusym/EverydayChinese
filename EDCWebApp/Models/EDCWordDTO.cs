using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDCWebApp.Models
{
    public class PhraseExample
    {
        public string Chinese { get; set; }
        public string English { get; set; }
    }
    public class Phrase
    {
        public string Chinese { get; set; }
        public string English { get; set; }
        public string Pinyin { get; set; }
        public IEnumerable<PhraseExample> Examples { get; set; }
    }
    public class Quote
    {
        public string Where { get; set; }
        public string What { get; set; }
        public string Who { get; set; }
    }
    public class EDCWordDTO
    {
        public int Id { get; set; }
        public string Character { get; set; }
        public string Pinyin { get; set; }
        public string Explanation { get; set; }
        public string Audio { get; set; }
        public string BasicMeanings { get; set; }
        public string Date { get; set; }

        public IEnumerable<Phrase> Phrases { get; set; }
        public IEnumerable<Quote> Quotes { get; set; }
    }
}