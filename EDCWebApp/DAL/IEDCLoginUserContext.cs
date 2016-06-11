using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EDCWebApp.Models;


namespace EDCWebApp.DAL
{
    public interface IEDCLoginUserContext : IDisposable
    {
        DbSet<EDCStudent> Students { get; }
        DbSet<EDCTeacher> Teachers { get; }
        DbSet<EDCLearnRequest> LearnRequests { get; }
        DbSet<EDCWord> Words { get;  }
        DbSet<EDCScenarioContent> Scenarios { get; }
        DbSet<EDCPhraseExample> PhraseExamples { get; set; }
        DbSet<EDCPhrase> Phrases { get; set; }
        DbSet<EDCQuote> Quotes { get; }

        void SaveChangesToDb();
        Task<int> SaveChangesToDbAsync();
        void SetEntityModified<T>(T entity) where T : class;
    }
}
