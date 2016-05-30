using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDCWebApp.Models;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EDCWebApp.Tests
{
    static class TestUtils
    {
        #region getting words
        public static IList<EDCWebApp.Models.EDCWord> GetWords()
        {


            //example
            var examples = new List<EDCPhraseExample>();
            examples.Add(new EDCPhraseExample()
            {
                ID = 1,
                Chinese = "别人都说他肯定能赢下这场比赛.",
                Englisgh = "All others believe he will win this game.",
                PhraseID = 1
            });
            examples.Add(new EDCPhraseExample()
            {
                ID = 2,
                Chinese = "别人都完成了作业，就剩下托尼还没有完成.",
                Englisgh = "All others have finished their homework except Tony. ",
                PhraseID = 1
            });
            examples.Add(new EDCPhraseExample()
            {
                ID = 3,
                Chinese = "那个女人真漂亮.",
                Englisgh = "That woman is very beautiful.",
                PhraseID = 2
            });
            examples.Add(new EDCPhraseExample()
            {
                ID = 4,
                Chinese = "故人已去，不免令人伤感.",
                Englisgh = "I feel so sad when I know my old friend has gone.",
                PhraseID = 3
            });
            examples.Add(new EDCPhraseExample()
            {
                ID = 5,
                Chinese = "他有很好的人品，其他人都很喜欢他.",
                Englisgh = "He has a good personality that all others like him.",
                PhraseID = 4
            });
            examples.Add(new EDCPhraseExample()
            {
                ID = 6,
                Chinese = "现在还有许多问题没有解决.",
                Englisgh = "For now there are still many problems not solved yet.",
                PhraseID = 5
            });
            examples.Add(new EDCPhraseExample()
            {
                ID = 7,
                Chinese = "现在这本书看起来是多余的.",
                Englisgh = "Now this book looks like surplus.",
                PhraseID = 6
            });


            var phraseCharacters = new List<string>();
            var phraseEnglishWords = new List<string>();
            phraseCharacters.Add("别人");
            phraseEnglishWords.Add("Other");
            phraseCharacters.Add("女人");
            phraseEnglishWords.Add("Woman");
            phraseCharacters.Add("故人");
            phraseEnglishWords.Add("Old friend/Someone you are familiar with died");
            phraseCharacters.Add("人品");
            phraseEnglishWords.Add("Personality");
            var phraseAudios = new List<string>();
            phraseAudios.Add("Content/Audios/PhraseAudio/别人.m4a");
            phraseAudios.Add("Content/Audios/PhraseAudio/女人.m4a");
            phraseAudios.Add("Content/Audios/PhraseAudio/故人.m4a");
            phraseAudios.Add("Content/Audios/PhraseAudio/人品.m4a");
            var phrases = new List<EDCPhrase>
            {
                new EDCPhrase{ ID = 1, WordID = 1, Pinyin = phraseAudios[0], Chinese = phraseCharacters[0], English = phraseEnglishWords[0]},
                new EDCPhrase{ ID = 2, WordID = 1, Pinyin = phraseAudios[1], Chinese = phraseCharacters[1], English = phraseEnglishWords[1]},
                new EDCPhrase{ ID = 3, WordID = 1, Pinyin = phraseAudios[2], Chinese = phraseCharacters[2], English = phraseEnglishWords[2]},
                new EDCPhrase{ ID = 4, WordID = 1, Pinyin = phraseAudios[3], Chinese = phraseCharacters[3], English = phraseEnglishWords[3]}
            };

            var phraseCharactersTwo = new List<string>();
            var phraseEnglishTwo = new List<string>();
            phraseCharactersTwo.Add("许多");
            phraseEnglishTwo.Add("Many");
            phraseCharactersTwo.Add("多余");
            phraseEnglishTwo.Add("Surplus/Excess");
            var phraseAudiosTwo = new List<string>();
            phraseAudiosTwo.Add("Content/Audios/PhraseAudio/许多.m4a");
            phraseAudiosTwo.Add("Content/Audios/PhraseAudio/多余.m4a");
            var phrasesTwo = new List<EDCPhrase>
            {
                new EDCPhrase{ ID = 5, WordID = 2, Pinyin = phraseAudiosTwo[0], Chinese = phraseCharactersTwo[0],English = phraseEnglishTwo[0]},
                new EDCPhrase{ ID = 6, WordID = 2, Pinyin = phraseAudiosTwo[1], Chinese = phraseCharactersTwo[1],English = phraseEnglishTwo[1]}
            };


            var words = new List<EDCWord>();
            var wordCharacters = new List<string>();
            wordCharacters.Add("人");
            wordCharacters.Add("多");
            var wordMeanings = new List<string>();
            wordMeanings.Add("Human/Person");
            wordMeanings.Add("Many/Not Small");
            var wordPinyins = new List<string>();
            wordPinyins.Add("Content/Pinyin/WordPinyin/人.png");
            wordPinyins.Add("Content/Pinyin/WordPinyin/多.png");
            var wordAudios = new List<string>();
            wordAudios.Add("Content/Audios/WordAudio/人.m4a");
            wordAudios.Add("Content/Audios/WordAudio/多.m4a");

            words.Add(new EDCWord()
            {
                Character = wordCharacters[0],
                BasicMeanings = wordMeanings[0],
                Pinyin = wordPinyins[0],
                Phrases = phrases,
                Audio = wordAudios[0],
                Date = DateTime.Parse("2016-05-22").ToShortDateString(),
                StudentName = "yov.max@gmail.com"
            });

            words.Add(new EDCWord()
            {
                Character = wordCharacters[1],
                BasicMeanings = wordMeanings[1],
                Pinyin = wordPinyins[1],
                Phrases = phrasesTwo,
                Audio = wordAudios[1],
                Date = DateTime.Parse("2016-05-21").ToShortDateString(),
                StudentName = "yov.max@gmail.com"
            });
            return words;

        }

        public static EDCStudent GetStudent()
        {
            var words = GetWords();
            var student = new EDCStudent()
            {
                StudentName = "test@gmail.com",
                Words = words
            };
            return student;
        }
        public static void CheckExceptionMessage(HttpResponseException e, string checkMsg)
        {
            var response = e.Response.Content.ReadAsStringAsync();
            //  var obj = JsonConvert.DeserializeObject(response.Result);
            var obj = JObject.Parse(response.Result);
            var objValue = obj.GetValue("ModelState");
            Assert.IsNotNull(objValue);
            var msgProperty = objValue.First as JProperty;
            Assert.IsNotNull(msgProperty);
            var msgArr = msgProperty.Value.ToObject<string[]>();
            Assert.IsNotNull(msgArr);
            Assert.IsTrue(msgArr.Length == 1);
            var message = msgArr[0];
            Assert.IsTrue(message == checkMsg);
        }
    }
        #endregion

}
