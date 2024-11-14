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
	public class Tests {

		IBuildEngine CreateMockEngine (IList<BuildErrorEventArgs> errors = null, IList<BuildWarningEventArgs> warnings = null, IList<BuildMessageEventArgs> messages = null, IList<CustomBuildEventArgs> customEvents = null)
		{
			return new MockBuildEngine (TestContext.Out, errors, warnings, messages, customEvents);
		}

		[Test]
		public void ExampleTaskTest ()
		{
			List<BuildMessageEventArgs> messages = new List<BuildMessageEventArgs> ();
			var engine = CreateMockEngine (messages: messages);
			var task = new Example () {
				BuildEngine = engine,
				Text = "Hello World",
			};
			Assert.True (task.Execute ());
			Assert.AreEqual (1, messages.Count, "There should be 1 message");
		}

	}
}
