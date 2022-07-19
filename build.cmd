@echo off

FOR /F "tokens=1 delims=" %%F IN ('vswhere.cmd') DO SET result=%%F
2>NUL CALL "%result%\Common7\Tools\VsDevCmd.bat"
IF ERRORLEVEL 1 CALL:FAILED_CASE

dotnet msbuild MSBuildSandbox.sln /restore /t:Build /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary