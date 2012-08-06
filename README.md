#YouTrackSharp#

.NET Library to access YouTrack API.

For more information on YouTrack visit:

http://www.jetbrains.com/youtrack

## Supported YouTrack Versions ##

YouTrack Versions 3.X and higher

*Important:* Master branch supports YouTrack 4.0 and higher. All further development will
only take place on this main branch. 

If you're looking for YouTrack 3.x support, please use the 3.x branch, although be aware that
this branch if frozen and only bug fixes will make it in. No new feature development. The main
reason for this is that the JSON from one version to another has changed quite a bit and makes a lot of new functionality possible in 4.0, which would require a lot of effort in previous versions. 


## Breaking Changes in 2.0 ##

Issue is now a dynamic type. To work with it, you need to declare an issue as dynamic. The only
fixed field it has is "Id" (although later on some others might be added). The reason for this is that it is now inline with how YouTrack works itself which is that every field in an issue is basically a custom field. You can now have as many or as little custom fields as you like! This was a major change in 2.0 and is incompatible with existing code unfortunately.

## Usage ## 

Until we cover some simple usage scenarios here, take a look at the Spec tests. They are self-explanatory

##Issue and Feature Requests##

Please log all issues in YouTrack on Codebetter: http://youtrack.codebetter.com/issues/YTSRP
