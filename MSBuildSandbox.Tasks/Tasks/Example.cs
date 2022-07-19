using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuildSandbox.Tasks {
	public class Example : Task {
		public override bool Execute ()
		{
			return !Log.HasLoggedErrors;
		}
	}
}
