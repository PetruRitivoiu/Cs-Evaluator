using System.Linq;
using System.Threading.Tasks;
using CsEvaluator.Data.Entities;

namespace CsEvaluator.Database.SqlHelper
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
                new SubjectEntity{Name="PAW"},
            };

            var homeworkDescriptions = new HomeworkDescriptionEntity[]
            {
                new HomeworkDescriptionEntity{Subject=subjects[0], Name="Proiect PAW 2018", ShortDescription="Proiect PAW 2018",
                    FullDescription = "Cerinta proiectului poate fi gasita la adresa http://acs.ase.ro/paw" }
            };

            var homeworks = new HomeworkEntity[]
            {
                new HomeworkEntity{FileName="MOCK", HomeworkDescription = homeworkDescriptions[0]}
            };

            var studentSubjectRelationship = new StudentSubjectRelationship[]
            {
                new StudentSubjectRelationship{Student=students[0], Subject=subjects[0]},
                new StudentSubjectRelationship{Student=students[1], Subject=subjects[0]}
            };

            _context.Students.AddRange(students);
            _context.Subjects.AddRange(subjects);
            _context.HomeworkDescriptions.AddRange(homeworkDescriptions);
            _context.Homeworks.AddRange(homeworks);
            _context.StudentSubjects.AddRange(studentSubjectRelationship);

            await _context.SaveChangesAsync();
        }
    }
}
