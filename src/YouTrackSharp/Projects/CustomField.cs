using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Projects
{
    /// <summary>
    /// Custom field for a project
    /// </summary>
    public class CustomField
    {
        /// <summary>
        /// Creates an instance of the <see cref="CustomField" /> class.
        /// </summary>
        public CustomField()
        {
            Name = string.Empty;
            Type = string.Empty;
            CanBeEmpty = false;
            EmptyText = string.Empty;
        }
        
        /// <summary>
        /// Creates an instance of the <see cref="CustomField" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="BundleProjectCustomField"/> to convert from.</param>
        internal static CustomField FromApiEntity(BundleProjectCustomField entity)
        {
            return new CustomField()
            {
                Id = entity.Id,
                Name = entity.Field.Name,
                Type = entity.Field.FieldType?.Id,
                CanBeEmpty = entity.CanBeEmpty ?? false,
                EmptyText = entity.EmptyFieldText
            };
        }
        
        /// <summary>
        /// Converts to instance of the <see cref="BundleProjectCustomField" /> class used in api client.
        /// </summary>
        internal BundleProjectCustomField ToApiEntity(ICollection<Generated.CustomField> allFields)
        {
            //TODO we allow fields with the same name but different types, so .Single() won't work
            var field = allFields.First(f => f.Name == Name);
            
            var fieldTypeId = field.FieldType.Id;
            var pos = fieldTypeId.IndexOf('[');
            if (pos >= 0)
            {
                fieldTypeId = fieldTypeId.Substring(0, pos);
            }

            BundleProjectCustomField bundleField = fieldTypeId switch
            {
                "enum" => new EnumProjectCustomField(),
                "build" => new BuildProjectCustomField(),
                "ownedField" => new OwnedProjectCustomField(),
                "version" => new VersionProjectCustomField(),
                "state" => new StateProjectCustomField(),
                "user" => new UserProjectCustomField(),
                "group" => new GroupProjectCustomField(),
                _ => new SimpleProjectCustomField()
            };
            bundleField.Field = field;
            bundleField.CanBeEmpty = CanBeEmpty;
            bundleField.EmptyFieldText = EmptyText;

            return bundleField;
        }

        /// <summary>
        ///  Database id of project custom field.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///  Name of project custom field.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Type of this custom field.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        ///  Mandatory binary parameter defining if the field can have empty value or not.
        /// </summary>
        [JsonProperty("canBeEmpty")]
        public bool CanBeEmpty { get; set; }

        /// <summary>
        ///  Text that is shown when the custom field has no value.
        /// </summary>
        [JsonProperty("emptyText")]
        public string EmptyText { get; set; }
    }
}