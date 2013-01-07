using System.Linq;
using System.Xml.Linq;
using Unicoen.Apps.UniMetrics.UcoAnalyzerComponent;
using Unicoen.Apps.UniMetrics.Utils;

namespace Unicoen.Apps.UniMetrics.XmlGeneratorComponent
{
    public abstract class XmlGenerator
    {
        /// <summary>
        /// create XML Element for file details
        /// the "Primitive Operations"
        /// </summary>
        public abstract XElement CreateFileDetails(string inFilePath);

        /// <summary>
        /// set which measurable element generator will be used
        /// the "Primitive Operations"
        /// </summary>
        public abstract MeasurableElementGenerator SelectMeasurableElementGenerator();

        protected XElement CreateSizeMetrics(DefaultMeasurement size)
        {
            return new XElement("size_metrics",
                       new XElement("total_lines", size.TotalLoc),
                       new XElement("blank_lines", size.BlankLoc),
                       new XElement("comment_lines", size.CommentLoc),
                       new XElement("effective_lines", size.EffectiveLoc),
                       new XElement("number_of_statement", size.NumberOfStatement));
        }

        protected XElement CreateComplexityMetrics(DefaultMeasurement comp)
        {
            return new XElement("complexity_metrics",
                       new XElement("complexity_metrics", comp.CyclomaticComplexity),
                       new XElement("number_of_operator", comp.NumberOfOperator),
                       new XElement("number_of_operand", comp.NumberOfOperand));
        }

        /// <summary>
        /// the "Template Method"
        /// </summary>
        public XDocument GenerateXmlDocument(string inFilePath/*, MeasurableElementGenerator measurableElementGenerator*/)
        {
            if (CodeAnalyzer.IsValid(inFilePath))
            {
                var defaultMeasurement = new DefaultMeasurement();
                defaultMeasurement.SetDefaultMeasurement(inFilePath);
                var measurableElementGenerator = SelectMeasurableElementGenerator();
                measurableElementGenerator.SetMeasurableElement(inFilePath);
                var xdoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", null),
                    new XElement("unimetrics",
                                 CreateFileDetails(inFilePath),
                                 CreateSizeMetrics(defaultMeasurement),
                                 CreateComplexityMetrics(defaultMeasurement),
                                 new XElement("measurable_element",
                                              measurableElementGenerator.ListElementNamespace.Select(
                                                  CreateXElementNamespace)))
                    );
                return xdoc;
            }
            return null;
        }

        #region Measurable Element XElement Generator
        
        private static XElement CreateXElementNamespace(MsElementNamespace elNsp)
        {
            return new XElement("namespace",
                       new XAttribute("name", elNsp.Name),
                       new XAttribute("type", elNsp.Type),
                       elNsp.ListClass.Select(CreateXElementClass));
        }

        private static XElement CreateXElementClass(MsElementClass elCls)
        {
            return new XElement("class",
                       new XElement("class_name", elCls.Name),
                       new XElement("class_type", elCls.Type),
                       new XElement("class_is_a_child_of", elCls.ClassParent),
                       elCls.ListMethod.Select(CreateXElementMethod),
                       elCls.ListAttribute.Select(CreateXElementAttribute));
        }

        private static XElement CreateXElementMethod(MsElementMethod elMet)
        {
            return new XElement("method",
                        new XElement("method_name", elMet.Name),
                        new XElement("method_type", elMet.Type),
                        elMet.ListMethofArgument.Select(CreateXElementMethodArgument),
                        elMet.ListMethodCall.Select(CreateXElementMethodCall));
        }
        private static XElement CreateXElementMethodArgument(MsElementMethodArgument elArg)
        {
            return new XElement("method_argument",
                       new XElement("method_argument_type", elArg.ArgType),
                       new XElement("method_argument_name", elArg.ArgName));
        }

        private static XElement CreateXElementMethodCall(string elCal)
        {
            return new XElement("method_calls_method", elCal);
        }

        private static XElement CreateXElementAttribute(MsElementAttribute elAtr)
        {
            return new XElement("attribute",
                       new XElement("attribute_type", elAtr.Type),
                       new XElement("attribute_name", elAtr.Name));
        }
        #endregion
    }
}

            /*
             XElement xmltree = 
                new XElement("unimetrics", 
                    fileInfo,
                    sizeMetrics,
                    complexityMetrics,
                new XElement("measurable_element",
                    from elNsp in meas.ListElementNamespace
                    select 
                        new XElement("namespace", new XAttribute("name", elNsp.Name), new XAttribute("type", elNsp.Type),
                        from elCls in elNsp.ListClass
                            select
                                new XElement("class",
                                new XElement("class_name", elCls.Name),
                                new XElement("class_type", elCls.Type),
                                new XElement("class_is_a_child_of", elCls.ClassParent),
                                from elMet in elCls.ListMethod
                                    select
                                        new XElement("method",
                                        new XElement("method_name", elMet.Name),
                                        new XElement("method_type", elMet.Type),
                                        from elArg in elMet.ListMethofArgument
                                            select
                                                new XElement("method_argument",
                                                new XElement("method_argument_type", elArg.ArgType),
                                                new XElement("method_argument_name", elArg.ArgName)),
                                        from elCal in elMet.ListMethodCall
                                        select
                                            new XElement("method_calls_method", elCal)),
                                from elAtr in elCls.ListAttribute
                                    select
                                        new XElement("attribute",
                                        new XElement("attribute_type", elAtr.Type),
                                        new XElement("attribute_name", elAtr.Name))))
                    )
               );
             */