﻿#region License

// Copyright (C) 2011-2012 The Unicoen Project
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.IO;
using System.Linq;
using Unicoen.Apps.Loc.Util;
using Unicoen.Apps.UniMetrics.FrameworkUser;
using Unicoen.Apps.UniMetrics.UcoAnalyzerComponent;
using Unicoen.Apps.UniMetrics.Utils;
using Unicoen.Apps.UniMetrics.XmlGeneratorComponent;

namespace Unicoen.Apps.UniMetrics {
	internal class Program {
		private static void Main(string[] args) {
			// BASIC MEASUREMENT
			// args[0] is the language
			// arge[1] is the file name or directory name
			if (args.Length == 2) {
				var inputPath = FixtureUtil.GetInputPath(args[0], args[1]);
				Console.WriteLine("Language : " + args[0]);
				Console.WriteLine("Input    : " + inputPath + "\n");

				MeasurableElementGenerator me = null;
				XmlGenerator xml = null;
				/*switch (CodeAnalyzer.GetFileExtension(inputPath))
				{
					case ".c":
						// not yet implemented
						break;
					case ".cs":
						// not yet implemented
						break;
					case ".java":
						me = new MeasurableElementForJava();
						xml = new XmlGeneratorForJava();
						break;
					case ".js":
						me = new MeasurableElementForJavaScript();
						xml = new XmlGeneratorForJavaScript();
						break;
					case ".py":
						// not yet implemented
						break;
					case ".rb":
						// not yet implemented
						break;
					default:
						Console.WriteLine("Incorrect input file");
						break;
				}*/
				xml = new XmlGeneratorForJava();
				var a = new XmlFilesCreator("_uco");
				a.SaveFiles(inputPath, xml);
			}

			// DIFFERENTIAL MEASUREMENT
			// args[0] is the language
			// arge[1] is the original source code file
			// arge[2] is the modified source code file
			if (args.Length == 3) {
				var orign = FixtureUtil.GetInputPath(args[0], args[1]);
				var modif = FixtureUtil.GetInputPath(args[0], args[2]);
				Console.WriteLine("Language      : " + args[0] + "\n");
				Console.WriteLine("File 1 : " + orign);
				Console.WriteLine("File 2 : " + modif);
				PrintDifferentialInfo(orign, modif);
			}
		}

		private static void PrintDifferentialInfo(string orign, string modif) {
			DiffCounter c = DifferentialLoc.Count(orign, modif);
			Console.WriteLine("Added Line of Code    : " + c.AddedCount);
			Console.WriteLine("Deleted Line of Code  : " + c.DeletedCount);
			Console.WriteLine("Modified Line of Code : " + c.ModifiedCount);
			Console.WriteLine("Equal Line of Code    : " + c.EqualCount);
			Console.WriteLine();
		}
	}

	#region FixtureUtil
	static class FixtureUtil
	{
		public static string RootPath = Path.Combine("..", "..");
		public static string FixturePath = Path.Combine(RootPath, "fixture");
		public const string AopExpectationName = "aspect_expectation";
		public const string ExpectationName = "expectation";
		public const string InputName = "input";
		public const string DownloadName = "download";
		public const string FailedInputName = "failed_input";
		public const string OutputName = "output";
		public const string XmlExpectationName = "xmlexpectation";
		public const string ScriptName = "script";

		public static string CleanOutputAndGetOutputPath()
		{
			var path = GetOutputPath();
			if (Directory.Exists(path))
			{
				var dirPaths = Directory.EnumerateDirectories(
						path, "*", SearchOption.TopDirectoryOnly);
				foreach (var dirPath in dirPaths)
				{
					Directory.Delete(dirPath, true);
				}
				var filePaths = Directory.EnumerateFiles(
						path, "*", SearchOption.TopDirectoryOnly);
				foreach (var filePath in filePaths)
				{
					File.Delete(filePath);
				}
			}
			Directory.CreateDirectory(path);
			return path.GetFullPathAddingSubNames();
		}

		public static string GetFullPathAddingSubNames(
				this string path,
				params string[] subNames)
		{
			return Path.GetFullPath(subNames.Aggregate(path, Path.Combine));
		}

		public static string GetOutputPath(params string[] names)
		{
			var path = Path.Combine(FixturePath, OutputName);
			Directory.CreateDirectory(path);
			return path.GetFullPathAddingSubNames(names);
		}

		public static string GetInputPath(string lang, params string[] names)
		{
			return Path.Combine(FixturePath, lang, InputName)
					.GetFullPathAddingSubNames(names);
		}

		//AspectAdaptorTest向けのアスペクトファイルへのパスを返す
		public static string GetAspectPath(string name)
		{
			return GetInputPath("Aspect", "partial_aspect", name);
		}

		//AspectAdaptorTest向けのアスペクト合成後の期待値へのパスを返す
		public static string GetAspectExpectationPath(string name)
		{
			return GetInputPath("Aspect", "expectation", name);
		}

		public static string GetDownloadPath(string lang, params string[] names)
		{
			return Path.Combine(FixturePath, lang, DownloadName)
					.GetFullPathAddingSubNames(names);
		}

		public static string GetFailedInputPath(string lang, params string[] names)
		{
			return Path.Combine(FixturePath, lang, FailedInputName)
					.GetFullPathAddingSubNames(names);
		}

		public static string GetExpectationPath(string lang, params string[] names)
		{
			return Path.Combine(FixturePath, lang, ExpectationName)
					.GetFullPathAddingSubNames(names);
		}

		public static string GetAopExpectationPath(string lang, params string[] names)
		{
			return Path.Combine(FixturePath, lang, AopExpectationName)
					.GetFullPathAddingSubNames(names);
		}

		public static string GetXmlExpectationPath(string lang, params string[] names)
		{
			return Path.Combine(FixturePath, lang, XmlExpectationName)
					.GetFullPathAddingSubNames(names);
		}

		public static string GetScriptPath(string lang, params string[] names)
		{
			return Path.Combine(FixturePath, lang, ScriptName)
					.GetFullPathAddingSubNames(names);
		}
	}
	#endregion
}