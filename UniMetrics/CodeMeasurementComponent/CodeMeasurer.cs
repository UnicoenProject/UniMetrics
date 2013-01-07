using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Unicoen.Apps.UniMetrics.CodeMeasurementComponent
{
    public abstract class CodeMeasurer
    {
        protected string FilePath { get; set; }

        public virtual void PrintMetrics(XDocument xDoc)
        {
            IEnumerable<string> enumString = GetMethodsFromClass("TestPageResult", xDoc);
            foreach (var e in enumString)
            {
                Console.WriteLine(e);
            }
        }

        public virtual IEnumerable<string> GetMethodsFromClass(string className, XDocument xDoc)
        {
            IEnumerable<string> met =
                                from item in xDoc.Descendants("method")
                                where (item.Parent.Element("class_name").Value.Equals(className))
                                select 
                                    item.Element("method_type").Value
                                    + "::" + item.Element("method_name").Value;
            return met;
        }

        public virtual Dictionary<string, string> GetSizeMeasurement(XDocument xDoc)
        {
            return GetMeasurement(xDoc, "size_metrics");
        }

        public virtual Dictionary<string, string> GetComplexityMeasurement(XDocument xDoc)
        {
            return GetMeasurement(xDoc, "complexity_metrics");
        }

        private static Dictionary<string, string> GetMeasurement(XDocument xDoc, string name)
        {
            Dictionary<string, string> dataDictionary = null;
            foreach (var elem in xDoc.Descendants(name))
            {
                dataDictionary = elem.Descendants().ToDictionary(
                                      element => element.Name.LocalName, 
                                      element => element.Value);
            }
            return dataDictionary;
        }

        public void PrintDictionary(Dictionary<string, string> result)
        {
            foreach (var i in result)
            {
                Console.WriteLine("key: {0}, value: {1}", i.Key, i.Value);
            }
        }
    }
}
