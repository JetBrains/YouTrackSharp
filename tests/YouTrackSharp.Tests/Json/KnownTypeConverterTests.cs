using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Xunit;
using YouTrackSharp.Json;
using Fixtures = YouTrackSharp.Tests.Json.KnownTypeConverterTestFixtures;

namespace YouTrackSharp.Tests.Json
{
    [UsedImplicitly]
    public class KnownTypeConverterTests
    {
        public class ReadJson
        {
            [Fact]
            public void Identifies_Correct_Subtype_ChildTypeA()
            {
                KnownTypeConverter<Fixtures.BaseType> converter = new KnownTypeConverter<Fixtures.BaseType>();
                Fixtures.Json fixtures = new Fixtures.Json();

                string expectedName = "Example Name";
                int expectedId = 123;

                using JsonTextReader reader =
                    new JsonTextReader(new StringReader(fixtures.GetJsonForChildA(expectedId, expectedName)));
                reader.Read();

                // Act
                Fixtures.ChildA result =
                    converter.ReadJson(reader, typeof(Fixtures.BaseType), null,
                                       new JsonSerializer()) as Fixtures.ChildA;

                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
                Assert.Equal(expectedName, result.Name);
            }

            [Fact]
            public void Identifies_Correct_Subtype_ChildTypeB()
            {
                KnownTypeConverter<Fixtures.BaseType> converter = new KnownTypeConverter<Fixtures.BaseType>();
                Fixtures.Json fixtures = new Fixtures.Json();

                string expectedTitle = "Example Title";
                int expectedId = 123;

                using JsonTextReader reader =
                    new JsonTextReader(new StringReader(fixtures.GetJsonForChildB(expectedId, expectedTitle)));
                reader.Read();

                // Act
                Fixtures.ChildB result =
                    converter.ReadJson(reader, typeof(Fixtures.BaseType), null,
                                       new JsonSerializer()) as Fixtures.ChildB;

                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
                Assert.Equal(expectedTitle, result.Title);
            }

            [Fact]
            public void Unknown_Subtype_Defaults_To_BaseType()
            {
                KnownTypeConverter<Fixtures.BaseType> converter = new KnownTypeConverter<Fixtures.BaseType>();
                Fixtures.Json fixtures = new Fixtures.Json();

                string expectedTitle = "Example Title";
                int expectedId = 123;

                using JsonTextReader reader =
                    new JsonTextReader(new StringReader(fixtures.GetJsonForUnknownType(expectedId, expectedTitle)));
                reader.Read();

                // Act
                Fixtures.BaseType result =
                    converter.ReadJson(reader, typeof(Fixtures.BaseType), null, new JsonSerializer()) as
                        Fixtures.BaseType;

                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
            }

            [Fact]
            public void Unspecified_Type_Defaults_To_BaseType()
            {
                KnownTypeConverter<Fixtures.BaseType> converter = new KnownTypeConverter<Fixtures.BaseType>();
                Fixtures.Json fixtures = new Fixtures.Json();

                string expectedTitle = "Example Title";
                int expectedId = 123;

                using JsonTextReader reader =
                    new JsonTextReader(new StringReader(fixtures.GetJsonForUnspecifiedType(expectedId, expectedTitle)));
                reader.Read();

                // Act
                Fixtures.BaseType result =
                    converter.ReadJson(reader, typeof(Fixtures.BaseType), null, new JsonSerializer()) as
                        Fixtures.BaseType;

                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
            }

            [Fact]
            public void Deserialize_Type_With_Polymorphic_Field()
            {
                string expectedName = "Example Name";
                int expectedId = 123;

                string expectedChildName = "Child Name";
                int expectedChildId = 456;

                Fixtures.Json fixtures = new Fixtures.Json();
                string json =
                    fixtures.GetJsonForCompoundType(expectedId, expectedName, expectedChildId, expectedChildName);

                Fixtures.CompoundType result = JsonConvert.DeserializeObject<Fixtures.CompoundType>(json);


                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
                Assert.Equal(expectedName, result.Name);

                Fixtures.ChildA child = result.Child as Fixtures.ChildA;
                Assert.NotNull(child);
                Assert.Equal(expectedChildId, child.Id);
                Assert.Equal(expectedChildName, child.Name);
            }

            [Fact]
            public void Deserialize_Type_With_Null_Polymorphic_Field()
            {
                string expectedName = "Example Name";
                int expectedId = 123;

                Fixtures.Json fixtures = new Fixtures.Json();
                string json = fixtures.GetJsonForCompoundTypeWithNullField(expectedId, expectedName);

                Fixtures.CompoundType result = JsonConvert.DeserializeObject<Fixtures.CompoundType>(json);


                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
                Assert.Equal(expectedName, result.Name);

                Fixtures.ChildA child = result.Child as Fixtures.ChildA;
                Assert.Null(child);
            }
        }
    }
}