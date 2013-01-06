using System;
using System.IO;
using System.Linq;
using Unicoen.Languages.C.ProgramGenerators;
using Unicoen.Languages.CSharp.ProgramGenerators;
using Unicoen.Languages.Java.ProgramGenerators;
using Unicoen.Languages.JavaScript.ProgramGenerators;
using Unicoen.Languages.Python2.ProgramGenerators;
using Unicoen.Languages.Ruby18.Model;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.Utils
{
    public static class CodeAnalyzer
    {
        public static bool IsValid (string filePath)
        {
            string[] extArray = { ".c", ".cs", ".java", ".js", ".py", ".rb" };
            return extArray.Any(s => GetFileExtension(filePath).Contains(s));
        }

        public static string GetFileExtension (string filePath)
        {
            return Path.GetExtension(filePath).ToLower();
        }

        public static UnifiedProgram CreateCodeObject(string filePath)
        {
            UnifiedProgram codeObject = null;
            switch (GetFileExtension(filePath))
            {
                case ".c":
                    codeObject = new CProgramGenerator().GenerateFromFile(filePath);
                    break;
                case ".cs":
                    codeObject = new CSharpProgramGenerator().GenerateFromFile(filePath);
                    break;
                case ".java":
                    codeObject = new JavaProgramGenerator().GenerateFromFile(filePath);
                    break;
                case ".js":
                    codeObject = new JavaScriptProgramGenerator().GenerateFromFile(filePath);
                    break;
                case ".py":
                    codeObject = new Python2ProgramGenerator().GenerateFromFile(filePath);
                    break;
                case ".rb":
                    codeObject = new Ruby18ProgramGenerator().GenerateFromFile(filePath);
                    break;
                default:
                    Console.WriteLine("Incorrect input file");
                    break;
            }
            return codeObject;
        }
    }
}