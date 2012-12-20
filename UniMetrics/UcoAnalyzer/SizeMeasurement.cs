using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unicoen.Languages.C.ProgramGenerators;
using Unicoen.Languages.CSharp.ProgramGenerators;
using Unicoen.Languages.Java.ProgramGenerators;
using Unicoen.Languages.JavaScript.ProgramGenerators;
using Unicoen.Languages.Python2.ProgramGenerators;
using Unicoen.Languages.Ruby18.Model;
using Unicoen.Model;

namespace Unicoen.Apps.Loc.UcoAnalyzer
{
    public class SizeMeasurement
    {
        public string FilePath { get; set; }
        public string Language { get; set; }
        public int TotalLoc { get; set; }
        public int BlankLoc { get; set; }
        public int CommentLoc { get; set; }
        public int EffectiveLoc { get; set; }
        public int NumberOfStatement { get; set; }

        public SizeMeasurement(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// set the measurement value into the object
        /// </summary>
        public void SetSizeMeasurement()
        {
            this.SetTotalLocBlankLoc();
            this.SetCommentStatement();
            this.EffectiveLoc = this.TotalLoc - this.BlankLoc - this.CommentLoc;
        }

        /// <summary>
        /// set the Total LOC and Blank LOC value into the object
        /// combined into one method because both read the file
        /// </summary>
        private void SetTotalLocBlankLoc()
        {
            string line;
            var total = 0;
            var blank = 0;
            var sr = new StreamReader(this.FilePath);
            while ((line = sr.ReadLine()) != null)
            {
                total++;
                if (line.Trim().Length == 0) blank++;
            }
            sr.Close();

            var srb = new StreamReader(this.FilePath);
            var arrb = srb.ReadToEnd().ToCharArray();
            var lastb = BitConverter.GetBytes(arrb[arrb.Length - 1]);
            if (lastb[0] == 10)
            {
                total++;
                blank++;
            }
            srb.Close();

            this.TotalLoc = total;
            this.BlankLoc = blank;
        }

        /// <summary>
        /// set the Comment LOC and Number of Statement value into the object
        /// combined into one method because both analyze the UCO
        /// </summary>
        private void SetCommentStatement()
        {
            UnifiedProgram codeObject = null;
            var ext = Path.GetExtension(this.FilePath);
            switch (ext.ToLower())
            {
                case ".c":
                    codeObject = new CProgramGenerator().GenerateFromFile(this.FilePath);
                    this.Language = "C";
                    break;
                case ".cs":
                    codeObject = new CSharpProgramGenerator().GenerateFromFile(this.FilePath);
                    this.Language = "C#";
                    break;
                case ".java":
                    codeObject = new JavaProgramGenerator().GenerateFromFile(this.FilePath);
                    this.Language = "Java";
                    break;
                case ".js":
                    codeObject = new JavaScriptProgramGenerator().GenerateFromFile(this.FilePath);
                    this.Language = "JavaScript";
                    break;
                case ".py":
                    codeObject = new Python2ProgramGenerator().GenerateFromFile(this.FilePath);
                    this.Language = "Python";
                    break;
                case ".rb":
                    codeObject = new Ruby18ProgramGenerator().GenerateFromFile(this.FilePath);
                    this.Language = "Ruby";
                    break;
                default:
                    Console.WriteLine("Incorrect input file");
                    break;
            }

            if (codeObject.Comments != null)
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
                this.CommentLoc = comment;
            }
            else
            {
                this.CommentLoc = 0;
            }
            this.NumberOfStatement = codeObject.Descendants<UnifiedExpression>().Count(e => e.Parent is UnifiedBlock);
            //Console.WriteLine(codeObject.ToXml());
        }

        /// <summary>
        /// print the size measurement values
        /// </summary>
        public void PrintSizeMeasurement()
        {
            Console.WriteLine("Total Lines of Code     : " + this.TotalLoc);
            Console.WriteLine("Blank Lines of Code     : " + this.BlankLoc);
            Console.WriteLine("Comment Lines of Code   : " + this.CommentLoc);
            Console.WriteLine("Effective Lines of Code : " + this.EffectiveLoc);
            Console.WriteLine("Number of Statements    : " + this.NumberOfStatement);
        }
    }
}
