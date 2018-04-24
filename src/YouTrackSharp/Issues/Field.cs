using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents a YouTrack issue field.
    /// </summary>
    [DebuggerDisplay("{Name}: {Value}")]
    public class Field
    {
        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// Value.
        /// </summary>
        [JsonProperty("value")]
        public object Value;

        /// <summary>
        /// Value Id.
        /// </summary>
        [JsonProperty("valueId")]
        public object ValueId;

        /// <summary>
        /// Field color.
        /// </summary>
        [JsonProperty("color")]
        public YouTrackColor Color;

        /// <summary>
        /// Gets the value as a <see cref="T:System.String"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.String"/>.</returns>
        public string AsString()
        {
            switch (Value)
            {
                case null:
                    return null;
                case ICollection<string> collection:
                    return collection.SingleOrDefault();
            }

            return Value.ToString();
        }
        
        /// <summary>
        /// Gets the value as a <see cref="T:System.Collections.Generic.ICollection{System.String}"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.Collections.Generic.ICollection{System.String}"/>.</returns>
        public ICollection<string> AsCollection()
        {
            switch (Value)
            {
                case null:
                    return new List<string>();
                case ICollection<string> collection:
                    return collection;
                default:
                    return new List<string>
                    {
                        Value.ToString()
                    };
            }
        }
        
        /// <summary>
        /// Gets the value as a <see cref="T:System.DateTime"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.DateTime"/>.</returns>
        public DateTime AsDateTime()
        {
            switch (Value)
            {
                case DateTime dateTime:
                    return dateTime;
                case DateTimeOffset dateTimeOffset:
                    return dateTimeOffset.DateTime;
                default:
                    var milliseconds = Convert.ToInt64(AsString());
            
                    return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).DateTime;
            }
        }

        /// <summary>
        /// Gets the value as a <see cref="T:System.Int32"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.Int32"/>.</returns>
        public int AsInt32()
        {
            return Convert.ToInt32(AsString());
        }

        /// <summary>
        /// Gets the value as a <see cref="T:System.Boolean"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.Boolean"/>.</returns>
        public bool AsBool()
        {
            return Convert.ToBoolean(AsString());
        }
    }
}