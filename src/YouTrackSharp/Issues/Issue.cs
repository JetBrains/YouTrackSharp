using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YouTrackSharp.Json;

namespace YouTrackSharp.Issues
{
    // TODO: Add dynamic object implementation cache so no iteration is needed over Fields

    /// <summary>
    /// A class that represents YouTrack issue information. Can be casted to a <see cref="DynamicObject"/>.
    /// </summary>
    [DebuggerDisplay("{Id}: {Summary}")]
    public class Issue 
        : DynamicObject
    {
        private readonly IDictionary<string, Field> _fields = new Dictionary<string, Field>(StringComparer.OrdinalIgnoreCase);
        
        /// <summary>
        /// Creates an instance of the <see cref="Issue" /> class.
        /// </summary>
        public Issue()
        {
            Comments = new List<Comment>();
            Tags = new List<SubValue<string>>();
        }

        /// <summary>
        /// Id of the issue.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Entity Id internal to YouTrack.
        /// </summary>
        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        /// <summary>
        /// If issue was imported from JIRA, represents the Id it has in JIRA.
        /// </summary>
        [JsonProperty("jiraId")]
        public string JiraId { get; set; }

        /// <summary>
        /// Summary of the issue.
        /// </summary>
        public string Summary {
            get
            {
                var field = GetField("Summary");
                return field?.Value.ToString();
            }
            set => SetField("Summary", value);
        }

        /// <summary>
        /// Description of the issue.
        /// </summary>
        public string Description {
            get
            {
                var field = GetField("Description");
                return field?.Value.ToString();
            }
            set => SetField("Description", value);
        }

        /// <summary>
        /// Is the issue description in Markdown format?
        /// </summary>
        /// <remarks>Setting the format to Markdown is supported in YouTrack versions 2018.2 and later.</remarks>
        public bool IsMarkdown {
            get
            {
                var field = GetField("markdown");
                return field != null && field.AsBool();
            }
            set => SetField("markdown", value.ToString().ToLowerInvariant());
        }
        
        /// <summary>
        /// Issue fields.
        /// </summary>
        public ICollection<Field> Fields => _fields.Values;

        /// <summary>
        /// Issue comments.
        /// </summary>
        [JsonProperty("comment")]
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Issue tags.
        /// </summary>
        [JsonProperty("tag")]
        public ICollection<SubValue<string>> Tags { get; set; }

        /// <summary>
        /// Gets a specific <see cref="Field"/> from the <see cref="Fields"/> collection.
        /// </summary>
        /// <param name="fieldName">The name of the <see cref="Field"/> to get.</param>
        /// <returns><see cref="Field"/> matching the <paramref name="fieldName"/>; null when not found.</returns>
        public Field GetField(string fieldName)
        {
            _fields.TryGetValue(fieldName, out var field);
            return field;
        }

        /// <summary>
        /// Sets a specific <see cref="Field"/> in the <see cref="Fields"/> collection.
        /// </summary>
        /// <param name="fieldName">The name of the <see cref="Field"/> to set.</param>
        /// <param name="value">The value to set for the <see cref="Field"/>.</param>
        public void SetField(string fieldName, object value)
        {
            if (_fields.TryGetValue(fieldName, out var field))
            {
                field.Value = value;
            }
            else
            {
                _fields.Add(fieldName, new Field { Name = fieldName, Value = value });
            }
        }
        
        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var field = GetField(binder.Name) 
                ?? GetField(binder.Name.Replace("_", " ")); // support fields with space in the name by using underscore in code
            
            if (field != null)
            {
                result = field.Value;
                return true;
            }

            result = null;
            return true;
        }

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // "field" setter when deserializing JSON into Issue object
            if (string.Equals(binder.Name, "field", StringComparison.OrdinalIgnoreCase) && value is JArray)
            {   
                var fieldElements = ((JArray)value).ToObject<List<Field>>();
                foreach (var fieldElement in fieldElements)
                {
                    if (fieldElement.Value is JArray fieldElementAsArray)
                    {
                        // Map collection
                        if (string.Equals(fieldElement.Name, "assignee", StringComparison.OrdinalIgnoreCase))
                        {
                            // For assignees, we can do a strong-typed list.
                            fieldElement.Value = fieldElementAsArray.ToObject<List<Assignee>>();
                        }
                        else
                        {
                            if (fieldElementAsArray.First is JValue &&
                                JTokenTypeUtil.IsSimpleType(fieldElementAsArray.First.Type))
                            {
                                // Map simple arrays to a collection of string
                                fieldElement.Value = fieldElementAsArray.ToObject<List<string>>();
                            }
                            else
                            {
                                // Map more complex arrays to JToken[]
                                fieldElement.Value = fieldElementAsArray;
                            }
                        }
                    }
                    
                    // Set the actual field
                    _fields[fieldElement.Name] = fieldElement;
                }
             
                return true;
            }
            
            // Regular setter
            if (_fields.TryGetValue(binder.Name, out var field) || _fields.TryGetValue(binder.Name.Replace("_", " "), out field))
            {
                field.Value = value;
            }
            else
            {
                _fields.Add(binder.Name, new Field { Name = binder.Name, Value = value });
            }
            
            return true;
        }

        /// <summary>
        /// Returns the current <see cref="Issue" /> as a <see cref="DynamicObject" />.
        /// </summary>
        /// <returns>The current <see cref="Issue" /> as a <see cref="DynamicObject" />.</returns>
        public dynamic AsDynamic()
        {
            return this;
        }
    }
}