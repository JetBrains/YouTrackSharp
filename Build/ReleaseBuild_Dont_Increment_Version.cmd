%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild /m:4 %0\..\Build.proj /p:IsDesktopBuild=true;SkipVersionNumberIncrement=true;GlobalBuildVersionNumber=1.0.0.0 
pause