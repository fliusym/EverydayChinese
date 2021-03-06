﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDCWebApp.DAL;
using System.Data.Entity;
using EDCWebApp.Models;

namespace EDCWebApp.Tests
{
    public class TestAppContext : IEDCLoginUserContext
    {
        public TestAppContext()
        {
            this.Words = new TestWordDbSet();
            this.Students = new TestStudentDbSet();
        }
        public DbSet<EDCWord> Words { get; set; }
        public DbSet<EDCStudent> Students { get; set; }
        public DbSet<EDCTeacher> Teachers { get; set; }
        public DbSet<EDCScenarioContent> Scenarios { get; set; }
        public DbSet<EDCLearnRequest> LearnRequests { get; set; }
        public DbSet<EDCPhrase> Phrases { get; set; }
        public DbSet<EDCPhraseExample> PhraseExamples { get; set; }
        public DbSet<EDCSlang> Slangs { get; set; }
        public DbSet<EDCScenarioImage> ScenarioImages { get; set; }
        public DbSet<EDCScenarioWord> ScenarioWords { get; set; }

        public void SaveChangesToDb()
        {
            return;
        }

        public Task<int> SaveChangesToDbAsync()
        {
            return Task.FromResult(0);
        }

        public void SetEntityModified<T>(T entity) where T : class
        {
            
        }
        public void RunCommand(string command, params object[] parameters)
        {
            
        }
        public void Dispose()
        {
            
        }
    }
}
