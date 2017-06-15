using System;
using System.Collections.Generic;
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
            Tags = new List<string>();
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
        /// Issue fields.
        /// </summary>
        public ICollection<Field> Fields
        {
            get { return _fields.Values; } 
        }

        /// <summary>
        /// Issue comments.
        /// </summary>
        [JsonProperty("comment")]
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Issue tags.
        /// </summary>
        [JsonProperty("tag")]
        public ICollection<string> Tags { get; set; }

        /// <summary>
        /// Gets a specific <see cref="Field"/> from the <see cref="Fields"/> collection.
        /// </summary>
        /// <param name="fieldName">The name of the <see cref="Field"/> to get.</param>
        /// <returns><see cref="Field"/> matching the <paramref name="fieldName"/>; null when not found.</returns>
        public Field GetField(string fieldName)
        {
            Field field;
            _fields.TryGetValue(fieldName, out field);
            return field;
        }

        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var field = GetField(binder.Name);
            if (field != null)
            {
                result = field.Value;
                return true;
            }
            
            return base.TryGetMember(binder, out result);
        }

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (string.Equals(binder.Name, "field", StringComparison.OrdinalIgnoreCase) && value is JArray)
            {
                var fieldElements = ((JArray)value).ToObject<List<Field>>();
                foreach (var fieldElement in fieldElements)
                {
                    if (fieldElement.Value is JArray fieldElementAsArray)
                    {
                        if (string.Equals(fieldElement.Name, "assignee", StringComparison.OrdinalIgnoreCase))
                        {
                            // For assignees, we can do a strong-typed list.
                            _fields[fieldElement.Name] = new Field
                            {
                                Name = binder.Name,
                                Value = fieldElementAsArray.ToObject<List<Assignee>>()
                            };
                        }
                        else
                        {
                            Type collectionElementType;
                            if (fieldElementAsArray.First is JValue && JTokenTypeUtil.TryMapSimpleTokenType(fieldElementAsArray.First.Type, out collectionElementType))
                            {
                                // Map simple arrays to their array representation, e.g. string[] or int[]
                                _fields[fieldElement.Name] = new Field
                                {
                                    Name = binder.Name,
                                    Value = fieldElementAsArray.ToObject(JTokenTypeUtil.GenericListType.MakeGenericType(collectionElementType))
                                };
                            }
                            else
                            {
                                // Map more complex arrays to JToken[]
                                _fields[fieldElement.Name] = new Field
                                {
                                    Name = binder.Name,
                                    Value = fieldElementAsArray
                                };
                            }
                        }
                    }
                    else
                    {
                        // For other value types provide the value as-is (string, int, ...)
                        _fields[fieldElement.Name] = fieldElement;
                    }
                }
                return true;
            }
            
            Field field;
            if (_fields.TryGetValue(binder.Name, out field))
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