using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.TimeTracking
{
    /// <summary>
    /// A class that represents YouTrack issue work type information.
    /// </summary>
    public class WorkType
    {
        /// <summary>
        /// Creates an instance of the <see cref="WorkType" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="WorkItemType"/> to convert from.</param>
        public static WorkType FromApiEntity(WorkItemType entity)
        {
            var workType = new WorkType {Id = entity.Id, Name = entity.Name};
            return workType;
        }
        
        /// <summary>
        /// Converts to instance of the <see cref="WorkItemType" /> class used in api client.
        /// </summary>
        public WorkItemType ToApiEntity()
        {
            return new WorkItemType() {Id = Id, Name = Name};
        }
        
        /// <summary>
        /// Id of the work type.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// Name of the work type.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /*TODO remove completely or generate on-the-fly
        /// <summary>
        /// Url of the work type.
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }
        */
    }
}