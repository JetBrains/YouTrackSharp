#region License
// Distributed under the BSD License
// =================================
// 
// Copyright (c) 2010-2011, Hadi Hariri
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Reflection;

namespace YouTrackSharp.Issues
{
    public class IssueTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof (List<Field>))
            {
                var hashValues = CreateHashOfFields((List<Field>) value);
                
                return ConvertFromFields(hashValues);
            }
            throw new InvalidCastException("Cannot convert from type: " + value.GetType());
        }

        Hashtable CreateHashOfFields(List<Field> value)
        {
            
            var hashTable = new Hashtable(value.Count, StringComparer.InvariantCultureIgnoreCase);
            foreach (var field in value)
            {
                hashTable.Add(field.name, field.value);
            }
            return hashTable;
        }


        Issue ConvertFromFields(Hashtable fields)
        {
            var issue = new Issue();

            PropertyInfo[] properties = typeof (Issue).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (String.Compare(property.Name, "Links") == 0 && fields["Links"] != null)
                {
                    issue.Links = ConvertLinks(fields["Links"]);

                } else
                {
                    // Special case. Assignee is Assignee on single and AssigneeName on multiple
                    if (String.Compare(property.Name, "AssigneeName") == 0)
                    {
                        if (fields["Assignee"] != null)
                        {
                            property.SetValue(issue, Convert.ChangeType(fields["Assignee"], property.PropertyType), null);
                        } else if (fields["AssigneeName"] != null)
                        {
                            property.SetValue(issue, Convert.ChangeType(fields["AssigneeName"], property.PropertyType), null);
                            
                        }
                    } else
                    {
                        property.SetValue(issue, Convert.ChangeType(fields[property.Name], property.PropertyType), null);
                    }
                }
            }
            return issue;
        }

        IList<Link> ConvertLinks(dynamic values)
        {
            var links = new List<Link>();

            if (values.GetType() == typeof(ExpandoObject[]))
            {
              for (int i = 0; i < values.Length; i++)
              {
                var link = ConvertLink(values[i]);
                links.Add(link);
              }
            }
            else
              links.Add(ConvertLink(values));

          return links;
        }

      private static Link ConvertLink(dynamic val)
      {
        var value = (IDictionary<string, object>) val;
        // THe reason for the casting is because there's a field called $ that you cannot access otherwise.
        var link = new Link()
                     {
                       Type = value["type"].ToString(),
                       Role = value["role"].ToString(),
                       Value = value["$"].ToString()
                     };
        return link;
      }
    }
}