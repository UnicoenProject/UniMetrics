using System;
using Unicoen.Languages.Java.ProgramGenerators;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.Strategy
{
    public class JavaStrategy : AbstractLanguageStrategy
    {
        public JavaStrategy(string filePath)
        {
            FilePath = filePath;
        }
        
        /*public override void Print()
        {
            base.Print();
            Console.WriteLine("this is Java strategy");
        }*/

        public override string GetLanguage()
        {
            return "Java";
        }

        public override UnifiedProgram CreateCodeObject()
        {
            return new JavaProgramGenerator().GenerateFromFile(FilePath);
        }

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
