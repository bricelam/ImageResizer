#tool nuget:?package=xunit.runner.console

var target = Argument<string>("target");
var configuration = Argument<string>("configuration");
var signOutput = HasArgument("signOutput");

Task("Restore")
    .Does(
        () =>
            NuGetRestore("ImageResizer.sln"));

Task("Build")
    .IsDependentOn("Restore")
    .Does(
        () =>
        {
            MSBuild(
                "ImageResizer.sln",
                new MSBuildSettings
                {
                    ArgumentCustomization = args => args.Append("/nologo")
                }
                    .SetConfiguration(configuration)
                    .SetMaxCpuCount(0)
                    .SetVerbosity(Verbosity.Minimal)
                    .WithProperty("SignOutput", signOutput.ToString()));
            MSBuild(
                "ImageResizer.sln",
                new MSBuildSettings
                {
                    ArgumentCustomization = args => args.Append("/nologo")
                }
                    .SetConfiguration(configuration)
                    .SetMaxCpuCount(0)
                    .SetVerbosity(Verbosity.Minimal)
                    .WithProperty("Platform", "x64")
                    .WithProperty("SignOutput", signOutput.ToString()));
        });

Task("Clean")
    .Does(
        () =>
        {
            MSBuild(
                "ImageResizer.sln",
                new MSBuildSettings()
                    .SetConfiguration(configuration)
                    .WithTarget("Clean"));
            MSBuild(
                "ImageResizer.sln",
                new MSBuildSettings()
                    .SetConfiguration(configuration)
                    .WithProperty("Platform", "x64")
                    .WithTarget("Clean"));
        });

Task("Test")
    .IsDependentOn("Build")
    .Does(
        () =>
            XUnit2(
                "./test/ImageResizer.Tests/bin/" + configuration + "/ImageResizer.Tests.dll",
                new XUnit2Settings
                {
                    NoAppDomain = true,
                    ArgumentCustomization = args => args.Append("-nologo")
                }));

RunTarget(target);
