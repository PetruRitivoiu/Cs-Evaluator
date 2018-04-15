using System;
using System.Xml.Linq;
using System.Collections.Generic;
using CsEvaluator.Engine.ReflectionEvaluator.Rules;
using CsEvaluator.Engine.ReflectionEvaluator.Enums;

namespace CsEvaluator.Engine.FileParser
{
    public class XmlParser : IParser
    {
        public List<Rule> ParseToList(string fullPath)
        {
            var doc = XDocument.Load(fullPath);
            var rules = new List<Rule>();

            foreach (XElement element in doc.Root.Elements("rule"))
            {
                try
                {
                    var verb = ParseEnumIgnoreCase<Verb>(element.Element("verb").Value.Trim());
                    var rule = Rule.CreateByVerb(verb);

                    rule.Id = Int32.Parse(element.Attribute("id").Value.Trim());
                    rule.SubjectType = ParseEnumIgnoreCase<SubjectType>(element.Element("subject-type").Value.Trim());
                    rule.SubjectValue = element.Element("subject-value").Value.Trim();
                    rule.Verb = verb;
                    rule.ComplementType = ParseEnumIgnoreCase<ComplementType>(element.Element("complement-type").Value.Trim());
                    rule.ComplementValue = element.Element("complement-value").Value.Trim();
                    rule.Count = Int32.Parse(element.Element("count").Value.Trim());

                    rules.Add(rule);
                }
                catch(Exception ex)
                {
                    //log error
                    continue;
                }
            }

            return rules;
        }

        private T ParseEnumIgnoreCase<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
        }
    }
}
