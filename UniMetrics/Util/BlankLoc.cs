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

namespace Unicoen.Apps.Loc.Util {
	public class BlankLoc {
		
		/// <summary>
		/// measure number of the lines which contain only space character(s)
		/// (the blank character, tab character and newline character) 
		/// </summary>
		public static int Count(string inputPath) {
			var attr = File.GetAttributes(@inputPath);
			// if inputPath is a directory
			if ((attr & FileAttributes.Directory) == FileAttributes.Directory) {
				var dirPath = new DirectoryInfo(@inputPath);
				return CountForDirectory(dirPath);
			}
					// if inputPath is a file
			else {
				return CountForFile(inputPath);
			}
		}

		/// <summary>
		/// count sum of blank LOC of all files in directory
		/// </summary>
		private static int CountForDirectory(DirectoryInfo dirPath) {
			var sum = 0;
			var files = dirPath.GetFiles("*.*");
			foreach (var file in files) {
				var fi = file.FullName;
				var fiLoc = CountForFile(fi);
				sum += fiLoc;
				Console.WriteLine(fi + " | bloc=" + fiLoc);
			}
			var dirs = dirPath.GetDirectories("*.*");
			foreach (DirectoryInfo dir in dirs) {
				sum += CountForDirectory(dir);
			}
			return sum;
		}

		
		/// <summary>
		/// count blank LOC of a file
		/// </summary>
		private static int CountForFile(string filePath) {
			string line;
			int count = 0;
			var sr = new StreamReader(filePath);
			while ((line = sr.ReadLine()) != null) {
				if (line.Trim().Length == 0) {
					count++;
				}
			}
			sr.Close();

			var srb = new StreamReader(filePath);
			var arrb = srb.ReadToEnd().ToCharArray();
			var lastb = BitConverter.GetBytes(arrb[arrb.Length - 1]);
			if (lastb[0] == 10) {
				count++;
			}
			srb.Close();

			return count;
		}
	}
}