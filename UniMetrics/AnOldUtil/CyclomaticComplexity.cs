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

namespace Unicoen.Apps.Loc.Util
{
    class CyclomaticComplexity
    {
        public static int Count(string inputPath)
        {
            var attr = File.GetAttributes(@inputPath);
            // if inputPath is a directory
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var dirPath = new DirectoryInfo(@inputPath);
                return CountForDirectory(dirPath);
            }
            // if inputPath is a file
            else
            {
                return CountForFile(inputPath);
            }
        }

        private static int CountForDirectory(DirectoryInfo dirPath)
        {
            var sum = 0;
            var files = dirPath.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                var fi = file.FullName;
                var fiLoc = CountForFile(fi);
                sum += fiLoc;
                Console.WriteLine(fi + " | cc=" + fiLoc);
            }
            var dirs = dirPath.GetDirectories("*.*");
            foreach (DirectoryInfo dir in dirs)
            {
                sum += CountForDirectory(dir);
            }
            return sum;
        }

        private static int CountForFile(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            switch (ext.ToLower())
            {
                case ".c":
                    return -99;
                case ".cs":
                    return -99;
                case ".java":
                    return
                            CcNum(
                                    new JavaProgramGenerator().GenerateFromFile(
                                            filePath));
                case ".js":
                    return -99;
                case ".py":
                    return -99;
                case ".rb":
                    return -99;
                default:
                    return 0;
            }
        }

        private static int CcNum(UnifiedProgram codeObject)
        {
            var bd = codeObject.Descendants<UnifiedIf, UnifiedFor, UnifiedWhile, UnifiedDoWhile, UnifiedCase>();
            return bd.Count() + 1;
        }
    }
}
