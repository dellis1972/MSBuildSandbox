using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
//using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Evaluation.Context;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Utilities;
using MSBuildSandbox.Tasks;
using NUnit;
using NUnit.Framework;
namespace MSBuildSandbox.Tests {
	public class TestTargets {

		public string Root {
			get {
				return Path.GetDirectoryName (typeof (Tests).Assembly.Location);
			}
		}

		IBuildEngine CreateMockEngine (IList<BuildErrorEventArgs> errors = null, IList<BuildWarningEventArgs> warnings = null, IList<BuildMessageEventArgs> messages = null, IList<CustomBuildEventArgs> customEvents = null)
		{
			return new MockBuildEngine (TestContext.Out, errors, warnings, messages, customEvents);
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
			string path = GetMSbuildLocation ();
			var collection = new ProjectCollection ();
			collection.AddToolset (new Toolset (ToolLocationHelper.CurrentToolsVersion, path, collection, string.Empty));
			return collection.LoadProject (reader);
			
		}

		public string GetMSbuildLocation ()
		{
			var path = Environment.GetEnvironmentVariable ("MSBuildExtensionsPath");
			if (!string.IsNullOrEmpty (path))
				return path;
			if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows)) {
				return ToolLocationHelper.GetPathToBuildToolsFile("msbuild.exe", ToolLocationHelper.CurrentToolsVersion);
			}
			else if (RuntimeInformation.IsOSPlatform (OSPlatform.OSX))
				return @"/usr/local/share/dotnet/sdk/8.0.100/";
			else if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux))
				return @"/usr/share/dotnet/sdk/8.0.200/";
			return ToolLocationHelper.GetPathToBuildToolsFile("MSBuild.dll", ToolLocationHelper.CurrentToolsVersion);
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
		//[Ignore ("Not working yet")]
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
	<Message Text=""$(FrameworkSDKRoot)"" Importance=""High"" />
	<Example Text=""Test""/>
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
