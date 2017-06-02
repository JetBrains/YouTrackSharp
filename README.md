# YouTrackSharp

![YouTrackSharp](logo.png)

.NET Library to access YouTrack API.

For more information on YouTrack visit [jetbrains.com/youtrack](http://www.jetbrains.com/youtrack).

[![MyGet Build Status](https://www.myget.org/BuildSource/Badge/youtracksharp?identifier=9cb1066f-5a24-47b1-acf8-51d21bf2d5d8)](https://www.myget.org/)

## Getting started

TODO

It is recommended to always use [permanent tokens](https://www.jetbrains.com/help/youtrack/incloud/Manage-Permanent-Token.html) to authenticate against YouTrack, using the `BearerTokenConnection`. For YouTrack instances that do not have token support, `UsernamePasswordConnection` can be used.

TODO

## Supported YouTrack versions

YouTrack versions 4.X and higher as well as YouTrack InCloud are supported.

*Note: If you're looking for YouTrack 3.x support, please use the 1.x branch from [https://github.com/JetBrains/YouTrackSharp](https://github.com/JetBrains/YouTrackSharp).
Be aware that this branch is frozen and only bug fixes will make it in. No new feature development is done on that version.*