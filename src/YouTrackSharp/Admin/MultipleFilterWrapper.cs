using JsonFx.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Admin
{
    public class MultipleFilterWrapper : IDataWrapper<Filter>
    {
        #region IDataWrapper<Filter> Members

        [JsonName("query")]
        public Filter[] Data { get; set; }

        #endregion
    }
}