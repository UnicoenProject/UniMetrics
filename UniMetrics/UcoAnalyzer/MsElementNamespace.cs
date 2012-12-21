using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicoen.Apps.Loc.UcoAnalyzer
{
    class MsElementNamespace
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
