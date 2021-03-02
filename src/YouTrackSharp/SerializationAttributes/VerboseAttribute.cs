using System;

namespace YouTrackSharp.SerializationAttributes {
  /// <summary>
  /// This attribute is used to mark a property as being only retrieved when verbose information was requested. 
  /// </summary>
  [AttributeUsage(AttributeTargets.Property)]
  public class VerboseAttribute : Attribute { }
}