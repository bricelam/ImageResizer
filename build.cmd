@ECHO OFF

IF NOT EXIST .nuget\nuget.exe (
    MKDIR .nuget
    powershell.exe -Command "& {iwr https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile .nuget\nuget.exe}"
)

IF NOT DEFINED VisualStudioVersion CALL "%VS140COMNTOOLS%VsDevCmd.bat"

.nuget\nuget.exe restore
MSBuild.exe ImageResizer.proj /m /v:m /nr:false /nologo %*
