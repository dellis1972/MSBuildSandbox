#!/bin/bash
msbuild MSBuildSandbox.sln /restore /t:Build /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary