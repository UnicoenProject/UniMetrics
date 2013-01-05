using Unicoen.Languages.JavaScript.ProgramGenerators;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.Strategy
{
    public class JavaScriptStrategy : AbstractLanguageStrategy
    {
        public JavaScriptStrategy(string filePath)
        {
            FilePath = filePath;
        }

        public override string GetLanguage()
        {
            return "JavaScript";
        }

        public override UnifiedProgram CreateCodeObject()
        {
            return new JavaScriptProgramGenerator().GenerateFromFile(FilePath);
        }

        public override string GetNamespaceType()
        {
            return "";
        }

        public override string GetClassType(bool isAbstract, bool isInterface)
        {
            return "";
        }

        public override string GetMethodType(bool isConstructor, bool isReturn)
        {
            return "function";
        }
    }
}
