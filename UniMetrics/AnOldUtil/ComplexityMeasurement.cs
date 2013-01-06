using System;
using System.Linq;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.OldUtil
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
            var codeObject = Utils.CodeAnalyzer.CreateCodeObject(FilePath);
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
