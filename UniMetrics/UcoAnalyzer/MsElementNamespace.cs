using System.Collections.Generic;

namespace Unicoen.Apps.UniMetrics.UcoAnalyzer
{
    public class MsElementNamespace
    {
        public string Name { get; set; }
        public string Type { get; set; } //namespace,package
        public List<MsElementClass> ListClass { get; set; }

        public MsElementNamespace()
        {
            this.Name = "";
            this.Type = "";
        }
    }
}
