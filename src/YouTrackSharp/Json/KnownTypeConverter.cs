using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Json {
  /// <summary>
  /// This <see cref="JsonConverter"/> allows to convert a JSON string into a concrete object that extends a given base
  /// type.<br/>
  /// This is used in conjunction with the <see cref="KnownTypeAttribute"/>, defined on
  /// the base class, and which lists the possible sub-types of that class.<br/><br/>
  /// Among these, the concrete sub-type instantiated is inferred from the Json object's "$type" field, which is
  /// compared to the defined known types (ignoring their namespace).<br/>
  /// If the "$type" field is undefined, or no <see cref="KnownTypeAttribute"/> matching the "$type" parameter is
  /// found, the base class is instantiated instead.  
  /// </summary>
  /// <typeparam name="T">Base type</typeparam>
  public class KnownTypeConverter<T> : JsonConverter<T> where T:new() {
    private readonly TypedJObjectConverter<T> _objectConverter;

    public KnownTypeConverter() {
      _objectConverter = new TypedJObjectConverter<T>();
    }
    
    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Reads the JSON representation of the object.<br/>
    /// This method will instanciate a subclass of the given <see cref="objectType"/>, based on the "$type" parameter
    /// of the json string, which is compared to <see cref="KnownTypeAttribute"/> defined for that base class.<br/>
    /// If the "$type" parameter is not defined, or no matching <see cref="KnownTypeAttribute"/> is found, the json is
    /// deserialized to an instance of the base class directly. 
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
    /// <param name="hasExistingValue">The existing value has a value.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer) {
      List<Type> types = typeof(T).GetCustomAttributes<KnownTypeAttribute>().Select(attr => attr.Type).ToList();

      JObject obj = JObject.Load(reader);

      return _objectConverter.ReadObject(obj, types, serializer);
    }
    
    /// <inheritdoc />
    public override bool CanRead => true;
    
    /// <inheritdoc />
    public override bool CanWrite => false;
  }
}