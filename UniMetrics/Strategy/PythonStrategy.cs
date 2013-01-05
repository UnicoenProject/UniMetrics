using Unicoen.Languages.Python2.ProgramGenerators;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.Strategy
{
    public class PythonStrategy : AbstractLanguageStrategy
    {
        public PythonStrategy(string filePath)
        {
            FilePath = filePath;
        }

        public override string GetLanguage()
        {
            return "Python";
        }

        public override UnifiedProgram CreateCodeObject()
        {
            return new Python2ProgramGenerator().GenerateFromFile(FilePath);
        }

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
