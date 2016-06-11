using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EDCWebApp.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EDCWebApp.DAL
{
    public class EDCLoginUserContext : DbContext, IEDCLoginUserContext
    {
        public DbSet<EDCStudent> Students { get; set; }
        public DbSet<EDCWord> Words { get; set; }
        public DbSet<EDCLearnRequest> LearnRequests { get; set; }
        public DbSet<EDCScenarioContent> Scenarios { get; set; }
        public DbSet<EDCTeacher> Teachers { get; set; }
        public DbSet<EDCPhraseExample> PhraseExamples { get; set; }
        public DbSet<EDCPhrase> Phrases { get; set; }
        public DbSet<EDCHubConnection> HubConnections { get; set; }

        public DbSet<EDCQuote> Quotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<EDCStudent>()
                .HasMany(c => c.LearnRequests).WithMany(l => l.RegisteredStudents)
                .Map(m => m.MapLeftKey("StudentName")
                .MapRightKey("LearnRequestID")
                .ToTable("StudentLearnRequest"));
                
                
            base.OnModelCreating(modelBuilder);
        }



        public void SaveChangesToDb()
        {
            this.SaveChanges();
        }

        public async System.Threading.Tasks.Task<int> SaveChangesToDbAsync()
        {
            return await this.SaveChangesAsync();
        }

        public void SetEntityModified<T>(T entity) where T : class
        {
            this.Entry<T>(entity).State = EntityState.Modified;
        }
    }
}