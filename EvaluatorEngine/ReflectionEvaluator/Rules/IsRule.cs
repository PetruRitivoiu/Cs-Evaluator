using EvaluatorEngine.ReflectionEvaluator.Enums;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace EvaluatorEngine.ReflectionEvaluator.Rules
{
    public class IsRule : Rule
    {
        public override bool Evaluate(Assembly assembly)
        {
            IEnumerable<Type> types;

            switch (SubjectType)
            {
                case SubjectType.Class:
                    types = assembly.GetTypes().Where(t => t.IsClass);
                    break;

                case SubjectType.Interface:
                    types = assembly.GetTypes().Where(t => t.IsInterface);
                    break;

                default:
                    types = null;
                    //log error
                    break;
            }

            if (!IsNullOrDefault(SubjectValue))
            {
                types = types.Where(t => t.Name == SubjectValue);
            }

            if (ComplementType != ComplementType.NullOrDefault)
            {
                switch (ComplementType)
                {
                    case ComplementType.Abstract:
                        types = types.Where(t => t.IsAbstract);
                        break;

                    case ComplementType.Interface:
                        types = types.Where(t => t.IsInterface);
                        break;

                    case ComplementType.Enum:
                        types = types.Where(t => t.IsEnum);
                        break;
                }
            }

            if (!IsNullOrDefault(ComplementValue))
            {
                var desiredType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).First(x => x.Name == ComplementValue);

                types = types.Where(t => desiredType.IsAssignableFrom(t));
            }

            return types.Count() >= Count;

        }
    }
}
