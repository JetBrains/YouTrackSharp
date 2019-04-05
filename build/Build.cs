using System;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public string PackageVersionSuffix => "develop-" + DateTime.UtcNow.ToString("yyyyMMddhhmm");

    [Solution] readonly Solution Solution;
    
    public static int Main() => Execute<Build>(x => x.Pack);

    Target Initialize => _ => _
        .Executes(() =>
        {
            SetVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1");
        });

    Target Clean => _ => _
        .DependsOn(Initialize)
        .Executes(() =>
        {
            foreach (var directory in Solution.Directory.GlobDirectories("**/bin", "**/obj"))
            {
                DeleteDirectory(directory);
            }
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore();
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(settings => settings
                .SetVersionSuffix(PackageVersionSuffix));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(settings => settings
                .SetProjectFile(Solution.Directory / "tests" / "YouTrackSharp.Tests" / "YouTrackSharp.Tests.csproj")
                .EnableNoBuild());
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            EnsureExistingDirectory(Solution.Directory / "artifacts");

            DotNetTasks.DotNetPack(settings => settings
                .SetOutputDirectory(Solution.Directory / "artifacts")
                .EnableIncludeSource()
                .EnableIncludeSymbols()
                .SetVersionSuffix(PackageVersionSuffix));
        });
}
