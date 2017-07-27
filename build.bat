@echo off
set config=%1
if "%config%" == "" (
    set config=Release
)
set DOTNET_CLI_TELEMETRY_OPTOUT=1

echo ##teamcity[blockOpened name='Prepare' description='Preparing build environment...']
dotnet clean
if not "%errorlevel%"=="0" goto failure
echo ##teamcity[blockClosed name='Prepare']

echo ##teamcity[blockOpened name='Restore' description='Restoring packages...']
dotnet restore
if not "%errorlevel%"=="0" goto failure
echo ##teamcity[blockClosed name='Restore']

echo ##teamcity[blockOpened name='Build' description='Building solution...']
dotnet build --configuration %config%
if not "%errorlevel%"=="0" goto failure
echo ##teamcity[blockClosed name='Build']

echo ##teamcity[blockOpened name='Test' description='Running tests...']
dotnet test tests\YouTrackSharp.Tests\YouTrackSharp.Tests.csproj --no-build --configuration %config%
if not "%errorlevel%"=="0" goto failure
echo ##teamcity[blockClosed name='Test']

echo ##teamcity[blockOpened name='Pack' description='Creating NuGet packages...']
mkdir artifacts
dotnet pack src\YouTrackSharp\YouTrackSharp.csproj --output %cd%\artifacts --include-symbols --include-source --configuration %config% /p:PackageVersion=%PackageVersion%
if not "%errorlevel%"=="0" goto failure
echo ##teamcity[blockClosed name='Pack']

:success
exit 0

:failure
exit -1
