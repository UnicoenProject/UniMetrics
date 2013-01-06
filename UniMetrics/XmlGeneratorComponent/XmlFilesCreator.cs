using System;
using System.IO;
using Unicoen.Apps.UniMetrics.UcoAnalyzerComponent;

namespace Unicoen.Apps.UniMetrics.XmlGeneratorComponent
{
    public class XmlFilesCreator
    {
        public string OutputDirectoryPath;
        public string InputDirectory;

        public XmlFilesCreator (string outdir)
        {
            OutputDirectoryPath = outdir;
        }

        public void SaveFiles(string inputPath, /*MeasurableElementGenerator msGenerator,*/ XmlGenerator xmlGenerator)
        {
            var input = File.GetAttributes(@inputPath);
            // if inputPath is a directory
            if ((input & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var dirPath = new DirectoryInfo(@inputPath);
                InputDirectory = inputPath;
                SaveForDirectory(dirPath, /*msGenerator,*/ xmlGenerator);
            }
            // if inputPath is a file
            else
            {
                var filPath = new FileInfo(@inputPath);
                InputDirectory = filPath.DirectoryName;
                SaveForFile(filPath, /*msGenerator,*/ xmlGenerator);
            }
        }

        private void SaveForDirectory(DirectoryInfo directory, /*MeasurableElementGenerator msGenerator,*/ XmlGenerator xmlGenerator)
        {
            var files = directory.GetFiles("*.*");
            foreach (FileInfo file in files)
            {   
                SaveForFile(file, /*msGenerator,*/ xmlGenerator);
            }
            var dirs = directory.GetDirectories("*.*");
            foreach (DirectoryInfo dir in dirs)
            {
                SaveForDirectory(dir, /*msGenerator,*/ xmlGenerator);
            }
        }

        private void SaveForFile(FileInfo file, /*MeasurableElementGenerator msGenerator,*/ XmlGenerator xmlGenerator)
        {
            var xdoc = xmlGenerator.GenerateXmlDocument(file.FullName/*, msGenerator*/);
            var xdir = file.Directory.ToString().Remove(0, InputDirectory.Length);
            var xname = xdir.Length == 0 ? file.Name : xdir.Replace("\\", "#") + "#" + file.Name;
            if (xdoc != null)
            {
                Console.WriteLine("Saving XML for " + xname);
                xdoc.Save(@OutputDirectoryPath + "\\" + xname + ".xml");
            }
            else
            {
                Console.WriteLine("Can not create XML for " + xname);
            }
        }

    }
}
