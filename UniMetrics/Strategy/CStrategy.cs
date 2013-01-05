using Unicoen.Languages.C.ProgramGenerators;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.Strategy
{
    public class CStrategy : AbstractLanguageStrategy
    {
        public CStrategy(string filePath)
        {
            FilePath = filePath;
        }

        public override string GetLanguage()
        {
            return "C";
        }

        public override UnifiedProgram CreateCodeObject()
        {
            return new CProgramGenerator().GenerateFromFile(FilePath);
        }

        public override string GetNamespaceType()
        {
            return "";
        }

        public override string GetClassType(bool isAbstract, bool isInterface)
        {
            return "module";
        }

        public override string GetMethodType(bool isConstructor, bool isReturn)
        {
            return isReturn ? "function" : "procedure";
        }
    }
}
