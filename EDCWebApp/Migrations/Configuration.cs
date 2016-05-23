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
            context.Phrases.AddOrUpdate(x => x.ID, phrases[0], phrases[1], phrases[2], phrases[3]);

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
            context.Phrases.AddOrUpdate(x => x.ID, phrasesTwo[0], phrasesTwo[1]);

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
        }
    }
}
