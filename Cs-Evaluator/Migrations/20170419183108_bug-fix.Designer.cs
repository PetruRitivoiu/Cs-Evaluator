using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CsEvaluator.SqlHelper;

namespace CsEvaluator.Migrations
{
    [DbContext(typeof(CsEvaluatorContext))]
    [Migration("20170419183108_bug-fix")]
    partial class bugfix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CsEvaluator.Entities.HomeworkDescriptionEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("SubjectID");

                    b.Property<string>("fullDescription");

                    b.Property<string>("fullname");

                    b.Property<string>("shortDescription");

                    b.HasKey("ID");

                    b.HasIndex("SubjectID");

                    b.ToTable("HomeworkDescriptions");
                });

            modelBuilder.Entity("CsEvaluator.Entities.HomeworkEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullPath");

                    b.Property<int?>("HomeworkDescriptionID");

                    b.Property<int?>("SubjectID");

                    b.HasKey("ID");

                    b.HasIndex("HomeworkDescriptionID");

                    b.HasIndex("SubjectID");

                    b.ToTable("Homeworks");
                });

            modelBuilder.Entity("CsEvaluator.Entities.StudentEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Forename");

                    b.Property<string>("Surname");

                    b.HasKey("ID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("CsEvaluator.Entities.StudentHomeworkRelationship", b =>
                {
                    b.Property<int>("StudentID");

                    b.Property<int>("HomeworkID");

                    b.HasKey("StudentID", "HomeworkID");

                    b.HasIndex("HomeworkID");

                    b.ToTable("StudentHomeworks");
                });

            modelBuilder.Entity("CsEvaluator.Entities.StudentSubjectRelationship", b =>
                {
                    b.Property<int>("StudentID");

                    b.Property<int>("SubjectID");

                    b.HasKey("StudentID", "SubjectID");

                    b.HasIndex("SubjectID");

                    b.ToTable("StudentSubjects");
                });

            modelBuilder.Entity("CsEvaluator.Entities.SubjectEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("CsEvaluator.Entities.HomeworkDescriptionEntity", b =>
                {
                    b.HasOne("CsEvaluator.Entities.SubjectEntity", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectID");
                });

            modelBuilder.Entity("CsEvaluator.Entities.HomeworkEntity", b =>
                {
                    b.HasOne("CsEvaluator.Entities.HomeworkDescriptionEntity", "HomeworkDescription")
                        .WithMany()
                        .HasForeignKey("HomeworkDescriptionID");

                    b.HasOne("CsEvaluator.Entities.SubjectEntity", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectID");
                });

            modelBuilder.Entity("CsEvaluator.Entities.StudentHomeworkRelationship", b =>
                {
                    b.HasOne("CsEvaluator.Entities.HomeworkEntity", "Homework")
                        .WithMany("StudentHomeworkRelationship")
                        .HasForeignKey("HomeworkID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CsEvaluator.Entities.StudentEntity", "Student")
                        .WithMany("StudentHomeworkRelationship")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CsEvaluator.Entities.StudentSubjectRelationship", b =>
                {
                    b.HasOne("CsEvaluator.Entities.StudentEntity", "Student")
                        .WithMany("StudentSubjectRelationship")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CsEvaluator.Entities.SubjectEntity", "Subject")
                        .WithMany("StudentSubjectRelationship")
                        .HasForeignKey("SubjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
