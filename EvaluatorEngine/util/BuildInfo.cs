using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluatorEngine.util
{
    public class BuildInfo
    {
        public bool Succes { get; private set; }
        public string Info { get; private set; }

        public BuildInfo(bool succes, string info)
        {
            Succes = succes;
            Info = info;
        }
    }
}
