using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicoen.Apps.Loc.UcoAnalyzer
{
    class MsElementClass 
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<MsElementMethod> ListMethod { get; set; }
        public List<MsElementAttribute> ListAttribute { get; set; }
        public string ClassParent { get; set; }
    }
}
