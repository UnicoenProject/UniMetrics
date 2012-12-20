using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicoen.Apps.Loc.UcoAnalyzer
{
    class ElementClass 
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<ElementMethod> ListMethod { get; set; }
        public List<ElementAttribute> ListAttribute { get; set; }
        public string ClassParent { get; set; }
    }
}
