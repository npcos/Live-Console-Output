# Live Console Output
See output of your console application whenever you compile.

Intended for use with short-running console applications where you want to constantly see the output refreshed whenever you compile.  
Kind of like live-reload for websites, but for console.

# How it works
You provide the path to your project output executable the arguments to use.  
Whenever there is a change to the directory containing that executable, it is re-run using the arguments provided.

Usage:
`dotnet LiveConsoleOutput.dll my-project-output.exe args`

When provided with a dll, it will use dotnet to execute it:
`dotnet LiveConsoleOutput.dll my-dotnet-core-project-output.dll args`