﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
//using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using MSBuildSandbox.Tasks;
using NUnit;
using NUnit.Framework;
namespace MSBuildSandbox.Tests {
	public class Tests {

		public string Root {
			get {
				return Path.GetDirectoryName (typeof (Tests).Assembly.Location);
			}
		}

		IBuildEngine CreateMockEngine ()
		{
			return new MockBuildEngine (TestContext.Out);
		}

		IEnumerable<ILogger> CreateLogger (StringBuilder sb)
		{
			var logger = new ConsoleLogger (LoggerVerbosity.Diagnostic, (string message) => {
				TestContext.Out.WriteLine (message);
				sb.AppendLine (message);
			}, null, null);
			List<ILogger> loggers = new List<ILogger> ();
			loggers.Add (logger);
			return loggers;
		}

		Project LoadProject (XmlReader reader) {
			if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
				Environment.SetEnvironmentVariable ("MSBUILD_EXE_PATH", typeof (Tests).Assembly.Location);
			}
			try {
				var collection = new ProjectCollection ();
				return collection.LoadProject (reader);
			} finally {
				if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
					Environment.SetEnvironmentVariable ("MSBUILD_EXE_PATH", null);
				}
			}
		}
		Project LoadProject (string source ) {
			var ms = new StringReader (source);
			var reader = XmlReader.Create (ms);
			return LoadProject(reader);
		}

		Project LoadProjectFromFile (string file) {
			var reader = XmlReader.Create (file);
			return LoadProject(reader);
		}

		[Test]
		public void ExampleTaskTest ()
		{
			var engine = CreateMockEngine ();
			var task = new Example () {
				BuildEngine = engine,
			};
			Assert.True (task.Execute ());
		}

		[Test]
		public void ExampleTargetTest ()
		{

			var path = Path.Combine (Root, "temp", TestContext.CurrentContext.Test.Name);
			Directory.CreateDirectory (path);
			var current = Directory.GetCurrentDirectory ();
			try {
				Directory.SetCurrentDirectory (path);
				var source = @"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
<Import Project=""..\..\MSBuildSandbox.targets"" />
<Target Name=""RunTest"" >
	<Example />
	<Message Text=""$(FrameworkSDKRoot)"" Importance=""High"" />
</Target>
</Project>";

				var project = LoadProject (source);
				var sb = new StringBuilder ();
				var b = project.Build ("RunTest", CreateLogger (sb));
				Assert.True (b, $"Build should have worked. {sb}");
			} finally {
				Directory.SetCurrentDirectory (current);
				Directory.Delete (path, recursive: true);
			}
		}
	}
}
