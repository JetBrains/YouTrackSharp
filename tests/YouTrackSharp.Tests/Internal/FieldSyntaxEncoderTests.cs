using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.Internal;

namespace YouTrackSharp.Tests.Internal
{
    [UsedImplicitly]
    public class FieldSyntaxEncoderTests
    {
        public class UrlEncoding
        {
            [Fact]
            public void Encode_Flat_Type()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.FlatType));

                string expected = "id,name,description";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Flat_Type_Non_Verbose()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.FlatType), false);

                string expected = "id,name";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Single_Nested_Types()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.NestedTypes));

                string expected = "id,name,info(id,address,phoneNumber)";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Deep_Nested_Types()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.DeepNestedTypes));

                string expected = "id,name,contact(firstName,lastName,info(id,address,phoneNumber),id)";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Nested_Types_With_Inheritance_Merges_Common_Field_Info()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.DeepNestedTypesWithInheritance));

                string expected = "id,name,contact(id,fullName,info(id,address,phoneNumber),firstName,lastName)";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Nested_Types_With_Collection()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.DeepNestedTypesWithCollection));

                string expected = "id,name,contacts(id,fullName,info(id,address,phoneNumber),firstName,lastName)";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Nested_Types_Max_Depth_0()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.DeepNestedTypes), true, 0);

                string expected = string.Empty;

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Nested_Types_Max_Depth_1()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.DeepNestedTypes), true, 1);

                string expected = "id,name,contact";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Nested_Types_Max_Depth_2()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.DeepNestedTypes), true, 2);

                string expected = "id,name,contact(firstName,lastName,info,id)";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Nested_Types_Max_Depth_3()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.DeepNestedTypes), true, 3);

                string expected = "id,name,contact(firstName,lastName,info(id,address,phoneNumber),id)";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Flat_Type_Not_Verbose()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.FlatType), false);

                string expected = "id,name";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Encode_Nested_Type_Not_Verbose()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.DeepNestedTypes), false);

                string expected = "id,contact(firstName,lastName,id)";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Control_Cyclic_References_With_Max_Depth()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.CyclicReference), true, 3);

                string expected = "id,name,cyclic(id,name,cyclic(id,name,cyclic))";

                Assert.Equal(expected, encoded);
            }

            [Fact]
            public void Skip_Cyclic_References_With_Verbose_Disabled()
            {
                FieldSyntaxEncoder encoder = new FieldSyntaxEncoder();
                string encoded = encoder.Encode(typeof(FieldSyntaxEncoderTestFixtures.CyclicReference), false);

                string expected = "id,name";

                Assert.Equal(expected, encoded);
            }
        }
    }
}