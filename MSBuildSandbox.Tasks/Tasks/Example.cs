using System;
using Microsoft.Build.Utilities;

namespace MSBuildSandbox.Tasks {
	public class Example : Task {
		public override bool Execute ()
		{
			return !Log.HasLoggedErrors;
		}
	}
}
