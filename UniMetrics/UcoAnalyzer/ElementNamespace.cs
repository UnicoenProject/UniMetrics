using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicoen.Apps.Loc.UcoAnalyzer
{
    class ElementNamespace
    {
        public string Name { get; set; }
        public string Type { get; set; } //namespace,package
        public List<ElementClass> ListClass { get; set; }
    }
}
