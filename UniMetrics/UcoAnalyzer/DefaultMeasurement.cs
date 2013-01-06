﻿using System;
using System.IO;
using System.Linq;
using Unicoen.Model;

namespace Unicoen.Apps.UniMetrics.UcoAnalyzer
{
    public class DefaultMeasurement
    {
        /// <summary>
        /// Size measurement
        /// </summary>
        public int TotalLoc { get; set; }
        public int BlankLoc { get; set; }
        public int CommentLoc { get; set; }
        public int EffectiveLoc { get; set; }
        public int NumberOfStatement { get; set; }

        /// <summary>
        /// Complexity measurement
        /// </summary>
        public int CyclomaticComplexity { get; set; }
        public int NumberOfOperator { get; set; }
        public int NumberOfOperand { get; set; }

        /// <summary>
        /// set the measurement value into the object
        /// </summary>
        public void SetDefaultMeasurement(string FilePath)
        {
            SetLineBasedMetrics(FilePath);
            SetUcoBasedMetrics(FilePath);
        }

        /// <summary>
        /// set some metrics that are measured based on the code's line
        /// </summary>
        private void SetLineBasedMetrics(string FilePath)
        {
            string line;
            var total = 0;
            var blank = 0;
            var sr = new StreamReader(FilePath);
            while ((line = sr.ReadLine()) != null)
            {
                total++;
                if (line.Trim().Length == 0) blank++;
            }
            sr.Close();
            var srb = new StreamReader(FilePath);
            var arrb = srb.ReadToEnd().ToCharArray();
            var lastb = BitConverter.GetBytes(arrb[arrb.Length - 1]);
            if (lastb[0] == 10)
            {
                total++;
                blank++;
            }
            srb.Close();

            // total lines of code
            TotalLoc = total;

            // blank lines of code
            BlankLoc = blank;

            // effective lines of code
            EffectiveLoc = TotalLoc - BlankLoc - CommentLoc;
        }

        /// <summary>
        /// set some metrics that are measured based on analyzing the UCOs
        /// </summary>
        private void SetUcoBasedMetrics(string FilePath)
        {
            var codeObject = Utils.CodeAnalyzer.CreateCodeObject(FilePath);

            // comment lines of code
            try
            {
                var comment = 0;
                foreach (var c in codeObject.Comments)
                {
                    if (c.Position.StartPos == c.Position.EndPos)
                    {
                        comment += c.Position.EndLine - c.Position.StartLine;
                    }
                    else
                    {
                        comment += c.Position.EndLine - c.Position.StartLine + 1;
                    }
                }
                CommentLoc = comment;
            }
            catch (Exception e)
            {
                CommentLoc = 0;
            }

            // number of statements
            NumberOfStatement = codeObject.Descendants<UnifiedExpression>().Count(e => e.Parent is UnifiedBlock);

            // cyclomatic complexity
            var binaryDecision = codeObject.Descendants<UnifiedIf, UnifiedFor, UnifiedWhile, UnifiedDoWhile, UnifiedCase>();
            CyclomaticComplexity = binaryDecision.Count() + 1;

            // number of operand ??
            var opernd = codeObject.Descendants<UnifiedVariableIdentifier>();
            NumberOfOperand = opernd.Count();

            // number of operator ??
            var opertr = codeObject.Descendants<UnifiedUnaryOperator, UnifiedBinaryOperator>();
            NumberOfOperator = opertr.Count();
        }
    }
}
