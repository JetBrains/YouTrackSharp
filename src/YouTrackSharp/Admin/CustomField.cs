using System.Collections.Generic;

namespace YouTrackSharp.Admin
{
    public class CustomField
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string EmptyText { get; set; }
        public IEnumerable<Param> Param { get; set; }
    }
}