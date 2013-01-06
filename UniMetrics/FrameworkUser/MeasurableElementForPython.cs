using Unicoen.Apps.UniMetrics.UcoAnalyzerComponent;

namespace Unicoen.Apps.UniMetrics.FrameworkUser
{
    public class MeasurableElementForPython : MeasurableElementGenerator
    {
        public override string GetNamespaceType()
        {
            return "";
        }

        public override string GetClassType(bool isAbstract, bool isInterface)
        {
            return isAbstract? "abstract" : "class";
        }

        public override string GetMethodType(bool isConstructor, bool isReturn)
        {
            return "method";
        }
    }
}
