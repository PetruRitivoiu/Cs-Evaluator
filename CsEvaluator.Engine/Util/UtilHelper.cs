using System;
using System.Collections.Generic;
using System.Text;

namespace CsEvaluator.Engine.Util
{
    public static class UtilHelper
    {
        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        public static string ChangeVirtualExtension(string file, string extension)
        {
            if (extension[0] != '.')
            {
                extension = "." + extension;
            }

            return file.Remove(file.LastIndexOf(".")) + extension;
        }

        public static string RemoveVirtualExtension(string file)
        {
            return file.Remove(file.LastIndexOf("."));
        }
    }
}
