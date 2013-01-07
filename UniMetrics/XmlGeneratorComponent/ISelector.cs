using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicoen.Apps.UniMetrics.XmlGeneratorComponent
{
    public interface ISelector
    {
        XmlGenerator SelectXmlGenerator(string ext);
    }
}
