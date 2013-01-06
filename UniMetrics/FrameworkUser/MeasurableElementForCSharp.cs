using Unicoen.Apps.UniMetrics.UcoAnalyzerComponent;

namespace Unicoen.Apps.UniMetrics.FrameworkUser
{
    public class MeasurableElementForCSharp : MeasurableElementGenerator
    {
        public override string GetNamespaceType()
        {
            return "namespace";
        }

        public override string GetClassType(bool isAbstract, bool isInterface)
        {
            if (isInterface) return "interface";
            return isAbstract ? "abstract" : "class";
        }

        public override string GetMethodType(bool isConstructor, bool isReturn)
        {
            return isConstructor ? "constructor" : "method";
        }
    }
}
