using System.Xml.Linq;
using Unicoen.Apps.UniMetrics.UcoAnalyzerComponent;
using Unicoen.Apps.UniMetrics.XmlGeneratorComponent;

namespace Unicoen.Apps.UniMetrics.FrameworkUser
{
    class XmlGeneratorForJava : XmlGenerator
    {
        public override XElement CreateFileDetails(string inFilePath)
        {
            return new XElement("file_info",
                       new XElement("file_name", inFilePath),
                       new XElement("language", "Java"));
        }

        public override MeasurableElementGenerator SelectMeasurableElementGenerator()
        {
            return new MeasurableElementForJava();
        }
    }
}
