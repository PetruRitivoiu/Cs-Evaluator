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

            var homeworks = new HomeworkEntity[]
            {
                new HomeworkEntity{FullPath="C:\\Users\\thinkpad-e560\\Documents\\Visual Studio 2017\\Projects\\cs-evaluator\\Cs-Evaluator\\wwwroot\\uploads\\homework_1", Subject=subjects[0]},
                new HomeworkEntity{FullPath="C:\\Users\\thinkpad-e560\\Documents\\Visual Studio 2017\\Projects\\cs-evaluator\\Cs-Evaluator\\wwwroot\\uploads\\homework_2", Subject=subjects[1]}
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
            _context.Homeworks.AddRange(homeworks);
            _context.StudentSubjects.AddRange(studentSubjectRelationship);
            _context.StudentHomeworks.AddRange(studentHomeworkRelationship);

            await _context.SaveChangesAsync();
        }
    }
}
