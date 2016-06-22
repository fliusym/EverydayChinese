namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using EDCWebApp.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<EDCWebApp.DAL.EDCLoginUserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EDCWebApp.DAL.EDCLoginUserContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            #region learn requests
            DateTime[] dts = new DateTime[] { DateTime.Now.Date, (DateTime.Now.AddDays(4)).Date };
            string[] times = new string[] { "7:00 am", "9:00 am" };
            string[] endTimes = new string[] { "8:00 am", "10:00 am" };
            var registeredStudents = new List<EDCStudent>();

            var learnRequests = new List<EDCLearnRequest>
            {
                new EDCLearnRequest{ ID = 1, Date = dts[0].ToShortDateString(), StartTime = times[0], EndTime = endTimes[0], RegisteredStudents = new List<EDCStudent>()},
                new EDCLearnRequest{ ID = 2, Date = dts[1].ToShortDateString(), StartTime = times[1], EndTime = endTimes[1], RegisteredStudents = new List<EDCStudent>()}
            };
            context.LearnRequests.AddOrUpdate(x => x.ID, learnRequests[0], learnRequests[1]
                );

            #endregion

            #region words

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
            context.PhraseExamples.AddOrUpdate(x => x.ID, examples[0], examples[1], examples[2], examples[3]
                , examples[4], examples[5], examples[6]);

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
            phraseAudios.Add("别人.m4a");
            phraseAudios.Add("女人.m4a");
            phraseAudios.Add("故人.m4a");
            phraseAudios.Add("人品.m4a");
            var phrases = new List<EDCPhrase>
            {
                new EDCPhrase{ ID = 1, WordID = 1, Pinyin = phraseAudios[0], Chinese = phraseCharacters[0], English = phraseEnglishWords[0]},
                new EDCPhrase{ ID = 2, WordID = 1, Pinyin = phraseAudios[1], Chinese = phraseCharacters[1], English = phraseEnglishWords[1]},
                new EDCPhrase{ ID = 3, WordID = 1, Pinyin = phraseAudios[2], Chinese = phraseCharacters[2], English = phraseEnglishWords[2]},
                new EDCPhrase{ ID = 4, WordID = 1, Pinyin = phraseAudios[3], Chinese = phraseCharacters[3], English = phraseEnglishWords[3]}
            };
            context.Phrases.AddOrUpdate(x => x.ID, phrases[0], phrases[1], phrases[2], phrases[3]);

            var phraseCharactersTwo = new List<string>();
            var phraseEnglishTwo = new List<string>();
            phraseCharactersTwo.Add("许多");
            phraseEnglishTwo.Add("Many");
            phraseCharactersTwo.Add("多余");
            phraseEnglishTwo.Add("Surplus/Excess");
            var phraseAudiosTwo = new List<string>();
            phraseAudiosTwo.Add("许多.m4a");
            phraseAudiosTwo.Add("多余.m4a");
            var phrasesTwo = new List<EDCPhrase>
            {
                new EDCPhrase{ ID = 5, WordID = 2, Pinyin = phraseAudiosTwo[0], Chinese = phraseCharactersTwo[0],English = phraseEnglishTwo[0]},
                new EDCPhrase{ ID = 6, WordID = 2, Pinyin = phraseAudiosTwo[1], Chinese = phraseCharactersTwo[1],English = phraseEnglishTwo[1]}
            };
            context.Phrases.AddOrUpdate(x => x.ID, phrasesTwo[0], phrasesTwo[1]);

            var slangs = new List<EDCSlang>
            {
                new EDCSlang{ID = 1, SlangChinese = "人迹罕至", SlangEnglish = "Remote place/A place few people can reach", SlangExampleChinese = "在这个人迹罕至的地方，很难看到绿色植物", SlangExampleEnglish = "In this extreme remote place, it is hard to find any green plant",WordID = 1},
                new EDCSlang{ID = 2, SlangChinese = "骇人听闻", SlangEnglish = "Appalling", SlangExampleChinese = "当听到这个骇人听闻的消息，所有人都惊呆了", SlangExampleEnglish = "Everybody is stunned by this appalling news",WordID = 1}
            };
            var slangsTwo = new List<EDCSlang>
            {
                new EDCSlang{ID = 3, SlangChinese = "多愁善感",SlangEnglish = "Melancholy", SlangExampleChinese = "自从珍妮母亲去世以来，她变得有些多愁善感",SlangExampleEnglish = "Jenny becomes a little melancholy since her mother passed away",WordID = 2}
            };
            context.Slangs.AddOrUpdate(p => p.ID, slangs[0],slangs[1], slangsTwo[0]);
            var words = new List<EDCWord>();
            var wordCharacters = new List<string>();
            wordCharacters.Add("人");
            wordCharacters.Add("多");
            var wordMeanings = new List<string>();
            wordMeanings.Add("Human/Person");
            wordMeanings.Add("Many/Not Small");
            var wordPinyins = new List<string>();
            wordPinyins.Add("人.png");
            wordPinyins.Add("多.png");
            var wordAudios = new List<string>();
            wordAudios.Add("人.m4a");
            wordAudios.Add("多.m4a");

            words.Add(new EDCWord()
            {
                Character = wordCharacters[0],
                BasicMeanings = wordMeanings[0],
                Pinyin = wordPinyins[0],
                Phrases = phrases,
                Audio = wordAudios[0],
                Date = DateTime.Parse("2016-06-10").ToShortDateString(),
                Slangs = slangs,
                StudentName = "yov.max@gmail.com"
            });

            words.Add(new EDCWord()
            {
                Character = wordCharacters[1],
                BasicMeanings = wordMeanings[1],
                Pinyin = wordPinyins[1],
                Phrases = phrasesTwo,
                Audio = wordAudios[1],
                Date = DateTime.Parse("2016-06-11").ToShortDateString(),
                Slangs = slangsTwo,
                StudentName = "yov.max@gmail.com"
            });
            context.Words.AddOrUpdate(x => x.ID, words[0], words[1]);
            #endregion

            #region login student
            var user = new EDCStudent()
            {
                StudentName = "yov.max@gmail.com",
                Words = words,
                //  LearnRequests = learnRequests
            };
            context.Students.AddOrUpdate(x => x.StudentName, user);
            #endregion

            #region update learn request students
            foreach (var l in learnRequests)
            {
                l.RegisteredStudents.Add(user);
            }
            context.SaveChanges();
            #endregion

            #region login teacher
            var teacher = new EDCTeacher()
            {
                TeacherName = "xyov.max@gmail.com",
                LearnRequests = learnRequests
            };
            context.Teachers.AddOrUpdate(x => x.TeacherName, teacher);
            #endregion

            #region scenario
            var scenarioWords = new List<EDCScenarioWord>
            {
                new EDCScenarioWord{ID = 1, ImageId = 1, ChineseWord = "我是",ChineseWordPinyin="我是.png",ChineseWordAudio="我是.m4a"},
                new EDCScenarioWord{ID = 2, ImageId = 1, ChineseWord = "会面",ChineseWordPinyin="会面.png",ChineseWordAudio="会面.m4a"},
                new EDCScenarioWord{ID = 3, ImageId = 2, ChineseWord = "到达",ChineseWordPinyin="到达.png",ChineseWordAudio="到达.m4a"},
                new EDCScenarioWord{ID = 4, ImageId = 3, ChineseWord = "国家",ChineseWordPinyin="国家.png",ChineseWordAudio="国家.m4a"}
            };
            context.ScenarioWords.AddOrUpdate(p => p.ID, scenarioWords[0], scenarioWords[1], scenarioWords[2], scenarioWords[3]);
            var images = new List<EDCScenarioImage>
            {
                new EDCScenarioImage{ID = 1, Image = "scenariofirst.png"},
                new EDCScenarioImage{ID = 2, Image = "scenariofirst.png"},
                new EDCScenarioImage{ID = 3, Image = "scenariofirst.png"},
                new EDCScenarioImage{ID = 4, Image = "scenariofirst.png"}
            };

            context.ScenarioImages.AddOrUpdate(p => p.ID, images[0], images[1], images[2], images[3]);

            var scene = new EDCScenarioContent
            {
                ID = 1,
                Date = DateTime.Parse("2016-06-11").ToShortDateString(),
                ThemeChinese = "如果你的家人或者朋友生病了，你想要表达一下你的关心",
                ThemeEnglish = "If your family or friend gets sick, you wants to offer your sympathy",
                Images = images
            };
            context.Scenarios.AddOrUpdate(p => p.ID, scene);
            #endregion
        }
    }
}
