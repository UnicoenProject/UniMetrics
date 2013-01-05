using Unicoen.Languages.Ruby18.Model;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.Strategy
{
    public class RubyStrategy : AbstractLanguageStrategy
    {
        public RubyStrategy(string filePath)
        {
            FilePath = filePath;
        }

        public override string GetLanguage()
        {
            return "Python";
        }

        public override UnifiedProgram CreateCodeObject()
        {
            return new Ruby18ProgramGenerator().GenerateFromFile(FilePath);
        }

        public override string GetNamespaceType()
        {
            return "";
        }

        public override string GetClassType(bool isAbstract, bool isInterface)
        {
            return "class";
        }

        public override string GetMethodType(bool isConstructor, bool isReturn)
        {
            return "method";
        }
    }
}
