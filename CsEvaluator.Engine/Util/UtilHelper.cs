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
    }
}
