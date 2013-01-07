using System;
using Unicoen.Apps.UniMetrics.XmlGeneratorComponent;

namespace Unicoen.Apps.UniMetrics.FrameworkUser
{
    class AllSelector : ISelector
    {
        public XmlGenerator SelectXmlGenerator(string ext)
        {
            XmlGenerator xgen = null;
            switch (ext)
            {
                /*case ".c":
                    break;
                case ".cs":
                    break;*/
                case ".java":
                    xgen = new XmlGeneratorForJava();
                    break;
                case ".js":
                    xgen = new XmlGeneratorForJavaScript();
                    break;
                /*case ".py":
                    break;
                case ".rb":
                    break;*/
                default:
                    Console.WriteLine("Incorrect input file");
                    break;
            }
            return xgen;
        }
    }
}
