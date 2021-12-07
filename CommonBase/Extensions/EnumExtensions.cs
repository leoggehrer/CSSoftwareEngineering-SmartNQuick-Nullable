//@BaseCode
//MdStart
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CommonBase.Extensions
{
    public static partial class EnumExtensions
    {
        public static T NextEnum<T>(this T source) where T : struct, Enum
        {
            if (typeof(T).IsEnum == false)
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

            var values = (T[])Enum.GetValues(source.GetType());
            var idx = Array.IndexOf(values, source) + 1;

            return (values.Length <= idx) ? values[0] : values[idx];
        }

        /// <summary>
        /// Gets the string of an DescriptionAttribute of an Enum.
        /// </summary>
        /// <param name="value">The Enum value for which the description is needed.</param>
        /// <returns>If a DescriptionAttribute is set it return the content of it.
        /// Otherwise just the raw name as string.</returns>
        public static string Description(this Enum value)
        {
            value.CheckArgument(nameof(value));

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        /// <summary>
        /// Creates an List with all keys and values of a given Enum class
        /// </summary>
        /// <typeparam name="T">Must be derived from class Enum!</typeparam>
        /// <returns>A list of KeyValuePair&lt;Enum, string&gt; with all available
        /// names and values of the given Enum.</returns>
        public static IList<KeyValuePair<Enum, string>> ToList<T>() where T : struct
        {
            var type = typeof(T);

            if (type.IsEnum == false)
            {
                throw new ArgumentException("T must be an enum");
            }

            return (IList<KeyValuePair<Enum, string>>)
                    Enum.GetValues(type)
                        .OfType<Enum>()
                        .Select(e => new KeyValuePair<Enum, string>(e, e.Description()))
                        .ToArray();
        }

        /// <summary>
        /// Creates an List with all keys and values of a given Enum class
        /// </summary>
        /// <typeparam name="T">Must be derived from class Enum!</typeparam>
        /// <returns>A list of KeyValuePair&lt;string, string&gt; with all available
        /// names and values of the given Enum.</returns>
        public static IList<KeyValuePair<string, string>> ToSelect<T>() where T : struct
        {
            var type = typeof(T);

            if (type.IsEnum == false)
            {
                throw new ArgumentException("T must be an enum");
            }

            return (IList<KeyValuePair<string, string>>)
                    Enum.GetValues(type)
                        .OfType<Enum>()
                        .Select(e => new KeyValuePair<string, string>(e.ToString(), e.Description()))
                        .ToArray();
        }

        public static T GetValueFromDescription<T>(string description) where T : struct
        {
            var type = typeof(T);

            if (type.IsEnum == false)
            {
                throw new ArgumentException("T must be an enum");
            }

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description.Equals(description))
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name.Equals(description))
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            return default;
        }
    }
}
//MdEnd