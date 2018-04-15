using CsEvaluator.Engine.ReflectionEvaluator.Enums;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace CsEvaluator.Engine.ReflectionEvaluator.Rules
{
    public class IsRule : Rule
    {
        public override bool Evaluate(Assembly assembly)
        {
            IEnumerable<Type> types = assembly.GetTypes();

            types = BySubjectType(types);
            types = BySubjectName(types);
            types = ByComplementType(types);
            types = ByComplementValue(types);

            return types.Count() >= Count;
        }

        private IEnumerable<Type> BySubjectType(IEnumerable<Type> types)
        {
            switch (SubjectType)
            {
                case SubjectType.Class:
                    return types.Where(t => t.IsClass);

                case SubjectType.Interface:
                    return types.Where(t => t.IsInterface);

                default:
                    //log error
                    return null;
            }
        }

        private IEnumerable<Type> BySubjectName(IEnumerable<Type> types)
        {
            return IsNullOrDefault(SubjectValue) ? types : types.Where(t => t.Name == SubjectValue);
        }

        private IEnumerable<Type> ByComplementType(IEnumerable<Type> types)
        {
            if (ComplementType != ComplementType.NullOrDefault)
            {
                switch (ComplementType)
                {
                    case ComplementType.Abstract:
                        return types.Where(t => t.IsAbstract);

                    case ComplementType.Interface:
                        return types.Where(t => t.IsInterface);

                    case ComplementType.Enum:
                        return types.Where(t => t.IsEnum);

                    default:
                        //log error
                        return null;
                }
            }
            else
            {
                return types;
            }
        }

        private IEnumerable<Type> ByComplementValue(IEnumerable<Type> types)
        {
            if (!IsNullOrDefault(ComplementValue))
            {
                var desiredType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).First(x => x.Name == ComplementValue);

                return types.Where(t => desiredType.IsAssignableFrom(t));
            } else
            {
                return types;
            }
        }
    }
}
