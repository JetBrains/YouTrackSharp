using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xunit;
using YouTrackSharp.Json;

namespace YouTrackSharp.Tests.Json
{
    public class TimeSpanMinutesConverterTests
    {
        public class CanConvert
        {
            [Theory]
            [InlineData(typeof(TimeSpan))]
            public void CanConvert_TimeSpan(Type type)
            {
                // Arrange
                var converter = new TimeSpanMinutesConverter();

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
                yield return new object[] { TimeSpan.FromMinutes(5), 5 };
                yield return new object[] { TimeSpan.FromSeconds(90), 2 }; // should round up
            }
            
            [Theory]
            [MemberData(nameof(GetData))]
            public void Writes_TimeSpan_To_Minutes(TimeSpan source, long destination)
            {
                // Arrange
                var result = new StringBuilder();
                var converter = new TimeSpanMinutesConverter();
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
            public static IEnumerable<object[]> GetData()
            {
                yield return new object[] { TimeSpan.FromMinutes(5), 5 };
                yield return new object[] { TimeSpan.FromSeconds(120), 2 };
            }
            
            [Theory]
            [MemberData(nameof(GetData))]
            public void Reads_Minutes_To_TimeSpan(TimeSpan destination, long source)
            {
                // Arrange
                TimeSpan result;
                var converter = new TimeSpanMinutesConverter();
                using (var reader = new JsonTextReader(new StringReader(source.ToString())))
                {
                    reader.Read();

                    // Act
                    result = (TimeSpan)converter.ReadJson(reader, typeof(TimeSpan), null, new JsonSerializer());
                }

                // Assert
                Assert.NotNull(result);
                Assert.Equal(destination, result);
            }
        }
    }
}