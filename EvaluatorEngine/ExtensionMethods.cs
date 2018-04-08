using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EvaluatorEngine
{
    public static class ExtensionMethods
    {
        public static bool IsAutoProperty(this PropertyInfo prop)
        {
            if (!prop.CanWrite || !prop.CanRead)
            	return false;

            return prop.DeclaringType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                     .Any(f => f.Name.Contains("<" + prop.Name + ">"));
        }
    }
}
