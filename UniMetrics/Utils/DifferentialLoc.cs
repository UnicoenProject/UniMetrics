#region License

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

using System.IO;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace Unicoen.Apps.UniMetrics.Utils {
	public class DifferentialLoc {
		/// <summary>
		/// measure differential between two files
		/// </summary>
		public static DiffCounter Count(
				string originalFile, string modifiedFile) {
			var original = File.ReadAllText(@originalFile);
			var modified = File.ReadAllText(@modifiedFile);

			var d = new Differ();
			var counter = new DiffCounter();
			var side = new SideBySideDiffBuilder(d);
			var result = side.BuildDiffModel(original, modified);

			// added, modified, and unmodified at modified file
			foreach (var line in result.NewText.Lines) {
				if (line.Type == ChangeType.Inserted) {
					counter.AddedCount++;
				}
				if (line.Type == ChangeType.Modified) {
					counter.ModifiedCount++;
				}
				if (line.Type == ChangeType.Unchanged) {
					counter.EqualCount++;
				}
			}

			// deleted from original file
			foreach (var line in result.OldText.Lines) {
				if (line.Type == ChangeType.Deleted) {
					counter.DeletedCount++;
				}
			}

			return counter;
		}
	}
}