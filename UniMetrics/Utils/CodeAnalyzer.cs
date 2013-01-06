using System;
using System.IO;
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
        public static UnifiedProgram CreateCodeObject(string filePath)
        {
            UnifiedProgram codeObject = null;
            var ext = Path.GetExtension(filePath);
            switch (ext.ToLower())
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