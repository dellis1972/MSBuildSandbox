﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;

namespace MSBuildSandbox.Tests {
	public class MockBuildEngine : IBuildEngine, IBuildEngine2, IBuildEngine3, IBuildEngine4 {
		public MockBuildEngine (TextWriter output, IList<BuildErrorEventArgs> errors = null)
		{
			this.Output = output;
			this.Errors = errors;
		}

		private TextWriter Output { get; }

		private IList<BuildErrorEventArgs> Errors { get; }

		int IBuildEngine.ColumnNumberOfTaskNode => -1;

		bool IBuildEngine.ContinueOnError => false;

		int IBuildEngine.LineNumberOfTaskNode => -1;

		string IBuildEngine.ProjectFileOfTaskNode => "this.xml";

		bool IBuildEngine2.IsRunningMultipleNodes => false;

		bool IBuildEngine.BuildProjectFile (string projectFileName, string [] targetNames, IDictionary globalProperties, IDictionary targetOutputs) => true;

		void IBuildEngine.LogCustomEvent (CustomBuildEventArgs e)
		{
			this.Output.WriteLine ($"Custom: {e.Message}");
		}

		void IBuildEngine.LogErrorEvent (BuildErrorEventArgs e)
		{
			this.Output.WriteLine ($"Error: {e.Message}");
			if (Errors != null)
				Errors.Add (e);
		}

		void IBuildEngine.LogMessageEvent (BuildMessageEventArgs e)
		{
			this.Output.WriteLine ($"Message: {e.Message}");
		}

		void IBuildEngine.LogWarningEvent (BuildWarningEventArgs e)
		{
			this.Output.WriteLine ($"Warning: {e.Message}");
		}

		private Dictionary<object, object> Tasks = new Dictionary<object, object> ();

		void IBuildEngine4.RegisterTaskObject (object key, object obj, RegisteredTaskObjectLifetime lifetime, bool allowEarlyCollection)
		{
			Tasks.Add (key, obj);
		}

		object IBuildEngine4.GetRegisteredTaskObject (object key, RegisteredTaskObjectLifetime lifetime)
		{
			object obj = null;
			Tasks.TryGetValue (key, out obj);
			return obj;
		}

		object IBuildEngine4.UnregisterTaskObject (object key, RegisteredTaskObjectLifetime lifetime)
		{
			var obj = Tasks [key];
			Tasks.Remove (key);
			return obj;
		}

		BuildEngineResult IBuildEngine3.BuildProjectFilesInParallel (string [] projectFileNames, string [] targetNames, IDictionary [] globalProperties, IList<string> [] removeGlobalProperties, string [] toolsVersion, bool returnTargetOutputs)
		{
			throw new NotImplementedException ();
		}

		void IBuildEngine3.Yield () { }

		void IBuildEngine3.Reacquire () { }

		bool IBuildEngine2.BuildProjectFile (string projectFileName, string [] targetNames, IDictionary globalProperties, IDictionary targetOutputs, string toolsVersion) => true;

		bool IBuildEngine2.BuildProjectFilesInParallel (string [] projectFileNames, string [] targetNames, IDictionary [] globalProperties, IDictionary [] targetOutputsPerProject, string [] toolsVersion, bool useResultsCache, bool unloadProjectsOnCompletion) => true;
	}
}
