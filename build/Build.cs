using System;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Core;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Core.EnvironmentInfo;
using static Nuke.Core.IO.FileSystemTasks;
using static Nuke.Core.IO.PathConstruction;

class Build : NukeBuild
{
    [GitVersion] readonly GitVersion GitVersion;

    public override string Configuration => IsServerBuild ? "Release" : Argument("configuration");

    public string PackageVersionSuffix => GitVersion.BranchName.Replace("/", "-") + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmm");

    public static int Main() => Execute<Build>(x => x.Pack);

    Target Initialize => _ => _
        .Executes(() =>
        {
            Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1");
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
        .Requires(() => GitVersion != null)
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
        .Requires(() => GitVersion != null)
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