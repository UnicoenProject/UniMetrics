using System.Collections.Generic;

namespace Unicoen.Apps.UniMetrics.Utils
{
    public class MsElementClass 
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<MsElementMethod> ListMethod { get; set; }
        public List<MsElementAttribute> ListAttribute { get; set; }
        public string ClassParent { get; set; }
    }
}
