//@BaseCode
using System;
using System.Collections.Generic;

namespace CommonBase.Extensions
{
    public static class ObjectExtensions
	{
        public static void CheckArgument(this object source, string argName)
        {
            if (source == null)
                throw new ArgumentNullException(argName);
        }
        public static void CheckNotNull(this object source, string itemName)
        {
            if (source == null)
                throw new ArgumentNullException(itemName);
        }

        public static bool TryParse(this object value, Type type, out object typeValue)
        {
            type.CheckArgument(nameof(type));

            bool result;

            if (value == null)
            {
                result = true;
                typeValue = null;
            }
            else if (type.IsNullableType())
            {
                if (type.IsGenericType)
                {
                    result = value.TryParse(Nullable.GetUnderlyingType(type), out typeValue);
                }
                else
                {
                    typeValue = Convert.ChangeType(value, type);
                    result = true;
                }
            }
            else if (type == typeof(TimeSpan))
            {
                typeValue = TimeSpan.Parse(value.ToString());
                result = true;
            }
            else if (type == typeof(DateTime))
            {
                typeValue = DateTime.Parse(value.ToString());
                result = true;
            }
            else if (type.IsEnum)
            {
                result = Enum.TryParse(type, value.ToString(), out typeValue);
            }
            else
            {
                typeValue = Convert.ChangeType(value, type);
                result = true;
            }
            return result;
        }
        public static bool ConvertByTypeCode(this object value, Type type, out object typeValue)
        {
            type.CheckArgument(nameof(type));

            var result = false;
            var code = Type.GetTypeCode(type);

            typeValue = null;
            switch (code)
            {
                case TypeCode.Empty:
                    {
                        result = true;
                        break;
                    }
                case TypeCode.Object:
                    {
                        result = true;
                        typeValue = value;
                        break;
                    }
                case TypeCode.DBNull:
                    {
                        result = value is null or DBNull;
                        typeValue = result ? DBNull.Value : null;
                        break;
                    }
                case TypeCode.Boolean:
                    {
                        var resultValue = default(bool);
                        result = value != null && Boolean.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.Char:
                    {
                        var resultValue = default(char);
                        result = value != null && Char.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.SByte:
                    {
                        var resultValue = default(sbyte);
                        result = value != null && SByte.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.Byte:
                    {
                        var resultValue = default(byte);
                        result = value != null && Byte.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.Int16:
                    {
                        var resultValue = default(Int16);
                        result = value != null && Int16.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        var resultValue = default(UInt16);
                        result = value != null && UInt16.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.Int32:
                    {
                        var resultValue = default(Int32);
                        result = value != null && Int32.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        var resultValue = default(UInt32);
                        result = value != null && UInt32.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.Int64:
                    {
                        var resultValue = default(Int64);
                        result = value != null && Int64.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        var resultValue = default(UInt64);
                        result = value != null && UInt64.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.Single:
                    {
                        var resultValue = default(Single);
                        result = value != null && Single.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.Double:
                    {
                        var resultValue = default(Double);
                        result = value != null && Double.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.Decimal:
                    {
                        var resultValue = default(Decimal);
                        result = value != null && Decimal.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.DateTime:
                    {
                        var resultValue = default(DateTime);
                        result = value != null && DateTime.TryParse(value.ToString(), out resultValue);
                        typeValue = result ? resultValue : null;
                        break;
                    }
                case TypeCode.String:
                    {
                        result = value != null;
                        typeValue = result ? value.ToString() : null;
                        break;
                    }
            }
            return result;
        }
        public static T CopyTo<T>(this object source) where T : class, new()
        {
            var target = new T();

            CopyProperties(target, source);
            return target;
        }
        public static T CopyTo<T>(this object source, Func<string, bool> filter) where T : class, new()
        {
            var target = new T();

            CopyProperties(target, source, filter, null);
            return target;
        }
        public static T CopyTo<T>(this object source, Func<string, string> mapping) where T : class, new()
        {
            var target = new T();

            CopyProperties(target, source, null, mapping);
            return target;
        }
        public static T CopyTo<T>(this object source, Func<string, bool> filter, Func<string, string> mapping) where T : class, new()
        {
            var target = new T();

            CopyProperties(target, source, filter, mapping);
            return target;
        }

        public static void CopyTo(this object source, object target)
        {
            CopyProperties(target, source);
        }
        public static void CopyTo(this object source, object target, Func<string, bool> filter)
        {
            CopyProperties(target, source, filter, null);
        }
        public static void CopyTo(this object source, object target, Func<string, string> mapping)
        {
            CopyProperties(target, source, null, mapping);
        }
        public static void CopyTo(this object source, object target, Func<string, bool> filter, Func<string, string> mapping)
        {
            CopyProperties(target, source, filter, mapping);
        }

        public static void CopyFrom(this object target, object source)
        {
            if (source != null)
            {
                CopyProperties(target, source);
            }
        }
        public static void CopyFrom(this object target, object source, Func<string, bool> filter)
        {
            if (source != null)
            {
                CopyProperties(target, source, filter, null);
            }
        }
        public static void CopyFrom(this object target, object source, Func<string, string> mapping)
        {
            if (source != null)
            {
                CopyProperties(target, source, null, mapping);
            }
        }
        public static void CopyFrom(this object target, object source, Func<string, bool> filter, Func<string, string> mapping)
        {
            if (source != null)
            {
                CopyProperties(target, source, filter, mapping);
            }
        }

        public static void CopyProperties(object target, object source)
        {
            CopyProperties(target, source, null, null);
        }
        public static void CopyProperties(object target, object source, Func<string, bool> filter, Func<string, string> mapping)
        {
            target.CheckArgument(nameof(target));
            source.CheckArgument(nameof(source));

            Dictionary<string, PropertyItem> targetPropertyInfos = target.GetType().GetAllTypeProperties();
            Dictionary<string, PropertyItem> sourcePropertyInfos = source.GetType().GetAllTypeProperties();

            SetPropertyValues(target, source, filter, mapping, targetPropertyInfos, sourcePropertyInfos);
        }

        private static void SetPropertyValues(object target, object source, Func<string, bool> filter, Func<string, string> mapping, Dictionary<string, PropertyItem> targetPropertyInfos, Dictionary<string, PropertyItem> sourcePropertyInfos)
        {
            filter ??= (n => true);
            mapping ??= (n => n);
            foreach (KeyValuePair<string, PropertyItem> propertyItemTarget in targetPropertyInfos)
            {
                if (sourcePropertyInfos.TryGetValue(mapping(propertyItemTarget.Value.PropertyInfo.Name), out var propertyItemSource))
                {
                    if (propertyItemSource.PropertyInfo.PropertyType == propertyItemTarget.Value.PropertyInfo.PropertyType
                        && propertyItemSource.CanRead
                        && propertyItemTarget.Value.CanWrite
                        && (filter(propertyItemTarget.Value.PropertyInfo.Name)))
                    {
                        if (propertyItemSource.IsStringType)
                        {
                            object value = propertyItemSource.PropertyInfo.GetValue(source, null);

                            propertyItemTarget.Value.PropertyInfo.SetValue(target, value, null);
                        }
                        else if (propertyItemSource.IsArrayType)
                        {
                            object value = propertyItemSource.PropertyInfo.GetValue(source, null);

                            propertyItemTarget.Value.PropertyInfo.SetValue(target, value, null);
                        }
                        else if (propertyItemSource.PropertyInfo.PropertyType.IsValueType
                            && propertyItemTarget.Value.PropertyInfo.PropertyType.IsValueType)
                        {
                            object value = propertyItemSource.PropertyInfo.GetValue(source, null);

                            propertyItemTarget.Value.PropertyInfo.SetValue(target, value, null);
                        }
                        else if (propertyItemSource.IsComplexType)
                        {
                            object srcValue = propertyItemSource.PropertyInfo.GetValue(source);
                            object tarValue = propertyItemTarget.Value.PropertyInfo.GetValue(target);

                            if (srcValue != null && tarValue != null)
                            {
                                SetPropertyValues(tarValue, srcValue, filter, mapping, propertyItemTarget.Value.PropertyItems, propertyItemSource.PropertyItems);
                            }
                        }
                    }
                }
            }
        }
    }
}
