[![official project](https://jb.gg/badges/official-flat-square.svg)](https://confluence.jetbrains.com/display/ALL/JetBrains+on+GitHub)

# YouTrackSharp

![YouTrackSharp](https://github.com/JetBrains/YouTrackSharp/raw/202/package_icon.png)

.NET library to access YouTrack API.

For more information on YouTrack visit [jetbrains.com/youtrack](https://www.jetbrains.com/youtrack).

## Getting started

First of all, install `YouTrackSharp` into your project using a NuGet client.

	Install-Package YouTrackSharp

If you want to work with pre-release builds, configure our [MyGet feed](https://www.myget.org/gallery/youtracksharp) as a package source.

To communicate with a YouTrack server instance, we'll need a connection. It is recommended to always use [permanent tokens](https://www.jetbrains.com/help/youtrack/incloud/Manage-Permanent-Token.html) to authenticate against YouTrack, using the `BearerTokenConnection`.

```csharp
var connection = new BearerTokenConnection("https://ytsharp.myjetbrains.com/youtrack/", "perm:abcdefghijklmn");
```

Once a connection is made, various services can be used. For example to get a list of projects the user has access to, the `ProjectsService` can be used:

```csharp
var projectsService = connection.CreateProjectsService();
var projectsForCurrentUser = await projectsService.GetAccessibleProjects();
```

Other services are available as well, mapping to the [YouTrack REST API](https://www.jetbrains.com/help/youtrack/incloud/deprecated-rest-api-reference.html) endpoints and operations that are available.

## Supported operations

YouTrackSharp is a .NET Library to access the YouTrack API. Main features:

* All calls are `async` all the way.
* Handles serialization of YouTrack's timestamps into `DateTime` where possible.
* Authentication using [permanent tokens](https://www.jetbrains.com/help/youtrack/incloud/Manage-Permanent-Token.html).
* Comes with a color indices list.

The following API's are currently supported:
* [User-related methods](https://www.jetbrains.com/help/youtrack/standalone/User-Related-Methods.html) through `UserService`
* [Projects-related methods](https://www.jetbrains.com/help/youtrack/standalone/Projects-Related-Methods.html) through `ProjectsService`
* [Issues-related methods](https://www.jetbrains.com/help/youtrack/standalone/Issues-Related-Methods.html) through `IssuesService`
* [Time-tracking-related methods](https://www.jetbrains.com/help/youtrack/standalone/Time-Tracking-User-Methods.html) through `TimeTrackingService`
* Administration API's
  * [User management](https://www.jetbrains.com/help/youtrack/standalone/Users.html) through `UserManagementService`
  * [Time Tracker management](https://www.jetbrains.com/help/youtrack/standalone/Time-Tracking-Settings-Methods.html) through `TimeTrackingManagementService`
  
Many other API's are not included yet - feel free to [tackle one of the `UpForGrabs` issues](https://github.com/JetBrains/YouTrackSharp/issues?q=is%3Aissue+is%3Aopen+label%3AUpForGrabs) and make YouTrackSharp better!

## Supported YouTrack versions

YouTrackSharp versions follow YouTrack versioning. This means that YouTrackSharp 2018.4 supports YouTrack version 2018.4 as well as YouTrack InCloud for that version.

Some features will work with both newer and older versions of YouTrack as well but they are not officially suppported.

For YouTrack versions before 2018.4:

* YouTrack versions before 2018.4 - Use [YouTrackSharp 3.x](https://www.nuget.org/packages/YouTrackSharp/)
* YouTrack Standalone 7.0 - [use the 2.x branch](https://github.com/JetBrains/YouTrackSharp/tree/2.x) or [YouTrackSharp 2.x](https://www.nuget.org/packages/YouTrackSharp/)
* YouTrack Standalone 6.5 - [use the 2.x branch](https://github.com/JetBrains/YouTrackSharp/tree/2.x) or [YouTrackSharp 2.x](https://www.nuget.org/packages/YouTrackSharp/)
* YouTrack Standalone 6.0 - [use the 2.x branch](https://github.com/JetBrains/YouTrackSharp/tree/2.x) or [YouTrackSharp 2.x](https://www.nuget.org/packages/YouTrackSharp/)
* YouTrack Standalone 5.x - [use the 2.x branch](https://github.com/JetBrains/YouTrackSharp/tree/2.x) or [YouTrackSharp 2.x](https://www.nuget.org/packages/YouTrackSharp/)
* YouTrack Standalone 4.x - [use the 2.x branch](https://github.com/JetBrains/YouTrackSharp/tree/2.x) or [YouTrackSharp 2.x](https://www.nuget.org/packages/YouTrackSharp/)
* YouTrack Standalone 3.x - [use the 1.x branch](https://github.com/JetBrains/YouTrackSharp/tree/1.x) or [YouTrackSharp 1.x](https://www.nuget.org/packages/YouTrackSharp/)

Be aware that these older branches are frozen and bug fixes nor new feature development is done on them.
