using System;
using System.Linq;

namespace DemoProiectPAW
{
    public class DemoTesting : IDemoTesting
    {
        public string GetShortFileName(string filename)
        {
            return filename.Split('\\').Last();
        }

        public bool IsURLValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
