using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using YouTrackSharp.Json;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Tests.Json {
  [UsedImplicitly]
  public class KnownTypeConverterTestFixtures {
    public class Json {
      public string GetJsonForChildA(int id, string name) {
        return $"{{ \"id\": {id}, \"name\": \"{name}\", \"$type\": \"ChildA\" }}";
      }

      public string GetJsonForChildB(int id, string title) {
        return $"{{ \"id\": {id}, \"title\": \"{title}\", \"$type\": \"ChildB\" }}";
      }

      public string GetJsonForUnknownType(int id, string title) {
        return $"{{ \"id\": {id}, \"title\": \"{title}\", \"$type\": \"UnknownType\" }}";
      }

      public string GetJsonForUnspecifiedType(int id, string title) {
        return $"{{ \"id\": {id}, \"title\": \"{title}\" }}";
      }

      public string GetJsonForCompoundType(int id, string name, int childId, string childName) {
        string childJson = GetJsonForChildA(childId, childName);

        return $"{{ \"id\": {id}, \"name\": \"{name}\", \"child\": {childJson}, \"$type\": \"CompoundType\" }}";
      }
    }

    public class CompoundType {
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
    public class BaseType {
      [JsonProperty("id")]
      public int Id { get; set; }
    }

    public class ChildA : BaseType {
      [JsonProperty("name")]
      public string Name { get; set; }
    }

    public class ChildB : BaseType {
      [JsonProperty("title")]
      public string Title { get; set; }
    }
  }
}