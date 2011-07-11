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
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace YouTrackSharp.Issues
{
    public class IssueTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof (Field[]))
            {
                return ConvertFromFields((Field[]) value);
            }
            throw new InvalidCastException("Cannot convert from type: " + value.GetType());
        }


        Issue ConvertFromFields(Field[] source)
        {
            var issue = new Issue();

            PropertyInfo[] properties = typeof (Issue).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(issue, GetValueByName(property.Name, source), null);
            }


            return issue;
        }

        static string GetValueByName(string fieldName, Field[] fields)
        {
            for (int i = 0; i < fields.Length - 1; i++)
            {
                Field field = fields[i];

                if (String.Compare(field.name, fieldName, true) == 0)
                {
                    return field.value.ToString();
                }
            }
            return String.Empty;
        }
    }
}