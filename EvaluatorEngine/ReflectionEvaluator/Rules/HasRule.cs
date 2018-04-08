using EvaluatorEngine.ReflectionEvaluator.Enums;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace EvaluatorEngine.ReflectionEvaluator.Rules
{
    public class HasRule : Rule
    {
        public override bool Evaluate(Assembly assembly)
        {
            List<Type> types = assembly.GetTypes().ToList();
            int actualCount = int.MinValue;

            types = BySubjectType(types);
            types = BySubjectName(types);

            switch (ComplementType)
            {
                case ComplementType.Constructor:
                    actualCount = CheckForAll(types, ByConstructor);
                    break;

                case ComplementType.Property:
                    actualCount = CheckForAll(types, ByProperty);
                    break;

                case ComplementType.OperatorOverload:
                    actualCount = CheckForAll(types, ByOperatorOverLoad);
                    break;
            }



            return actualCount == Count;
        }

        private List<Type> BySubjectType(List<Type> types)
        {
            switch (SubjectType)
            {
                case SubjectType.Class:
                    return types.Where(t => t.IsClass);

                case SubjectType.Interface:
                    return types.Where(t => t.IsInterface);

                default:
                    return types;
            }
        }

        private List<Type> BySubjectName(List<Type> types)
        {
            return IsNullOrDefault(SubjectValue) ? types : types.Where(t => t.Name == SubjectValue);
        }


        //One to rule them all
        private int CheckForAll(List<Type> types, Func<Type, int> function)
        {
            int count = 0;

            types.ForEach(t => count += function(t));

            return count;
        }


        //Atomic functions area
        private int ByConstructor(Type type)
        {
            int count = 0;
            var ctors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            switch (ComplementValue)
            {
                case nameof(Constructor.NoArgs):
                    foreach (ConstructorInfo ctor in ctors)
                    {
                        if (ctor.GetParameters().Count() == 0)
                        {
                            count++;
                        }
                    }
                    break;

                case nameof(Constructor.WithArgs):
                    foreach (ConstructorInfo ctor in ctors)
                    {
                        if (ctor.GetParameters().Count() > 0)
                        {
                            count++;
                        }
                    }
                    break;
            }

            return count;
        }

        private int ByProperty(Type type)
        {
            int count = 0;
            var props = type.GetProperties();

            switch (ComplementValue)
            {
                case nameof(Property.Auto):
                    foreach (PropertyInfo prop in props)
                    {
                        if (prop.IsAutoProperty())
                        {
                            count++;
                        }
                    }
                    break;

                case nameof(Property.BackingField):
                    foreach (PropertyInfo prop in props)
                    {
                        if (!prop.IsAutoProperty())
                        {
                            count++;
                        }
                    }
                    break;
            }

            return count;
        }

        private int ByOperatorOverLoad(Type type)
        {
            int count = 0;
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (IsNullOrDefault(ComplementValue))
                {
                    if (method.Name.StartsWith("op_"))
                    {
                        count++;
                    }
                }
                else
                {
                    if (method.Name == ComplementValue)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
