using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YouTrackSharp
{
    public class JsonIssueConverter : IJsonIssueConverter
    {
        /// <summary>
        /// Convert a dynamic issue to a static typed Issue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Issue ConvertFromDynamic(dynamic source)
        {
            var issue = new Issue
                        {
                            Id = source.id,
                            FixedInBuild = source.fixedInBuild,
                            Priority = source.priority,
                            Type = source.type,
                            ReporterName = source.reporterName,
                            ProjectShortName = source.projectShortName,
                            State = source.state,
                            Subsystem = source.subsystem,
                            Summary = source.summary
                        };

            return issue;

        }

        /// <summary>
        /// Converts a dynamic issue where fields are represented as field property to typed Issue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Issue ConvertFromDynamicFields(dynamic source)
        {
            var issue = new Issue();

            var properties = typeof(Issue).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.SetValue(issue, GetField(property.Name, source.field), null);
            }

            issue.Id = source.id;

            return issue;
        }

        static string GetField(string fieldName, IList<dynamic> fields)
        {
            for (var i = 0; i < fields.Count() - 1 ; i++)
            {
                var field = fields[i];

                if (String.Compare(field.name, fieldName, true) == 0)
                {
                    return field.value;
                }
            }
            return String.Empty;
        }

        //public Issue GetYouTrackIssue(dynamic fields)
        //{
        //    // TODO: Refactor this to not have to iterate all properties
        //    var issue = new Issue()
        //                {
        //                    Id = data.id,
        //                    ProjectShortName = fields[0].value,
        //                    Assignee = fields[]

        //                };

        //    return issue;

        //}
    }
}
