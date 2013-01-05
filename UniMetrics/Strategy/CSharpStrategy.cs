using Unicoen.Languages.CSharp.ProgramGenerators;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.Strategy
{
    public class CSharpStrategy : AbstractLanguageStrategy
    {
        public CSharpStrategy(string filePath)
        {
            FilePath = filePath;
        }

        public override string GetLanguage()
        {
            return "CSharp";
        }

        public override UnifiedProgram CreateCodeObject()
        {
            return new CSharpProgramGenerator().GenerateFromFile(FilePath);
        }

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
