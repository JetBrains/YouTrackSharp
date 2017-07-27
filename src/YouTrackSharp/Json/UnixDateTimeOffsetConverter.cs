using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YouTrackSharp.Json
{
    /// <summary>
    /// A JSON convertor that can convert a unix timestamp into a <see cref="DateTimeOffset" /> value and vice-versa.
    /// </summary>
    public class UnixDateTimeOffsetConverter
        : DateTimeConverterBase
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTimeOffset)
            {
                writer.WriteValue(((DateTimeOffset)value).ToUnixTimeSeconds());
            }
            else if (value is DateTime)
            {
                writer.WriteValue(new DateTimeOffset((DateTime)value).ToUnixTimeSeconds());
            }
            else
            {
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

            long ticks = 0;

            if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float)
            {
                ticks = (long)reader.Value;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                long.TryParse(reader.Value.ToString(), out ticks);
            }
            else
            {
                throw new FormatException(string.Format(Strings.Exception_CouldNotParseUnixTimeStamp, reader.Value.ToString()));
            }

            DateTimeOffset converted;
            if (Math.Ceiling(Math.Log10(ticks)) >= 12)
            {
                // Milliseconds
                converted = DateTimeOffset.FromUnixTimeMilliseconds(ticks);
            }
            else
            {
                // Seconds
                converted = DateTimeOffset.FromUnixTimeSeconds(ticks);
            }

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