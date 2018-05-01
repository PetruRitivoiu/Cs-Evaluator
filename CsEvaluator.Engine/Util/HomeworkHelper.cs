using System.IO;
using System.Linq;

namespace CsEvaluator.Engine.Util
{
    public static class HomeworkHelper
    {
        public static int InitFolder(string studentsHomeworkFolder, string teachersHomeworkFolder)
        {
            //ensure created
            Directory.CreateDirectory(studentsHomeworkFolder);

            //copy all .cs and .dll files from validation-files to homework folders
            string[] files = Directory.GetFiles(teachersHomeworkFolder);

            foreach (string file in files)
            {
                if (Path.GetExtension(file) == (".cs") || Path.GetExtension(file) == (".dll"))
                {
                    File.Copy(file, Path.Combine(studentsHomeworkFolder, GetShortFileName(file)), true);
                }
            }

            return files.Length;
        }

        private static string GetShortFileName(string filename)
        {
            return filename.Split("\\").Last();
        }
    }
}
