using System;
using CoreSBBL.Logging.Models.DAL.GN;
using CoreSBBL.Logging.Models.DAL.TS;
using CoreSBBL.Logging.Models.TC.DAL;
using CoreSBShared.Universal.Infrastructure.EF;
using CoreSBShared.Universal.Infrastructure.Interfaces;
using CoreSBShared.Universal.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CoreSBBL.Logging.Infrastructure.TS
{
    public class LogsContextTC : DbContext
    {
        public LogsContextTC(DbContextOptions<LogsContextTC> options) : base(options)
        {
        }

        public virtual DbSet<LogsDALEf> Logging { get; set; }
        public virtual DbSet<LabelDalIntTc> LoggingLabel { get; set; }
        public virtual DbSet<LogsTagDALEfTc> LoggingTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RegisterModel<LogsDALEf>(modelBuilder, "Logs");
            RegisterModel<LabelDalIntTc>(modelBuilder, "LogsLabel");
            RegisterModel<LogsTagDALEfTc>(modelBuilder, "LogsTags");

            modelBuilder.Entity<LogsDALEf>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Loggings)
                .UsingEntity("LogsToTags",
                    l => l.HasOne(typeof(LogsTagDALEfTc)).WithMany().HasForeignKey("TagsId")
                        .HasPrincipalKey(nameof(LogsTagDALEfTc.Id)),
                    r => r.HasOne(typeof(LogsDALEf)).WithMany().HasForeignKey("LogId")
                        .HasPrincipalKey(nameof(LogsDALEf.Id)),
                    j => j.HasKey("LogId", "TagsId"));
        }

        private void RegisterModel<T>(ModelBuilder modelBuilder, string Name)
            where T : CoreDalint
        {
            modelBuilder.Entity<T>().ToTable(Name);
            modelBuilder.Entity<T>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<T>().HasKey(p => p.Id).HasName($"{Name}_Id");
            modelBuilder.Entity<T>().Property(p => p.Id).HasColumnName("Id");
        }
    }
}

//Generic ids
namespace CoreSBBL.Logging.Infrastructure.GN
{
    public class LogsContextGN : DbContext
    {
        public LogsContextGN(DbContextOptions<LogsContextGN> options) : base(options)
        {
        }
        
        public virtual DbSet<LoggingDalEfInt> LoggingGN { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoggingDalEfInt>().HasKey(s => s.Id);
            modelBuilder.Entity<LoggingDalEfInt>().ToTable("LogGN");
            modelBuilder.Entity<LoggingDalEfInt>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<LoggingDalEfInt>().Property(p => p.Id).HasColumnName("Id");

        }

        private void RegisterModel<T>(ModelBuilder modelBuilder, string Name)
            where T :  CoreDalGnInt
        {
            modelBuilder.Entity<T>().ToTable(Name);
            modelBuilder.Entity<T>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<T>().HasKey(p => p.Id).HasName($"{Name}_Id");
            modelBuilder.Entity<T>().Property(p => p.Id).HasColumnName("Id");
        }
    }
}


namespace CoreSBBL.Logging.Infrastructure.Generic
{
    public class LogsContextGeneric2 : DbContext
    {
        public LogsContextGeneric2(DbContextOptions<LogsContextGeneric2> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoggingGenericInt>().HasKey(s => s.Id);
            modelBuilder.Entity<LoggingGenericInt>().ToTable("LoggingGenericInt");
            modelBuilder.Entity<LoggingGenericInt>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<LoggingGenericInt>().Property(p => p.Id).HasColumnName("Id");
        }
    }

    public class LogsContextGeneric : DbContext
    {
        public LogsContextGeneric(DbContextOptions<LogsContextGeneric> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoggingGenericInt>().HasKey(s => s.Id);
            modelBuilder.Entity<LoggingGenericInt>().ToTable("LoggingGenericInt");
            modelBuilder.Entity<LoggingGenericInt>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<LoggingGenericInt>().Property(p => p.Id).HasColumnName("Id");

            modelBuilder.Entity<LoggingGenericGuid>().HasKey(s => s.Id);
            modelBuilder.Entity<LoggingGenericGuid>().ToTable("LoggingGenericGuid");
            modelBuilder.Entity<LoggingGenericGuid>().Property(p => p.Id).HasValueGenerator<CustomGuidGeneratorGuid>();
            // modelBuilder.Entity<LoggingGenericGuid>().Property(p => p.Id).ValueGeneratedOnAdd();
            // modelBuilder.Entity<LoggingGenericInt>().Property(p => p.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<LoggingGenericGuid>().Property(p => p.Id).HasColumnName("Id");
            
            modelBuilder.Entity<LoggingGenericString>().HasKey(s => s.Id);
            modelBuilder.Entity<LoggingGenericString>().ToTable("LoggingGenericString");
            modelBuilder.Entity<LoggingGenericString>().Property(p => p.Id).HasValueGenerator<CustomGuidGeneratorString>();
            modelBuilder.Entity<LoggingGenericString>().Property(p => p.Id).HasColumnName("Id");
        }
    }

    public class CustomGuidGeneratorString : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Guid.NewGuid().ToString();
    }
    public class CustomGuidGeneratorGuid : ValueGenerator<Guid>
    {
        public override bool GeneratesTemporaryValues => false;

        public override Guid Next(EntityEntry entry)
            => Guid.NewGuid();
    }
}

