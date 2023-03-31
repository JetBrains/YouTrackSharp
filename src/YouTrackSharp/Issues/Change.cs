using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents a YouTrack issue change.
    /// </summary>
    public class Change
    {
        /// <summary>
        /// Creates an instance of the <see cref="Change" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="ActivityItem"/> to convert from.</param>
        internal static Change FromApiEntity(ActivityItem entity)
        {
            var fieldChange = new FieldChange();

            switch (entity)
            {
                case CustomFieldActivityItem a:
                    fieldChange.Name = a.Field.Name ?? a.Field.Presentation;

                    fieldChange.From.Value = a.Removed == null
                        ? null
                        :
                        a.Removed.GetType().GetInterface(nameof(ICollection)) == null
                            ?
                            new JArray() {JToken.FromObject(a.Removed)}
                            : JArray.FromObject(a.Removed);
                    fieldChange.To.Value = a.Added == null
                        ? null
                        :
                        a.Added.GetType().GetInterface(nameof(ICollection)) == null
                            ?
                            new JArray() {JToken.FromObject(a.Added)}
                            : JArray.FromObject(a.Added);
                    break;
                case IssueResolvedActivityItem a:
                    fieldChange.Name = "resolved";
                    fieldChange.From.Value = a.Removed == null ? null : new JArray() {JToken.FromObject(a.Removed)};
                    fieldChange.From.Value = a.Added == null ? null : new JArray() {JToken.FromObject(a.Added)};
                    break;
                case TextMarkupActivityItem a:
                    fieldChange.Name = a.Field.Name ?? a.Field.Presentation;
                    fieldChange.From.Value = a.Removed == null ? null : new JArray() {JToken.FromObject(a.Removed)};
                    fieldChange.To.Value = a.Added == null ? null : new JArray() {JToken.FromObject(a.Added)};
                    break;
                case VisibilityGroupActivityItem a:
                    fieldChange.Name = "permittedGroup";
                    fieldChange.From.Value = a.Removed == null ? new JArray() : JArray.FromObject(a.Removed);
                    fieldChange.To.Value = a.Added == null ? new JArray() : JArray.FromObject(a.Added);
                    break;
                case VisibilityUserActivityItem a:
                    fieldChange.Name = "permittedUser";
                    fieldChange.From.Value = a.Removed == null ? new JArray() : JArray.FromObject(a.Removed);
                    fieldChange.To.Value = a.Added == null ? new JArray() : JArray.FromObject(a.Added);
                    break;
                case SprintActivityItem a:
                    fieldChange.Name = a.Field.Name;
                    fieldChange.From.Value = a.Removed == null ? new JArray() : JArray.FromObject(a.Removed);
                    fieldChange.To.Value = a.Added == null ? new JArray() : JArray.FromObject(a.Added);
                    break;
                case LinksActivityItem a:
                    fieldChange.Name = a.Field.Presentation;
                    fieldChange.From.Value = a.Removed == null ? new JArray() : JArray.FromObject(a.Removed);
                    fieldChange.To.Value = a.Added == null ? new JArray() : JArray.FromObject(a.Added);
                    break;
                case AttachmentActivityItem a:
                    fieldChange.Name = a.Field.Presentation;
                    fieldChange.From.Value = a.Removed == null ? new JArray() : JArray.FromObject(a.Removed);
                    fieldChange.To.Value = a.Added == null ? new JArray() : JArray.FromObject(a.Added);
                    break;
                case TagsActivityItem a:
                    fieldChange.Name = "tags";
                    fieldChange.From.Value = a.Removed == null ? new JArray() : JArray.FromObject(a.Removed);
                    fieldChange.To.Value = a.Added == null ? new JArray() : JArray.FromObject(a.Added);
                    break;
                case VcsChangeActivityItem a:
                    fieldChange.Name = "vcs";
                    fieldChange.From.Value = a.Removed == null ? new JArray() : JArray.FromObject(a.Removed);
                    fieldChange.To.Value = a.Added == null ? new JArray() : JArray.FromObject(a.Added);
                    break;
                case ProjectActivityItem a:
                    fieldChange.Name = "project";
                    fieldChange.From.Value = a.Removed == null ? new JArray() : new JArray() {JObject.FromObject(a.Removed)};
                    fieldChange.To.Value = a.Added == null ? new JArray() : new JArray() {JObject.FromObject(a.Added)};
                    break;
            }
            
            var updaterName = new FieldChange();
            updaterName.Name = "updaterName";
            updaterName.To.Value = entity.Author?.Login;

            var updated = new FieldChange();
            updated.Name = "updated";
            updated.To.Value = entity.Timestamp;
            
            return new Change() {Fields = new List<FieldChange>(){fieldChange, updaterName, updated}};
        }
        
        /// <summary>
        /// Creates an instance of the <see cref="Change" /> class.
        /// </summary>
        public Change()
        {
            Fields = new List<FieldChange>();
        }
        
        /// <summary>
        /// Fields that have been changed.
        /// </summary>
        [JsonProperty("field")]
        public ICollection<FieldChange> Fields { get; set; }

        /// <summary>
        /// Get <see cref="FieldChange"/> for a field identified by <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">Name of the field to retrieve <see cref="FieldChange"/> for.</param>
        /// <returns><see cref="FieldChange"/> for a field identified by <paramref name="fieldName"/>. Can be <value>null</value>.</returns>
        public FieldChange ForField(string fieldName)
        {
            return Fields.FirstOrDefault(field => string.Equals(field.Name, fieldName, StringComparison.OrdinalIgnoreCase));
        }
    }
}