using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YouTrackSharp.Json
{
    /// <summary>
    /// A JSON convertor that can convert a unix timestamp (in milliseconds) into a <see cref="DateTimeOffset" /> value and vice-versa.
    /// </summary>
    public class UnixDateTimeOffsetConverter
        : DateTimeConverterBase
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case DateTimeOffset offset:
                    writer.WriteValue(offset.ToUnixTimeMilliseconds());
                    break;
                case DateTime dateTime:
                    writer.WriteValue(new DateTimeOffset(dateTime).ToUnixTimeMilliseconds());
                    break;
                default:
                    throw new Exception("Expected DateTimeOffset or DateTime.");
            }
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            long ticks;

            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                    ticks = (long)reader.Value;
                    break;
                case JsonToken.String:
                    long.TryParse(reader.Value.ToString(), out ticks);
                    break;
                default:
                    throw new FormatException(string.Format(Strings.Exception_CouldNotParseUnixTimeStamp, reader.Value.ToString()));
            }

            var converted = Math.Ceiling(Math.Log10(ticks)) >= 12 
                ? DateTimeOffset.FromUnixTimeMilliseconds(ticks) 
                : DateTimeOffset.FromUnixTimeSeconds(ticks);

            // Return value
            if (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?))
            {
                return converted;
            }
            else if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
            {
                return converted.DateTime;
            }

            return null;
        }
    }
}