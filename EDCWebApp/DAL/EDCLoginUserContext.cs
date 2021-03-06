﻿using System;
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

        public DbSet<EDCSlang> Slangs { get; set; }
        public DbSet<EDCScenarioImage> ScenarioImages { get; set; }
        public DbSet<EDCScenarioWord> ScenarioWords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<EDCStudent>()
                .HasMany(c => c.LearnRequests).WithMany(l => l.RegisteredStudents)
                .Map(m => m.MapLeftKey("StudentName")
                .MapRightKey("LearnRequestID")
                .ToTable("StudentLearnRequest"));
            modelBuilder.Entity<EDCStudent>()
                .HasMany(c => c.Words).WithMany(w => w.Students)
                .Map(m => m.MapLeftKey("StudentName")
                    .MapRightKey("WordID")
                    .ToTable("StudentWord"));
            modelBuilder.Entity<EDCStudent>()
                .HasMany(c => c.Scenarios).WithMany(w => w.Students)
                .Map(m => m.MapLeftKey("StudentName")
                    .MapRightKey("ScenarioID")
                    .ToTable("StudentScenario"));
                
                
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

        public void RunCommand(string command,params object[] parameters)
        {
            Database.ExecuteSqlCommand(command, parameters);
        }
    }
}