using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Xunit;
using Fixtures = YouTrackSharp.Tests.Json.KnownTypeConverterTestFixtures;

namespace YouTrackSharp.Tests.Json
{
    [UsedImplicitly]
    public class KnownTypeListConverterTests
    {
        public class ReadJson
        {
            [Fact]
            public void Deserialize_Type_With_Polymorphic_Collection()
            {
                string expectedName = "Example Name";
                int expectedId = 123;

                IList<Tuple<Type, int, string>> expectedChildren = new List<Tuple<Type, int, string>>();

                for (int i = 0; i < 10; i++)
                {
                    Type type = i % 2 == 0 ? typeof(Fixtures.ChildA) : typeof(Fixtures.ChildB);
                    expectedChildren.Add(new Tuple<Type, int, string>(type, i, $"Child {i}"));
                }

                Fixtures.Json fixtures = new Fixtures.Json();
                string json = fixtures.GetJsonForCompoundTypeWithList(expectedId, expectedName, expectedChildren);

                Fixtures.CompoundTypeWithList result =
                    JsonConvert.DeserializeObject<Fixtures.CompoundTypeWithList>(json);

                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
                Assert.Equal(expectedName, result.Name);

                Assert.NotNull(result.Children);
                foreach (Fixtures.BaseType child in result.Children)
                {
                    int id = child.Id;
                    Tuple<Type, int, string> expectedChild = expectedChildren.FirstOrDefault(c => c.Item2 == id);

                    Assert.NotNull(expectedChild);

                    Assert.Equal(expectedChild.Item1, child.GetType());

                    switch (child)
                    {
                        case Fixtures.ChildA childA:
                            Assert.Equal(expectedChild.Item3, childA.Name);
                            break;

                        case Fixtures.ChildB childB:
                            Assert.Equal(expectedChild.Item3, childB.Title);
                            break;

                        default:
                            Assert.True(false, "Child was not deserialized to recognizable type");
                            break;
                    }
                }
            }

            [Fact]
            public void Deserialize_Type_With_Polymorphic_Collection_Null()
            {
                string expectedName = "Example Name";
                int expectedId = 123;

                Fixtures.Json fixtures = new Fixtures.Json();
                string json = fixtures.GetJsonForCompoundTypeWithNullList(expectedId, expectedName);

                Fixtures.CompoundTypeWithList result =
                    JsonConvert.DeserializeObject<Fixtures.CompoundTypeWithList>(json);

                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
                Assert.Equal(expectedName, result.Name);

                Assert.Null(result.Children);
            }

            [Fact]
            public void Deserialize_Type_With_Polymorphic_Collection_Empty()
            {
                string expectedName = "Example Name";
                int expectedId = 123;

                Fixtures.Json fixtures = new Fixtures.Json();
                string json = fixtures.GetJsonForCompoundTypeWithEmptyList(expectedId, expectedName);

                Fixtures.CompoundTypeWithList result =
                    JsonConvert.DeserializeObject<Fixtures.CompoundTypeWithList>(json);

                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
                Assert.Equal(expectedName, result.Name);
                Assert.NotNull(result.Children);
                Assert.Empty(result.Children);
            }
        }
    }
}