using Unicoen.Apps.UniMetrics.UcoAnalyzer;

namespace Unicoen.Apps.UniMetrics.FrameworkUser
{
    public class MeasurableElementForJava : MeasurableElementGenerator
    {
        public override string GetNamespaceType()
        {
            return "package";
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
