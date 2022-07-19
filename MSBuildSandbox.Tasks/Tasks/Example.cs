using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuildSandbox.Tasks {
	public class Example : Task {
		public string Text { get; set; }
		public override bool Execute ()
		{
			Log.LogMessage (Text, MessageImportance.High);
			return !Log.HasLoggedErrors;
		}
	}
}
