using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace Unicoen.Apps.Loc.UcoAnalyzer
{
    class MeasurementXmlGenerator
    {
        public MeasurementXmlGenerator()
        {
            
        }

        public void GenerateXmlElement(string filePath)
        {
            var size = new SizeMeasurement(filePath);
            size.SetSizeMeasurement();
            var comp = new ComplexityMeasurement(filePath);
            comp.SetComplexityMeasurement();
            var meas = new MeasurableElement(filePath);
            meas.SetMeasurableElement();

            XElement xmltree = 
                new XElement("unimetrics",
                    new XElement("file_name", size.FilePath),
                    new XElement("language", size.Language),
                new XElement("size_metrics",
                    new XElement("total_lines", size.TotalLoc),
                    new XElement("blank_lines", size.BlankLoc),
                    new XElement("comment_lines", size.CommentLoc),
                    new XElement("effective_lines", size.EffectiveLoc),
                    new XElement("number_of_statement", size.NumberOfStatement)),
                new XElement("complexity_metrics",
                    new XElement("complexity_metrics", comp.CyclomaticComplexity),
                    new XElement("number_of_operator", comp.NumberOfOperator),
                    new XElement("number_of_operand", comp.NumberOfOperand)),
                new XElement("measurable_element",
                    from f in meas.ListElementNamespace
                    select 
                        new XElement("namespace", new XAttribute("name", f.Name), new XAttribute("type", f.Type),
                        from a in f.ListClass
                            select
                                new XElement("class",
                                new XElement("class_name", a.Name),
                                new XElement("class_type", a.Type),
                                new XElement("class_is_a_child_of", a.ClassParent),
                                from b in a.ListMethod
                                    select
                                        new XElement("method",
                                        new XElement("method_name", b.Name),
                                        new XElement("method_type", b.Type),
                                        from c in b.ListMethofArgument
                                            select
                                                new XElement("method_argument",
                                                new XElement("method_argument_type", c.ArgType),
                                                new XElement("method_argument_name", c.ArgName)),
                                        from c in b.ListMethodCall
                                        select
                                            new XElement("method_calls_method", c)),
                                from d in a.ListAttribute
                                    select
                                        new XElement("attribute",
                                        new XElement("attribute_type", d.Type),
                                        new XElement("attribute_name", d.Name))))
                    )
               );
            Console.WriteLine(xmltree);
            //return xel;
        }
    }
}
