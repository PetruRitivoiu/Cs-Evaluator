using CsEvaluator.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CsEvaluator.SqlHelper { 

    public class CsEvaluatorContext : DbContext
    {
        private IConfigurationRoot _config;

        /*
        public CsEvaluatorContext(IConfigurationRoot config, DbContextOptions options)
            : base(options)
        {
            _config = config;
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentHomeworkRelationship>()
                .HasKey(shr => new { shr.StudentID, shr.HomeworkID });

            modelBuilder.Entity<StudentSubjectRelationship>()
                .HasKey(shr => new { shr.StudentID, shr.SubjectID });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(/*_config["ConnectionStrings:CsEvaluator"]*/ "Server=(localdb)\\MSSQLLocalDb;Database=CsEvaluatorDatabase;Trusted_Connection=true;MultipleActiveResultSets=true");
        }

        public DbSet<HomeworkEntity> Homeworks { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<StudentSubjectRelationship> StudentSubjects { get; set; }
        public DbSet<StudentHomeworkRelationship> StudentHomeworks { get; set; }
    }
}
