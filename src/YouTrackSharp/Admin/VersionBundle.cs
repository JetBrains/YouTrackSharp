using System.Collections.Generic;

namespace YouTrackSharp.Admin
{
    public class VersionBundle
    {
        public string Name { get; set; }
        public IEnumerable<Version> Version { get; set; }
    }
}