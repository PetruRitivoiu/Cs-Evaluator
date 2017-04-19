using CsEvaluator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.SqlHelper
{

    public class DbInitializer
    {
        private CsEvaluatorContext _context;

        public DbInitializer(CsEvaluatorContext context)
        {
            this._context = context;
        }

        public async Task Initialize()
        {
            _context.Database.EnsureCreated();

            // Look for any students.
            if (_context.Subjects.Any())
            {
                return;   // DB has been seeded
            }

            var students = new StudentEntity[]
            {
                new StudentEntity{Forename="Petru", Surname="Ritivoiu"},
                new StudentEntity{Forename="Horia", Surname="Popescu"}
            };

            var subjects = new SubjectEntity[]
            {
                new SubjectEntity{Name="BPC"},
                new SubjectEntity{Name="ATP"}
            };

            var homeworkDescriptions = new HomeworkDescriptionEntity[]
            {
                new HomeworkDescriptionEntity{Subject=subjects[0], fullname="Tema 1", shortDescription="Produs vectorial a 2 vectori",
                    fullDescription = "Sa se calculeze produsul vectorial a doi vectori de lungime 20",},
                new HomeworkDescriptionEntity{Subject=subjects[1], fullname="Tema 2", shortDescription="Inmultire doua matrici",
                    fullDescription = "Sa se inmulteasca 2 matrici de marime 3*3 si sa se afiseze rezultatul la consola"}
            };

            var homeworks = new HomeworkEntity[]
            {
                new HomeworkEntity{FullPath="MOCK", Subject=subjects[0], HomeworkDescription = homeworkDescriptions[0]},
                new HomeworkEntity{FullPath="MOCK", Subject=subjects[1], HomeworkDescription = homeworkDescriptions[1]}
            };

            var studentSubjectRelationship = new StudentSubjectRelationship[]
            {
                new StudentSubjectRelationship{Student=students[0], Subject=subjects[0]},
                new StudentSubjectRelationship{Student=students[1], Subject=subjects[1]}
            };

            var studentHomeworkRelationship = new StudentHomeworkRelationship[]
            {
                new StudentHomeworkRelationship{Student=students[0], Homework=homeworks[0]},
                new StudentHomeworkRelationship{Student=students[1], Homework=homeworks[1]}
            };

            _context.Students.AddRange(students);
            _context.Subjects.AddRange(subjects);
            _context.HomeworkDescriptions.AddRange(homeworkDescriptions);
            _context.Homeworks.AddRange(homeworks);
            _context.StudentSubjects.AddRange(studentSubjectRelationship);
            _context.StudentHomeworks.AddRange(studentHomeworkRelationship);

            await _context.SaveChangesAsync();
        }
    }
}
