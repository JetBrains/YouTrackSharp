using System.Collections.Generic;
using System.Linq;

namespace YouTrackSharp.Internal
{
    /// <summary>
    /// Simplified representation of an object's field.
    /// It describes the name of the field and its sub-fields.
    /// The 'subfields' are the members of the this field's type<br/>
    /// </summary>
    /// <remarks>
    /// This class allows to encode the recursive structure of an object's fields.
    /// An object's field has a type, which itself has other members, each being of a type, and so on, down to the
    /// primitive types.<br/>
    /// </remarks>
    public class Field
    {
        /// <summary>
        /// Name of the object's field
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Sub-fields of this field's type
        /// </summary>
        public List<Field> Subfields { get; }

        /// <summary>
        /// Creates an instance of <see cref="Field"/>, with the given name, type and subfields.
        /// </summary>
        /// <param name="name">Name of the field</param>
        /// <param name="subfields">Sub-fields of the field's type</param>
        public Field(string name, IEnumerable<Field> subfields)
        {
            Name = name;
            Subfields = subfields.ToList();
        }
    }
}