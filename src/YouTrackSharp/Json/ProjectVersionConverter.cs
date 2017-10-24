using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace YouTrackSharp.Json
{
    /// <summary>
    /// A JSON convertor that can convert a string representing an array of version numbers to an actual list of version numbers represented as <see cref="T:System.String"/>.
    /// An example of such value could be <code>[2.0, 2.0.1, 2.0.2, 2.0.3, 2.0.4, 2.0.5, 2.0.6, 2.0.7, 2.0.8]</code>, as seen in the <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Accessible-Projects.html">YouTrack documentation</a>.
    /// </summary>
    public class ProjectVersionConverter
        : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ICollection<string> collection)
            {
                writer.WriteValue("[" + string.Join(", ", collection) + "]");
            }
            else
            {
                writer.WriteValue("[]");
            }
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var collection = existingValue as ICollection<string>;

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    var splitEntries = value.Trim('[', ']').Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var entry in splitEntries)
                    {
                        collection?.Add(entry.Trim());
                    }
                }
            }

            return collection;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof(ICollection<string>).GetTypeInfo()
                .IsAssignableFrom(objectType.GetTypeInfo());
        }
    }
}