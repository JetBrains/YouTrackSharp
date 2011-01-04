using JsonFx.Json;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Projects
{
    public class Project
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsImporting { get; set; }
        public SubValuesArray AssigneesFullname   { get; set; }
    }
}