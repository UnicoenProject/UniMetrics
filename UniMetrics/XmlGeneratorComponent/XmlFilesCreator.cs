using System;
using System.IO;
using Unicoen.Apps.UniMetrics.UcoAnalyzerComponent;

namespace Unicoen.Apps.UniMetrics.XmlGeneratorComponent
{
    public class XmlFilesCreator
    {
        public string OutputDirectoryPath;
        public ISelector Selector;
        private string _inputDirectory;

        public XmlFilesCreator (string outdir, ISelector selector)
        {
            OutputDirectoryPath = outdir;
            this.Selector = selector;
        }

        public void SaveFiles(string inputPath)
        {
            var input = File.GetAttributes(@inputPath);
            // if inputPath is a directory
            if ((input & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var dirPath = new DirectoryInfo(@inputPath);
                _inputDirectory = inputPath;
                SaveForDirectory(dirPath);
            }
            // if inputPath is a file
            else
            {
                var filPath = new FileInfo(@inputPath);
                _inputDirectory = filPath.DirectoryName;
                //_ext = Utils.CodeAnalyzer.GetFileExtension(filPath.FullName);
                SaveForFile(filPath);
            }
        }

        private void SaveForDirectory(DirectoryInfo directory)
        {
            var files = directory.GetFiles("*.*");
            foreach (FileInfo file in files)
            {   
                SaveForFile(file);
            }
            var dirs = directory.GetDirectories("*.*");
            foreach (DirectoryInfo dir in dirs)
            {
                SaveForDirectory(dir);
            }
        }

        private void SaveForFile(FileInfo file)
        {
            var xdir = file.Directory.ToString().Remove(0, _inputDirectory.Length);
            var xname = xdir.Length == 0 ? file.Name : xdir.Replace("\\", "#") + "#" + file.Name;

            var xgen = Selector.SelectXmlGenerator(file.Extension.ToLower());
            if (xgen == null)
            {
                Console.WriteLine("Can not create XML for " + xname.Replace("#","\\"));
            }
            else
            {
                var xdoc = xgen.GenerateXmlDocument(file.FullName);
                if (xdoc != null)
                {
                    Console.WriteLine("Saving XML for " + xname.TrimStart('#').Replace("#", "\\"));
                    xdoc.Save(@OutputDirectoryPath + "\\" + xname + ".xml");
                }
            }
        }

    }
}
