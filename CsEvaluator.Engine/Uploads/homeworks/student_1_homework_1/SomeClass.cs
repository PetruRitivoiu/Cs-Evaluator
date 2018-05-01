using System.Linq;

namespace DemoProiectPAW
{
    public class SomeClass
    {
        public string GetShortFileName(string filename)
        {
            return filename.Split('\\').Last();
        }
    }
}
