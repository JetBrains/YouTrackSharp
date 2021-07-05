using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YouTrackSharp.Generated;
using YouTrackSharp.Json;

namespace YouTrackSharp.Issues
{
    // TODO: Add dynamic object implementation cache so no iteration is needed over Fields

    /// <summary>
    /// A class that represents YouTrack issue information. Can be casted to a <see cref="DynamicObject"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerDisplay("{Id}: {Summary}")]
    public class Issue
        : DynamicObject
    {
        /// <summary>
        /// Creates an instance of the <see cref="Issue" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="Generated.Issue"/> to convert from.</param>
        /// <param name="wikify">If set to <value>true</value>, then issue description will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        internal static Issue FromApiEntity(Generated.Issue entity, bool wikify = false)
        {
            var issue = new Issue
            {
                Id = entity.IdReadable,
                EntityId = entity.Id,
                Summary = entity.Summary,
                Description = wikify ? entity.WikifiedDescription : entity.Description,
                IsMarkdown = entity.UsesMarkdown ?? true,
                Comments = entity.Comments?.Select(comment => Comment.FromApiEntity(comment, false)).ToList(),
                Tags = entity.Tags?.Select(tag => new SubValue<string>(){Value=tag.Name})
            };

            if (entity.Watchers.HasStar ?? false)
            {
                var starTag = new SubValue<string>() {Value = "Star"};
                issue.Tags = issue.Tags == null ? new List<SubValue<string>>(){starTag} : issue.Tags.Append(starTag);
            }
            
            issue.SetField("projectShortName", entity.Project.ShortName);
            issue.SetField("numberInProject", entity.NumberInProject);
            issue.SetField("wikified", wikify);
            issue.SetField("created", entity.Created);
            issue.SetField("reporterName", entity.Reporter.Login);
            issue.SetField("reporterFullName", entity.Reporter.FullName);
            issue.SetField("updated", entity.Updated);
            issue.SetField("updaterName", entity.Updater.Login);
            issue.SetField("updaterFullName", entity.Updater.FullName);
            if (entity.Resolved != null)
            {
                issue.SetField("resolved", entity.Resolved);
            }
            issue.SetField("commentsCount", entity.CommentsCount);
            issue.SetField("votes", entity.Votes);
            
            foreach (var customField in entity.CustomFields)
            {
                switch (customField)
                {
                    //TODO somehow if we skip block for DateIssueCustomField, it won't be processed
                    //      even though it's a descendant of SimpleIssueCustomField
                    case DateIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.ToString()};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = rawValue, ValueId = JArray.FromObject(rawValue)
                            };
                        }
                        break;
                    case SimpleIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.ToString()};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = rawValue, ValueId = JArray.FromObject(rawValue)
                            };
                        }
                        break;
                    case PeriodIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.Id.Substring(1, f.Value.Id.Length - 1).ToLower()};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name,
                                Value = new List<string>() {(f.Value.Minutes ?? 0).ToString()},
                                ValueId = JArray.FromObject(rawValue)
                            };
                        }
                        break;
                    case TextIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.Text};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = rawValue, ValueId = JArray.FromObject(rawValue)
                            };
                        }
                        break;
                    case SingleUserIssueCustomField f:
                        if (f.Value != null)
                        {
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name,
                                Value = new List<Assignee>()
                                {
                                    new Assignee() {UserName = f.Value.Login, FullName = f.Value.FullName}
                                }
                            };
                        }
                        break;
                    case SingleGroupIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.Name};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = rawValue, ValueId = JArray.FromObject(rawValue)
                            };
                        }
                        break;
                    //TODO could identical case bodies be optimized with switch over type? _3_ blocks below
                    case SingleBuildIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.Name};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name,
                                Value = rawValue,
                                ValueId = JArray.FromObject(rawValue),
                                Color = YouTrackColor.FromApiEntity(f.Value.Color)
                            };
                        }
                        break;
                    case SingleOwnedIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.Name};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name,
                                Value = rawValue,
                                ValueId = JArray.FromObject(rawValue),
                                Color = YouTrackColor.FromApiEntity(f.Value.Color)
                            };
                        }
                        break;
                    case SingleVersionIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.Name};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name,
                                Value = rawValue,
                                ValueId = JArray.FromObject(rawValue),
                                Color = YouTrackColor.FromApiEntity(f.Value.Color)
                            };
                        }
                        break;
                    //^^ end TODO
                    //TODO could identical case bodies be optimized with switch over type? _2_ blocks below
                    case SingleEnumIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.Name};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name,
                                Value = new List<string>() {f.Value.LocalizedName ?? f.Value.Name},
                                ValueId = JArray.FromObject(rawValue),
                                Color = YouTrackColor.FromApiEntity(f.Value.Color)
                            };
                        }
                        break;
                    case StateIssueCustomField f:
                        if (f.Value != null)
                        {
                            var rawValue = new List<string>() {f.Value.Name};
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name,
                                Value = new List<string>() {f.Value.LocalizedName ?? f.Value.Name},
                                ValueId = JArray.FromObject(rawValue),
                                Color = YouTrackColor.FromApiEntity(f.Value.Color)
                            };
                        }
                        break;
                    //^^ end TODO
                    case MultiUserIssueCustomField f:
                        if (f.Value != null && f.Value.Any())
                        {
                            var values = f.Value.Select(v => new Assignee()
                            {
                                UserName = v.Login, FullName = v.FullName
                            }).ToList();
                            
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = values
                            };
                        }
                        break;
                    //TODO could identical case bodies be optimized with switch over type? _4_ blocks below
                    case MultiGroupIssueCustomField f:
                        if (f.Value != null && f.Value.Any())
                        {
                            var values = f.Value.Select(v => v.Name).ToList();
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = values, ValueId = JArray.FromObject(values)
                            };
                        }
                        break;
                    case MultiBuildIssueCustomField f:
                        if (f.Value != null && f.Value.Any())
                        {
                            var values = f.Value.Select(v => v.Name).ToList();
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = values, ValueId = JArray.FromObject(values)
                            };
                        }
                        break;
                    case MultiOwnedIssueCustomField f:
                        if (f.Value != null && f.Value.Any())
                        {
                            var values = f.Value.Select(v => v.Name).ToList();
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = values, ValueId = JArray.FromObject(values)
                            };
                        }
                        break;
                    case MultiVersionIssueCustomField f:
                        if (f.Value != null && f.Value.Any())
                        {
                            var values = f.Value.Select(v => v.Name).ToList();
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = values, ValueId = JArray.FromObject(values)
                            };
                        }
                        break;
                    //^^ end TODO
                    case MultiEnumIssueCustomField f:
                        if (f.Value != null && f.Value.Any())
                        {
                            var localizedValues = f.Value.Select(v => v.LocalizedName).ToList();
                            var values = f.Value.Select(v => v.Name).ToList();
                            issue._fields[f.Name] = new Field()
                            {
                                Name = f.Name, Value = localizedValues, ValueId = JArray.FromObject(values)
                            };
                        }
                        break;
                    default:
                        //TODO
                        break;
                }
            }

            return issue;
        }
        
        /// <summary>
        /// Use case-sensitive field names? Defaults to false.
        /// </summary>
        /// <remarks>
        /// When set to true, fields like "assignee" and "Assignee" can be used.
        /// </remarks>
        public static bool UseCaseSensitiveFieldNames { get; set; }
        
        private readonly IDictionary<string, Field> _fields = new Dictionary<string, Field>(
            UseCaseSensitiveFieldNames
                ? StringComparer.Ordinal
                : StringComparer.OrdinalIgnoreCase);
        
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
        /// Get all issue fields.
        /// </summary>
        public ICollection<Field> Fields => _fields.Values;

        /// <summary>
        /// Get all issue field names.
        /// </summary>
        public ICollection<string> FieldNames => _fields.Keys;

        /// <summary>
        /// Issue comments.
        /// </summary>
        [JsonProperty("comment")]
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Issue tags.
        /// </summary>
        [JsonProperty("tag")]
        public IEnumerable<SubValue<string>> Tags { get; set; }

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
                _fields[fieldName] = new Field { Name = fieldName, Value = value };
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
            if (string.Equals(binder.Name, "field", StringComparison.OrdinalIgnoreCase) && value is JArray array)
            {   
                var fieldElements = array.ToObject<List<Field>>();
                foreach (var fieldElement in fieldElements)
                {
                    if (fieldElement.Value is JArray fieldElementAsArray)
                    {
                        // Map collection
                        // Heuristics for finding fields of the type List<Assignee>
                        var children = new List<JToken>(fieldElementAsArray.First.Children());
                        if (children.Count == 2 && children[0] is JProperty && ((JProperty)children[0]).Name == "value"
                            && children[1] is JProperty && ((JProperty)children[1]).Name == "fullName")
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