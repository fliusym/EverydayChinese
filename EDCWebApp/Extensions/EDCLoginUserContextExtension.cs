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

            var quotes = new List<Quote>();
            if (word.Quotes != null)
            {
                foreach (var q in word.Quotes)
                {
                    quotes.Add(new Quote
                    {
                        Who = q.Who,
                        What = q.What,
                        Where = q.Where
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
                Quotes = quotes
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
                images.Add(new ScenarioImage()
                {
                    Image = i.Image
                });
            }
            return new EDCScenarioContentDTO()
            {
                Id = content.ID,
                Images = images
            };
        }

        public static void AssignTeacherToLearnRequest(this IEDCLoginUserContext context, EDCLearnRequest learnRequest, string teacherName)
        {
            if (context != null && learnRequest != null)
            {
                var teacher = context.Teachers.Find(teacherName);
                if (teacher != null)
                {
                    teacher.LearnRequests.Add(learnRequest);
                }
            }
        }
    }
}