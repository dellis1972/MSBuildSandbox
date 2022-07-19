# MSBuildSandbox

This repo is designed to help with the development of MSBuild 
Tasks and Targets. It provides an integrated Unit Test Framework
which will allow you to debug your Tasks directly in the VSCode.

## Layout

The project is split up into two separate projects , the first is the 
MSBuildSandbox.Tasks project. This is where you would create new Tasks
and targets. 

The second project is the MSBuildSandbox.Tests project. This is where
the Unit tests can be added. 

The sandbox contains two sample tests, one for a Task and one for a 
.target. 

## Example Task Test

The Example.cs file contains a very basic MSBuild Task. It just returns
a result.  The Test found in Tests.cs contains the unit test for this 
Task. Task tests make use of the `MockBuildEngine` class to fake that
the task is running inside an MSBuild process. This allows you to call the
Task directly! It also allows you to debug and step through your task during
development. 

Looking at the example the first thing we do is create a `MockBuildEngine`.

```csharp
var engine = CreateMockEngine ();
```

We can then create the `Task` we want to test and set the `BuildEngine` property
to our `MockBuildEngine`. At this point you can also set any other properties
the task requires if your task takes custom inputs.

```csharp
var task = new Example () {
    BuildEngine = engine,
};
```

We can then call the `Task.Execute` method directly and `Assert` the result to 
make sure it executed correctly. 

```csharp
Assert.True (task.Execute ());
```

Once executed you could then check your `Output` properties if you have any to 
make sure they are set as expected. Or check for files that the `Task` should
have created. 


## Example Targets Test

This is an experiment in unit testing Tasks when being used from a Target.
The test defines a project which we load in the unit test. The project imports
the `MSBuildSandbox.targets` file along with the defined Tasks. 

We then use the `Example` task and the `Message` task in a `Target` in the unit test.
This allows us to call a `Task` from a `Target` and put breakpoints in the `Task`
so we can step through the code. 

This is a work in progress, and has only been tested on MacOS so far. 

Wish List
- [X] Break in Task when used from Targets.

- [] Works on Windows in Visual Studio and VSCode.

- [] Works on MacOS in Visual Studio for Mac

- [] Step through target files (at this time not possible at all).
