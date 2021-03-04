using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using YouTrackSharp.Json;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Tests.Json
{
    [UsedImplicitly]
    public class KnownTypeConverterTestFixtures
    {
        public class Json
        {
            /// <summary>
            /// Creates a Json string representation of a <see cref="ChildA"/> instance, with given id and name.
            /// </summary>
            /// <param name="id">Id</param>
            /// <param name="name">Name</param>
            /// <returns>Json representation of <see cref="ChildA"/> with given id and name</returns>
            public string GetJsonForChildA(int id, string name)
            {
                return $"{{ \"id\": {id}, \"name\": \"{name}\", \"$type\": \"ChildA\" }}";
            }

            /// <summary>
            /// Creates a Json string representation of a <see cref="ChildB"/> instance, with given id and title.
            /// </summary>
            /// <param name="id">Id</param>
            /// <param name="title">Title</param>
            /// <returns>Json representation of <see cref="ChildB"/> with given id and title</returns>
            public string GetJsonForChildB(int id, string title)
            {
                return $"{{ \"id\": {id}, \"title\": \"{title}\", \"$type\": \"ChildB\" }}";
            }

            /// <summary>
            /// Creates a Json string representation of an unknown sub-type of <see cref="BaseType"/> (unknown to
            /// serialization), with given id and title.
            /// </summary>
            /// <param name="id">Id of object</param>
            /// <param name="title">Title of object</param>
            /// <returns>Json representation of an unknown sub-type of <see cref="BaseType"/></returns>
            public string GetJsonForUnknownType(int id, string title)
            {
                return $"{{ \"id\": {id}, \"title\": \"{title}\", \"$type\": \"UnknownType\" }}";
            }

            /// <summary>
            /// Creates a Json string representation of an object, with no "$type" field, with given id and title. 
            /// </summary>
            /// <param name="id">Id of object</param>
            /// <param name="title">Title of object</param>
            /// <returns>Json representation of an untyped object</returns>
            public string GetJsonForUnspecifiedType(int id, string title)
            {
                return $"{{ \"id\": {id}, \"title\": \"{title}\" }}";
            }

            /// <summary>
            /// Creates a Json representation of a <see cref="CompoundType"/>, with given id, name.<br/>
            /// The <see cref="CompoundType.Child"/> instance is a json representation of  concrete type <see cref="ChildA"/>,
            /// with given child id and child name.
            /// </summary>
            /// <param name="id">Id</param>
            /// <param name="name">Name</param>
            /// <param name="childId">Id of child</param>
            /// <param name="childName">Name of child</param>
            /// <returns>
            /// Json representation of <see cref="CompoundType"/>, with <see cref="ChildA"/> instance variable.
            /// </returns>
            public string GetJsonForCompoundType(int id, string name, int childId, string childName)
            {
                string childJson = GetJsonForChildA(childId, childName);

                return $"{{ \"id\": {id}, \"name\": \"{name}\", \"child\": {childJson}, \"$type\": \"CompoundType\" }}";
            }

            /// <summary>
            /// Creates a Json representation of a <see cref="CompoundType"/>, with given id, name, but null child<br/>
            /// </summary>
            /// <param name="id">Id</param>
            /// <param name="name">Name</param>
            /// <returns>
            /// Json representation of <see cref="CompoundType"/>, with null child.
            /// </returns>
            public string GetJsonForCompoundTypeWithNullField(int id, string name)
            {
                return $"{{ \"id\": {id}, \"name\": \"{name}\", \"child\": null, \"$type\": \"CompoundType\" }}";
            }

            /// <summary>
            /// Creates Json representation of <see cref="CompoundTypeWithList"/>, with given id and name.
            /// The <see cref="CompoundTypeWithList.Children"/> is a list made of multiple <see cref="ChildA"/> and
            /// <see cref="ChildB"/> instances, created from the given <see cref="children"/> enumerable.
            /// This enumerable contains a tuple per child to create, with the concrete type to use (<see cref="ChildA"/> or
            /// <see cref="ChildB"/>, the child id and its name).
            /// </summary>
            /// <param name="id">Id</param>
            /// <param name="name">Name</param>
            /// <param name="children">Children specifications</param>
            /// <returns>
            /// Json representation of <see cref="CompoundTypeWithList"/>, with array of children of types
            /// <see cref="ChildA"/> or <see cref="ChildB"/>.
            /// </returns>
            public string GetJsonForCompoundTypeWithList(int id, string name,
                                                         IEnumerable<Tuple<Type, int, string>> children)
            {
                IEnumerable<string> childrenJson =
                    children.Select(child => GetJsonForChild(child.Item1, child.Item2, child.Item3));

                string childrenJsonArray = string.Join(", ", childrenJson);

                return $"{{ \"id\": {id}, \"name\": \"{name}\", \"children\": " +
                       $"[{childrenJsonArray}], \"$type\": \"CompoundTypeWithList\" }}";
            }

            /// <summary>
            /// Creates json representation of <see cref="CompoundTypeWithList"/>, with given id and name, but
            /// with children field set to <c>null</c>.
            /// </summary>
            /// <param name="id">Id</param>
            /// <param name="name">Name</param>
            /// <returns>
            /// Json representation of <see cref="CompoundTypeWithList"/> with <c>null</c> children.
            /// </returns>
            public string GetJsonForCompoundTypeWithNullList(int id, string name)
            {
                return
                    $"{{ \"id\": {id}, \"name\": \"{name}\", \"children\": null, \"$type\": \"CompoundTypeWithList\" }}";
            }

            /// <summary>
            /// Creates json representation of <see cref="CompoundTypeWithList"/>, with given id and name, but
            /// with the children field set to an empty array.
            /// </summary>
            /// <param name="id">Id</param>
            /// <param name="name">Name</param>
            /// <returns>
            /// Json representation of <see cref="CompoundTypeWithList"/> with empty children array.
            /// </returns>
            public string GetJsonForCompoundTypeWithEmptyList(int id, string name)
            {
                return
                    $"{{ \"id\": {id}, \"name\": \"{name}\", \"children\": [], \"$type\": \"CompoundTypeWithList\" }}";
            }

            /// <summary>
            /// Creates Json representation of <see cref="ChildA"/> or <see cref="ChildB"/>, depending on the given
            /// <see cref="Type"/> 
            /// </summary>
            /// <param name="concreteType">Concrete type (<see cref="ChildA"/> or <see cref="ChildB"/>)</param>
            /// <param name="id">Id of child</param>
            /// <param name="nameOrTitle">Name (for <see cref="ChildA"/>) or Title (for <see cref="ChildB"/>)</param>
            /// <returns>Json representation of given <see cref="Type"/></returns>
            private string GetJsonForChild(Type concreteType, int id, string nameOrTitle)
            {
                if (concreteType == typeof(ChildA))
                {
                    return GetJsonForChildA(id, nameOrTitle);
                }

                return GetJsonForChildB(id, nameOrTitle);
            }
        }

        public class CompoundTypeWithList
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("children")]
            [JsonConverter(typeof(KnownTypeListConverter<BaseType>))]
            public List<BaseType> Children { get; set; }
        }

        public class CompoundType
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("child")]
            [JsonConverter(typeof(KnownTypeConverter<BaseType>))]
            public BaseType Child { get; set; }
        }

        [KnownType(typeof(ChildA))]
        [KnownType(typeof(ChildB))]
        public class BaseType
        {
            [JsonProperty("id")]
            public int Id { get; set; }
        }

        public class ChildA : BaseType
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class ChildB : BaseType
        {
            [JsonProperty("title")]
            public string Title { get; set; }
        }
    }
}