@echo off
set config=%1
if "%config%" == "" (
    set config=Release
)

echo ##teamcity[blockOpened name='Prepare' description='Preparing build environment...']
mkdir artifacts
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
dotnet pack src\YouTrackSharp\YouTrackSharp.csproj --output %cd%\artifacts --include-symbols --include-source --configuration %config%
if not "%errorlevel%"=="0" goto failure
echo ##teamcity[blockClosed name='Pack']

:success
REM exit 0

:failure
REM exit -1