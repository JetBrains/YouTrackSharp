using JsonFx.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Admin
{
    public class MultipleFilterWrapper: IDataWrapper<Filter>
    {
        [JsonName("query")]
        public Filter[] Data { get; set; }
    }
}