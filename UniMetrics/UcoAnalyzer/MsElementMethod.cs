using System.Collections.Generic;

namespace Unicoen.Apps.UniMetrics.UcoAnalyzer
{
    class MsElementMethod
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<MsElementMethodArgument> ListMethofArgument { get; set; }
        public List<string> ListMethodCall { get; set; } 
    }

    class MsElementMethodArgument
    {
        public string ArgName { get; set; }
        public string ArgType { get; set; }
    }

    class MsElementAttribute
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
