﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnglishDX
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EngBaseEntities1 : DbContext
    {
        public EngBaseEntities1()
            : base("name=EngBaseEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Datum> Data { get; set; }
        public virtual DbSet<DayStat> DayStats { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TestResultInt> TestResultInts { get; set; }
        public virtual DbSet<Duration> Durations { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
    }
}
