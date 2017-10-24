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
        /// Value.
        /// </summary>
        [JsonProperty("oldValue")]
        public object OldValue;

        /// <summary>
        /// Value.
        /// </summary>
        [JsonProperty("newValue")]
        public object NewValue;

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
            if (Value == null)
            {
                return null;
            }
            
            if (Value is ICollection<string> collection)
            {
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
            if (Value == null)
            {
                return new List<string>();
            }
            else if (Value is ICollection<string> collection)
            {
                return collection;
            }
            else
            {
                return new List<string>()
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
            if (Value is DateTime dateTime)
            {
                return dateTime;
            }
            else if (Value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime;
            }
            else
            {
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
    }
}