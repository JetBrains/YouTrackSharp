using System;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public string PackageVersionSuffix => "develop-" + DateTime.UtcNow.ToString("yyyyMMddhhmm");

    public static int Main() => Execute<Build>(x => x.Pack);

    Target Initialize => _ => _
        .Executes(() =>
        {
            SetVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1");
        });

    Target Clean => _ => _
        .DependsOn(Initialize)
        .Executes(() => DeleteDirectories(GlobDirectories(SourceDirectory, "**/bin", "**/obj")));

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(SolutionDirectory);
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(SolutionFile, settings => settings
                .SetVersionSuffix(PackageVersionSuffix));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(SolutionDirectory / "tests" / "YouTrackSharp.Tests" / "YouTrackSharp.Tests.csproj")
                .EnableNoBuild());
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            EnsureExistingDirectory(ArtifactsDirectory);

            DotNetPack(settings => settings
                .SetOutputDirectory(ArtifactsDirectory)
                .EnableIncludeSource()
                .EnableIncludeSymbols()
                .SetVersionSuffix(PackageVersionSuffix));
        });
}
