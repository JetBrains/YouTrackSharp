[CmdletBinding()]
Param(
    [switch]$NoInit,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { $host.SetShouldExit(1) }
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

###########################################################################
# CONFIGURATION
###########################################################################

$NuGetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$SolutionDirectory = "$PSScriptRoot\..\YouTrackSharp"
$BuildProjectFile = "$PSScriptRoot\.\build\YouTrackSharp.Build.csproj"
$BuildExeFile = "$PSScriptRoot\.\build\bin\debug\YouTrackSharp.Build.exe"
$TempDirectory = "$PSScriptRoot\..\YouTrackSharp\.tmp"

###########################################################################
# PREPARE BUILD
###########################################################################

function ExecSafe([scriptblock] $cmd) {
    & $cmd
    if ($LastExitCode -ne 0) { throw "The following call failed with exit code $LastExitCode. '$cmd'" }
}

if (!$NoInit) {
    md -force $TempDirectory > $null

    $NuGetFile = "$TempDirectory\nuget.exe"
    $env:NUGET_EXE = $NuGetFile
    if (!(Test-Path $NuGetFile)) { (New-Object System.Net.WebClient).DownloadFile($NuGetUrl, $NuGetFile) }
    elseif ($NuGetUrl.Contains("latest")) { & $NuGetFile update -Self }

    ExecSafe { & $NuGetFile restore $BuildProjectFile -SolutionDirectory $SolutionDirectory }
    ExecSafe { & $NuGetFile install Nuke.MSBuildLocator -ExcludeVersion -OutputDirectory $TempDirectory -SolutionDirectory $SolutionDirectory }
}

$MSBuildFile = & "$TempDirectory\Nuke.MSBuildLocator\tools\Nuke.MSBuildLocator.exe"
ExecSafe { & $MSBuildFile $BuildProjectFile }

###########################################################################
# EXECUTE BUILD
###########################################################################

ExecSafe { & $BuildExeFile $BuildArguments }
