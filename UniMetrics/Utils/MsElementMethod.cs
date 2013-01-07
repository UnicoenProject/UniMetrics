using System.Collections.Generic;

namespace Unicoen.Apps.UniMetrics.Utils
{
    public class MsElementMethod
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<MsElementMethodArgument> ListMethofArgument { get; set; }
        public List<string> ListMethodCall { get; set; } 
    }

    public class MsElementMethodArgument
    {
        public string ArgName { get; set; }
        public string ArgType { get; set; }
    }

    public class MsElementAttribute
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
