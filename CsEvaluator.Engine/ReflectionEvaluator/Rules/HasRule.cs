using CsEvaluator.Engine.ReflectionEvaluator.Enums;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using CsEvaluator.Engine.Util;

namespace CsEvaluator.Engine.ReflectionEvaluator.Rules
{
    public class HasRule : Rule
    {
        public override RuleEvaluation Evaluate(Assembly assembly)
        {
            List<Type> types = assembly.GetTypes().ToList();

            int actualCount = int.MinValue;

            types = BySubjectType(types);
            types = BySubjectName(types);

            switch (ComplementType)
            {
                case ComplementType.Constructor:
                    actualCount = CheckAll(types, ByConstructor);
                    break;

                case ComplementType.Property:
                    actualCount = CheckAll(types, ByProperty);
                    break;

                case ComplementType.OperatorOverload:
                    actualCount = CheckAll(types, ByOperatorOverLoad);
                    break;

                case ComplementType.Event:
                    actualCount = CheckAll(types, ByEvent);
                    break;

                case ComplementType.Delegate:
                    actualCount = CheckAll(types, ByDelegate);
                    break;

                case ComplementType.Field:
                    actualCount = CheckAll(types, ByField);
                    break;
            }

            if (actualCount >= Count)
            {
                return new RuleEvaluation(new RuleInfo(this), true);
            }
            else
            {
                return new RuleEvaluation(new RuleInfo(this), false);
            }
        }

        private List<Type> BySubjectType(List<Type> types)
        {
            switch (SubjectType)
            {
                case SubjectType.Class:
                    return types.Where(t => t.IsClass).ToList();

                case SubjectType.Interface:
                    return types.Where(t => t.IsInterface).ToList();

                default:
                    return types;
            }
        }

        private List<Type> BySubjectName(List<Type> types)
        {
            return IsNullOrDefault(SubjectValue) ? types : types.Where(t => t.Name == SubjectValue).ToList();
        }


        //One to rule them all
        private int CheckAll(List<Type> types, Func<Type, int> function)
        {
            int count = 0;

            types.ForEach(t => count += function(t));

            return count;
        }


        //Atomic functions area
        private int ByConstructor(Type type)
        {
            //validation for auto-generated inner-classes for events
            if (UtilHelper.IsSameOrSubclass(typeof(MulticastDelegate), type))
            {
                return 0;
            }

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

        private int ByEvent(Type type)
        {
            int count = 0;
            var events = type.GetEvents();

            foreach (EventInfo e in events)
            {
                if (IsNullOrDefault(ComplementValue))
                {
                    count++;
                }
                else if (e.Name == ComplementValue)
                {
                    count++;
                }
            }

            return count;
        }

        private int ByDelegate(Type type)
        {
            int count = 0;
            var delegates = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                           .Where(t => t.BaseType == typeof(MulticastDelegate));

            foreach (var del in delegates)
            {
                if (IsNullOrDefault(ComplementValue))
                {
                    count++;
                }
                else if (del.Name == ComplementValue)
                {
                    count++;
                }
            }

            return count;
        }

        private int ByField(Type t)
        {
            int count = 0;
            var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

            switch (ComplementValue)
            {
                case nameof(Field.Array):
                    foreach (var field in fields)
                    {
                        var fieldType = field.FieldType;

                        //Check for string array
                        if (fieldType.IsArray)
                        {
                            count++;
                        }
                    }
                    break;
                case nameof(Field.NullOrDefault):
                    count = fields.Length;
                    break;
                default:
                    count = 0;
                    //log error
                    break;
            }

            return count;
        }

    }
}
