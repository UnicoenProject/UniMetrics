using Unicoen.Apps.UniMetrics.UcoAnalyzer;

namespace Unicoen.Apps.UniMetrics.FrameworkUser
{
    public class PythonStrategy : MeasurableElementGenerator
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
