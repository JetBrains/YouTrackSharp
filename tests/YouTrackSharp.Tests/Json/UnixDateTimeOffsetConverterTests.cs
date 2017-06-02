using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xunit;
using YouTrackSharp.Json;

namespace YouTrackSharp.Tests.Json
{
    public class UnixDateTimeOffsetConverterTests
    {
        public class CanConvert
        {
            [Theory]
            [InlineData(typeof(DateTimeOffset))]
            [InlineData(typeof(DateTimeOffset?))]
            [InlineData(typeof(DateTime))]
            [InlineData(typeof(DateTime?))]
            public void CanConvert_DateTimeOffset_And_DateTime(Type type)
            {
                // Arrange
                var converter = new UnixDateTimeOffsetConverter();

                // Act
                var result = converter.CanConvert(type);

                // Assert
                Assert.True(result);
            }
        }

        public class WriteJson
        {
            public static IEnumerable<object[]> GetData()
            {
                yield return new object[] { new DateTimeOffset(2001, 01, 01, 10, 11, 0, TimeSpan.Zero), 978343860 };
                yield return new object[] { new DateTime(2001, 01, 01, 10, 11, 0), 978340260 };
            }

            [Theory]
            [MemberData(nameof(GetData))]
            public void Writes_DateTimeOffset_And_DateTime_To_String(object source, long destination)
            {
                // Arrange
                var result = new StringBuilder();
                var converter = new UnixDateTimeOffsetConverter();
                using (var writer = new JsonTextWriter(new StringWriter(result)))
                {
                    // Act
                    converter.WriteJson(writer, source, new JsonSerializer());
                }

                // Assert
                Assert.Equal(destination.ToString(), result.ToString());
            }
        }

        public class ReadJson
        {
            public static IEnumerable<object[]> GetDateTimeOffsetData()
            {
                yield return new object[] { new DateTimeOffset(2001, 01, 01, 10, 11, 0, TimeSpan.Zero), 978343860 };
            }

            [Theory]
            [MemberData(nameof(GetDateTimeOffsetData))]
            public void Reads_String_To_DateTimeOffset(DateTimeOffset destination, long source)
            {
                // Arrange
                DateTimeOffset result;
                var converter = new UnixDateTimeOffsetConverter();
                using (var reader = new JsonTextReader(new StringReader(source.ToString())))
                {
                    reader.Read();

                    // Act
                    result = (DateTimeOffset)converter.ReadJson(reader, typeof(DateTimeOffset), null, new JsonSerializer());
                }

                // Assert
                Assert.NotNull(result);
                Assert.Equal(destination, result);
            }
        }
    }
}