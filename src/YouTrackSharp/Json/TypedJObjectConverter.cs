using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackSharp.Json {
  public class TypedJObjectConverter<T> {
    public T ReadObject(JObject token, List<Type> knownTypes, JsonSerializer serializer) {
      if (token == null) throw new ArgumentException("Invalid token");

      string jsonType = token["$type"]?.ToString();
      if (jsonType == null) {
        return token.ToObject<T>(serializer);
      }

      Type type = knownTypes.FirstOrDefault(t => t.Name.EndsWith(jsonType));

      if (type == null) {
        return token.ToObject<T>(serializer);
      }

      return (T)token.ToObject(type, serializer);
    }
  }
}