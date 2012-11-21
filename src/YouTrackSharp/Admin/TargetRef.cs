using System;
using JsonFx.Json;

namespace YouTrackSharp.Admin
{
    public class TargetRef
    {
        public string Name { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public string RestQuery
        {
            get
            {
                var uri = new Uri(Url);
                const string restPath = "/rest/";
                var restIndex = uri.LocalPath.IndexOf(restPath);
                var restQuery = uri.LocalPath.Remove(0, restIndex + restPath.Length);
                return restQuery;
            }
        }
    }
}