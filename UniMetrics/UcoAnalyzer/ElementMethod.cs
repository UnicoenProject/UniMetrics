using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicoen.Apps.Loc.UcoAnalyzer
{
    class ElementMethod
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<ElementMethodArgument> ListMethofArgument { get; set; }
        public List<string> ListMethodCall { get; set; } 
    }

    class ElementMethodArgument
    {
        public string ArgName { get; set; }
        public string ArgType { get; set; }
    }

    class ElementAttribute
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
