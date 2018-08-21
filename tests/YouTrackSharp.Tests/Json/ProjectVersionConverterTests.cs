using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xunit;
using YouTrackSharp.Json;

namespace YouTrackSharp.Tests.Json
{
    public class ProjectVersionConverterTests
    {
        public class CanConvert
        {
            [Theory]
            [InlineData(typeof(ICollection<string>))]
            [InlineData(typeof(IList<string>))]
            [InlineData(typeof(List<string>))]
            public void CanConvert_Generic_Collection_Of_Strings(Type collectionType)
            {
                // Arrange
                var converter = new ProjectVersionConverter();

                // Act
                var result = converter.CanConvert(collectionType);

                // Assert
                Assert.True(result);
            }
        }

        public class WriteJson
        {
            [Fact]
            public void Writes_Collection_To_String()
            {
                // Arrange
                var result = new StringBuilder();
                var converter = new ProjectVersionConverter();
                using (var writer = new JsonTextWriter(new StringWriter(result)))
                {
                    // Act
                    converter.WriteJson(writer, new List<string> { "0.0.1", "0.0.2" }, new JsonSerializer());
                }

                // Assert
                Assert.Equal("\"[0.0.1, 0.0.2]\"", result.ToString());
            }

            [Fact]
            public void Writes_Empty_Collection_To_String()
            {
                // Arrange
                var result = new StringBuilder();
                var converter = new ProjectVersionConverter();
                using (var writer = new JsonTextWriter(new StringWriter(result)))
                {
                    // Act
                    converter.WriteJson(writer, new List<string>(), new JsonSerializer());
                }

                // Assert
                Assert.Equal("\"[]\"", result.ToString());
            }

            [Fact]
            public void Writes_Null_To_String()
            {
                // Arrange
                var result = new StringBuilder();
                var converter = new ProjectVersionConverter();
                using (var writer = new JsonTextWriter(new StringWriter(result)))
                {
                    // Act
                    converter.WriteJson(writer, null, new JsonSerializer());
                }

                // Assert
                Assert.Equal("\"[]\"", result.ToString());
            }
        }

        public class ReadJson
        {
            [Fact]
            public void Reads_String_To_Collection()
            {
                // Arrange
                List<string> result;
                var converter = new ProjectVersionConverter();
                using (var reader = new JsonTextReader(new StringReader("\"[0.0.1, 0.0.2  ]\"")))
                {
                    reader.Read();

                    // Act
                    result = converter.ReadJson(reader, typeof(ICollection<string>), 
                        new List<string>(), new JsonSerializer()) as List<string>;
                }

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Equal("0.0.1", result[0]);
                Assert.Equal("0.0.2", result[1]);
            }

            [Fact]
            public void Reads_Empty_Array_To_Collection()
            {
                // Arrange
                List<string> result;
                var converter = new ProjectVersionConverter();
                using (var reader = new JsonTextReader(new StringReader("\"[]\"")))
                {
                    reader.Read();

                    // Act
                    result = converter.ReadJson(reader, typeof(ICollection<string>),
                        new List<string>(), new JsonSerializer()) as List<string>;
                }

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }

            [Fact]
            public void Reads_Empty_String_To_Collection()
            {
                // Arrange
                List<string> result;
                var converter = new ProjectVersionConverter();
                using (var reader = new JsonTextReader(new StringReader("\"\"")))
                {
                    reader.Read();

                    // Act
                    result = converter.ReadJson(reader, typeof(ICollection<string>),
                        new List<string>(), new JsonSerializer()) as List<string>;
                }

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }
    }
}