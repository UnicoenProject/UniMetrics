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

namespace Unicoen.Apps.UniMetrics.UcoAnalyzer
{
    class ComplexityMeasurement
    {
        public string FilePath { get; set; }
        public int CyclomaticComplexity { get; set; }
        public int NumberOfOperator { get; set; }
        public int NumberOfOperand { get; set; }

        public ComplexityMeasurement(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// set the measurement value into the object
        /// </summary>
        public void SetComplexityMeasurement()
        {
            UnifiedProgram codeObject = null;
            var ext = Path.GetExtension(this.FilePath);
            switch (ext.ToLower())
            {
                case ".c":
                    codeObject = new CProgramGenerator().GenerateFromFile(this.FilePath);
                    break;
                case ".cs":
                    codeObject = new CSharpProgramGenerator().GenerateFromFile(this.FilePath);
                    break;
                case ".java":
                    codeObject = new JavaProgramGenerator().GenerateFromFile(this.FilePath);
                    break;
                case ".js":
                    codeObject = new JavaScriptProgramGenerator().GenerateFromFile(this.FilePath);
                    break;
                case ".py":
                    codeObject = new Python2ProgramGenerator().GenerateFromFile(this.FilePath);
                    break;
                case ".rb":
                    codeObject = new Ruby18ProgramGenerator().GenerateFromFile(this.FilePath);
                    break;
                default:
                    Console.WriteLine("Incorrect input file");
                    break;
            }

            var binaryDecision = codeObject.Descendants<UnifiedIf, UnifiedFor, UnifiedWhile, UnifiedDoWhile, UnifiedCase>();
            this.CyclomaticComplexity = binaryDecision.Count() + 1;

            // still nedd to check
            var opernd = codeObject.Descendants<UnifiedVariableIdentifier>();
            this.NumberOfOperand = opernd.Count();
            var opertr = codeObject.Descendants<UnifiedUnaryOperator, UnifiedBinaryOperator>();
            this.NumberOfOperator = opertr.Count();
        }

        /// <summary>
        /// print the complexity measurement values
        /// </summary>
        public void PrintComplexityMeasurement()
        {
            Console.WriteLine("Cyclomatic Complexity     : " + this.CyclomaticComplexity);
            Console.WriteLine("Number of Operator     : " + this.NumberOfOperator);
            Console.WriteLine("Number of Operand   : " + this.NumberOfOperand);
        }
    }
}
