using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents a YouTrack issue change.
    /// </summary>
    public class Change
    {
        /// <summary>
        /// Creates an instance of the <see cref="Change" /> class.
        /// </summary>
        public Change()
        {
            Fields = new List<FieldChange>();
        }
        
        /// <summary>
        /// Fields that have been changed.
        /// </summary>
        [JsonProperty("field")]
        public ICollection<FieldChange> Fields { get; set; }

        /// <summary>
        /// Get <see cref="FieldChange"/> for a field identified by <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">Name of the field to retrieve <see cref="FieldChange"/> for.</param>
        /// <returns><see cref="FieldChange"/> for a field identified by <paramref name="fieldName"/>. Can be <value>null</value>.</returns>
        public FieldChange ForField(string fieldName)
        {
            return Fields.FirstOrDefault(field => string.Equals(field.Name, fieldName, StringComparison.OrdinalIgnoreCase));
        }
    }
}