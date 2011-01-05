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
                    return field.value;
                }
            }
            return String.Empty;
        }
    }
}