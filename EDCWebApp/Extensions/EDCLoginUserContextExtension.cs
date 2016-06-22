using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EDCWebApp.Models;
using EDCWebApp.DAL;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EDCWebApp.Extensions
{
    public static class EDCLoginUserContextExtension
    {
        public static EDCWordDTO GenerateDTO(this IEDCLoginUserContext context, EDCWord word)
        {
            if (context == null || word == null)
            {
                return null;
            }
            var phrases = new List<Phrase>();
            if (word.Phrases != null)
            {
                foreach (var p in word.Phrases)
                {
                    var examples = new List<PhraseExample>();
                    if (p.Examples != null)
                    {
                        foreach (var e in p.Examples)
                        {
                            examples.Add(new PhraseExample()
                            {
                                English = e.Englisgh,
                                Chinese = e.Chinese
                            });
                        }
                    }
                    phrases.Add(new Phrase()
                    {
                        Chinese = p.Chinese,
                        English = p.English,
                        Pinyin = p.Pinyin,
                        Examples = examples
                    });
                }
            }

            var slangs = new List<Slang>();
            if (word.Slangs != null)
            {
                foreach (var q in word.Slangs)
                {
                    slangs.Add(new Slang
                    {
                        SlangChinese = q.SlangChinese,
                        SlangEnglish = q.SlangEnglish,
                        SlangExampleChinese = q.SlangExampleChinese,
                        SlangExampleEnglish = q.SlangExampleEnglish
                    });
                }
            }
            return new EDCWordDTO()
            {
                Id = word.ID,
                Audio = word.Audio,
                BasicMeanings = word.BasicMeanings,
                Pinyin = word.Pinyin,
                Character = word.Character,
                Explanation = word.Explanation,
                Date = word.Date,
                Phrases = phrases,
                Slangs = slangs
            };
        }

        public static EDCLearnRequestDTO GenerateDTO(this IEDCLoginUserContext context, EDCLearnRequest request)
        {
            if (context == null || request == null)
            {
                return null;
            }
            var names = new List<string>();
            if (request.RegisteredStudents != null)
            {
                foreach (var n in request.RegisteredStudents)
                {
                    names.Add(n.StudentName);
                }
            }
            return new EDCLearnRequestDTO()
            {
                Id = request.ID,
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                StudentNames = names
            };

        }

        public static EDCScenarioContentDTO GenerateDTO(this IEDCLoginUserContext context, EDCScenarioContent content)
        {
            if (context == null || content == null)
            {
                return null;
            }
            var images = new List<ScenarioImage>();
            foreach (var i in content.Images)
            {
                var words = new List<ScenarioWord>();
                foreach (var w in i.Words)
                {
                    words.Add(new ScenarioWord
                    {
                        Word = w.ChineseWord,
                        Pinyin = w.ChineseWordPinyin,
                        Audio = w.ChineseWordAudio
                    });
                }
                images.Add(new ScenarioImage()
                {
                    Image = i.Image,
                    Words = words
                });
            }
            return new EDCScenarioContentDTO()
            {
                Id = content.ID,
                ThemeChinese = content.ThemeChinese,
                ThemeEnglish = content.ThemeEnglish,
                Images = images
            };
        }

        public static void AssignTeacherToLearnRequest(this IEDCLoginUserContext context, EDCLearnRequest learnRequest, string teacherName)
        {
            if (context != null && learnRequest != null)
            {
                var teacher = context.Teachers.Include(p => p.LearnRequests)
                    .Where(p => p.TeacherName == teacherName).SingleOrDefault();
                if (teacher != null)
                {
                    teacher.LearnRequests.Add(learnRequest);
                }
            }
        }
    }
}