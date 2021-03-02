using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Internal {
  /// <summary>
  /// Used to convert a given <see cref="Type"/> to the Youtrack REST API's 'fields' syntax, described at the following
  /// location: <a href="https://www.jetbrains.com/help/youtrack/standalone/api-fields-syntax.html">
  /// https://www.jetbrains.com/help/youtrack/standalone/api-fields-syntax.html
  /// </a>.
  /// </summary>
  public class FieldSyntaxEncoder {
    /// <summary>
    /// Cached used to store types that were already resolved.
    /// </summary>
    /// <remarks>
    /// The result of the field query can vary depending on the verbose option and the max nesting level.
    /// Therefore, the key used is a combination of the <see cref="Type"/>, verbosity and max depth.
    /// </remarks>
    private Dictionary<Tuple<Type, bool, int>, string> Cache { get; }

    /// <summary>
    /// Creates an instance of <see cref="FieldSyntaxEncoder"/>
    /// </summary>
    public FieldSyntaxEncoder() {
      Cache = new Dictionary<Tuple<Type, bool, int>, string>();
    }

    /// <summary>
    /// Encodes the given <see cref="Type"/>'s members to the Youtrack REST API's 'fields'-syntax, described at
    /// <a href="https://www.jetbrains.com/help/youtrack/standalone/api-fields-syntax.html">
    /// https://www.jetbrains.com/help/youtrack/standalone/api-fields-syntax.html
    /// </a>.<br/>
    /// This method uses reflexion to retrieve and encodes all public writable properties, decorated with a
    /// <see cref="JsonPropertyAttribute"/>.<br/>
    /// It also concatenates the fields retrieved from known subclasses of the given type (defined by the
    /// <see cref="KnownTypeAttribute"/>).<br/>
    /// Because the base class and the different subclasses may have fields with similar names (but different types and
    /// sub-fields), all the fields discovered this way are "factored" together. Therefore, if two known subclasses both
    /// have a field with the same name, but each with different types and so, different sub-fields, the two fields will
    /// be factored into one containing the union of their respective sub-fields.<br/>
    /// For example:<br/>
    /// - The first subclass has a field named A with subfields B, C and D, ie. A(B,C,D)<br/>
    /// - The second subclass also has a field A but with subfields D,E and F, ie. A(D,E,F)<br/>
    /// In that case, these two fields will be combined into one, with name A and subfields B, C, D, E and F, ie.
    /// A(B,C,D,E,F). Note that D appears only once.
    /// </summary>
    /// <param name="type">Type to encode</param>
    /// <param name="verbose">
    /// Verbosity. If <c>true</c>, will include all public writable properties of the given <see cref="Type"/>, marked
    /// with the <see cref="JsonPropertyAttribute"/>.
    /// But if <c>false</c>, it will exclude from these all the ones that were marked with the
    /// <see cref="VerboseAttribute"/>.
    /// </param>
    /// <param name="maxDepth">Max sub-field depth to go from the given type.</param>
    /// <returns>Fields of the given type, encoded to Youtrack
    /// <a href="https://www.jetbrains.com/help/youtrack/standalone/api-fields-syntax.html">REST API's 'field' syntax
    /// </a>
    /// </returns>
    /// <remarks>
    /// This method does not work well with types that have (direct or indirect) reference loops.
    /// For example, a class A that contains two fields of type B and C, where the type B itself contains a field of
    /// type A. This would result in a stack-overflow.
    /// </remarks>
    public string Encode(Type type, bool verbose = true, int maxDepth = Int32.MaxValue) {
      type = GetLeafType(type);
      
      Tuple<Type, bool, int> key = new Tuple<Type, bool, int>(type, verbose, maxDepth);

      if (Cache.ContainsKey(key)) return Cache[key];

      IEnumerable<Field> fields = GetFields(type, verbose, maxDepth, 0);
      
      Cache[key] = string.Join(",", fields.Select(ToString));

      return Cache[key];
    }

    /// <summary>
    /// Converts the <see cref="Field"/> recursive structure into a string representation, following Youtrack REST API's
    /// <a href="https://www.jetbrains.com/help/youtrack/standalone/api-fields-syntax.html">'field' syntax</a>
    /// Example: lines(fromPoint(x,y),toPoint(x,y)),color
    /// </summary>
    /// <param name="field"><see cref="Field"/> to convert</param>
    /// <returns></returns>
    private string ToString(Field field) {
      if (!field.Subfields.Any()) return field.Name;

      string subfields = string.Join(",", field.Subfields.Select(ToString));

      return $"{field.Name}({subfields})";
    }

    /// <summary>
    /// Recursive method which builds the <see cref="Field"/> structure for the given <see cref="Type"/>'s properties. 
    /// This method uses reflexion to convert all the public writable properties of teh given <see cref="Type"/>,
    /// decorated with a <see cref="JsonPropertyAttribute"/> to a <see cref="Field"/><br/>.
    /// It also retrieves the fields from known subclasses of the given type (defined by the
    /// <see cref="KnownTypeAttribute"/>).<br/>
    /// Because the base class and the different subclasses may have fields with similar names (but different types and
    /// sub-fields), all the fields discovered this way are "factored" together. Therefore, if two known subclasses both
    /// have a field with the same name, but different type, each with different sub-fields, the two fields will
    /// be factored into one containing the union of their respective sub-fields.<br/>
    /// For example:<br/>
    /// - The first subclass has a field named A with subfields B, C and D, ie. A(B,C,D)<br/>
    /// - The second subclass also has a field A but with subfields D,E and F, ie. A(D,E,F)<br/>
    /// In that case, these two fields will be combined into one, with name A and subfields B, C, D, E and F, ie.
    /// A(B,C,D,E,F). Note that D appears only once.
    /// </summary>
    /// <param name="type">Type to convert</param>
    /// <param name="verbose">
    /// Verbosity. If <c>true</c>, will include all public writable properties of the given <see cref="Type"/>, marked
    /// with the <see cref="JsonPropertyAttribute"/>.
    /// But if <c>false</c>, it will exclude from these all the ones that were marked with the
    /// <see cref="VerboseAttribute"/>.
    /// </param>
    /// <param name="maxDepth">Max sub-field depth to go from the given type.</param>
    /// <param name="level">Current sub-field level (to compare with <see cref="maxDepth"/>)</param>
    /// <returns>Fields of the given type, encoded to Youtrack
    /// <a href="https://www.jetbrains.com/help/youtrack/standalone/api-fields-syntax.html">REST API's 'field' syntax
    /// </a>
    /// </returns>
    /// <remarks>
    /// This method does not work well with types that have (direct or indirect) reference loops.
    /// For example, a class A that contains two fields of type B and C, where the type B itself contains a field of
    /// type A. This would result in a stack-overflow.
    /// </remarks>
    private IEnumerable<Field> GetFields(Type type, bool verbose, int maxDepth, int level) {
      if (level == maxDepth) return Enumerable.Empty<Field>();

      List<Field> fields = new List<Field>();
      
      IEnumerable<PropertyInfo> properties = GetProperties(type, verbose);
      foreach (PropertyInfo propertyInfo in properties) {
        Type propertyType = GetLeafType(propertyInfo.PropertyType);
        string propertyName = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
        
        IEnumerable<Field> subfields = GetFields(propertyType, verbose, maxDepth, level + 1);

        fields.Add(new Field(propertyName, subfields));
      }

      return Merge(fields);
    }

    /// <summary>
    /// Merges the given fields into a factored representation, where fields with the same name are combined into one.
    /// For example, if there are two fields named "A"<br/>
    /// - The first with subfields B, C and D, ie. A(B,C,D)<br/>
    /// - The second with subfields D,E and F, ie. A(D,E,F)<br/>
    /// These two fields will be combined into one, with name A and subfields B, C, D, E and F, ie.
    /// A(B,C,D,E,F). Note that D appears only once. 
    /// </summary>
    /// <param name="fields"><see cref="Field"/>s to factor</param>
    /// <returns>Factored fields</returns>
    private IEnumerable<Field> Merge(IEnumerable<Field> fields) {
      IEnumerable<IGrouping<string, Field>> groups = fields.GroupBy(field => field.Name);

      foreach (IGrouping<string, Field> group in groups) {
        IEnumerable<Field> subfields = group.SelectMany(f => f.Subfields);
        subfields = Merge(subfields);

        yield return new Field(group.Key, subfields);
      }
    }

    /// <summary>
    /// Retrieves the <see cref="PropertyInfo"/> of all public, writable properties marked with a
    /// <see cref="JsonPropertyAttribute"/> of the given <see cref="Type"/> and
    /// all of its known subclasses (defined by the <see cref="KnownTypeAttribute"/>.
    /// If <see cref="verbose"/> is set to <c>false</c>, it will exclude the ones marked with a
    /// <see cref="VerboseAttribute"/>. 
    /// </summary>
    /// <param name="type"><see cref="Type"/> from which to retrieve the properties</param>
    /// <param name="verbose">
    /// If <c>false</c> the properties marked with <see cref="KnownTypeAttribute"/> will be excluded from the results. 
    /// </param>
    /// <returns>
    /// The <see cref="PropertyInfo"/> of all public, writable properties marked with a
    /// <see cref="JsonPropertyAttribute"/> of the given <see cref="Type"/> and
    /// all of its known subclasses.
    /// </returns>
    private IEnumerable<PropertyInfo> GetProperties(Type type, bool verbose) {
      PropertyInfo[] properties = type.GetProperties();
      
      IEnumerable<PropertyInfo> propertyInfos = properties.Where(p => p.CanWrite)
                                                          .Where(p => p.GetCustomAttribute<JsonPropertyAttribute>() != null);

      if (!verbose) {
        propertyInfos = propertyInfos.Where(p => p.GetCustomAttribute<VerboseAttribute>() == null);
      }

      IEnumerable<Type> knownSubtypes = type.GetCustomAttributes<KnownTypeAttribute>(false).Select(attr => attr.Type);
      IEnumerable<PropertyInfo> subtypesProperties =
        knownSubtypes.SelectMany(subtype => GetProperties(subtype, verbose));

      return propertyInfos.Concat(subtypesProperties);
    }

    /// <summary>
    /// Returns the "leaf" type argument of any generic type.<br/>
    /// <i>Note: Generic types are nested, the "leaf" meaning the non-generic type at the bottom of the nesting chain</i>
    /// <br/>
    /// For example, a <see cref="List{T}"/> of <see cref="string"/>, will yield <see cref="string"/>.
    /// And a <see cref="List{T}"/> of <see cref="List{T}"/> of <see cref="string"/>
    /// will also yield <see cref="string"/>.<br/>
    /// <br/>
    /// For generic types with multiple type parameters, it will always follow the last, which is usually the value type.
    /// For example, a <see cref="Dictionary{TKey,TValue}"/> of key <see cref="string"/> and value <see cref="int"/>,
    /// will yield <see cref="int"/>.<br/>
    /// A <see cref="Dictionary{TKey,TValue}"/> of key <see cref="string"/> and value <see cref="List{T}"/> of
    /// <see cref="int"/>, will also yield <see cref="int"/>.
    /// </summary>
    /// <param name="type">Input <see cref="Type"/></param>
    /// <returns>Leaf type if the input type is generic, otherwise itself</returns>
    /// <remarks>
    /// The returned type is guaranteed to be non-generic.
    /// </remarks>
    private Type GetLeafType(Type type) {
      if (!type.IsGenericType) return type;

      return GetLeafType(type.GetGenericArguments().Last());
    }
  }
}