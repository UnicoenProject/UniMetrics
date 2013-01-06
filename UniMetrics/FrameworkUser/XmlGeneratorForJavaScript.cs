using System.Xml.Linq;
using Unicoen.Apps.UniMetrics.UcoAnalyzerComponent;
using Unicoen.Apps.UniMetrics.XmlGeneratorComponent;

namespace Unicoen.Apps.UniMetrics.FrameworkUser
{
    class XmlGeneratorForJavaScript : XmlGenerator
    {
        public override XElement CreateFileDetails(string inFilePath)
        {
            return new XElement("file_info",
                       new XElement("file_name", inFilePath),
                       new XElement("language", "JavaScript"));
        }

        public override MeasurableElementGenerator GetMeasurableElementGenerator()
        {
            return new MeasurableElementForJavaScript();
        }
    }
}
