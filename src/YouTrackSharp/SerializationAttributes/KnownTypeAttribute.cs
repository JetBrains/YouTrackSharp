using System;

namespace YouTrackSharp.SerializationAttributes {
  /// <summary>
  /// Attribute used to decorate a parent class with its known subtypes.<br/>
  /// This is used to identify candidate subclasses when generating REST requests (to include the subclasses'
  /// properties).
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class KnownTypeAttribute : Attribute {
    /// <summary>
    /// Known subclass's type
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Creates an instance of the <see cref="KnownTypeAttribute"/>, with the given subclass' type.
    /// </summary>
    /// <param name="type"></param>
    public KnownTypeAttribute(Type type) {
      Type = type;
    }
  }
}