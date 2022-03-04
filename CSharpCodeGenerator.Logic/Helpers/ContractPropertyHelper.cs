//@BaseCode
//MdStart
using CommonBase.Attributes;
using CSharpCodeGenerator.Logic.Generation;
using System;
using System.Linq;
using System.Reflection;

namespace CSharpCodeGenerator.Logic.Helpers
{
    internal partial class ContractPropertyHelper
    {
        public Type Type { get; }
        public PropertyInfo Property { get; }
        private PropertyInfoAttribute AttributeInfo { get; }

        public string PropertyName => Property.Name;

        public Type PropertyType => Property.PropertyType;
        public string PropertyFieldType => GeneratorObject.GetPropertyType(Property);
        public string PropertyFieldName => $"_{char.ToLower(PropertyName[0])}{PropertyName[1..]}";
        public Type DeclaringType => Property.DeclaringType;
        public bool CanRead => Property.CanRead;
        public bool CanWrite => Property.CanWrite;
        public ContentType ContentType => AttributeInfo != null ? AttributeInfo.ContentType : ContentType.Undefined;

        public bool NotMapped => AttributeInfo != null ? AttributeInfo.NotMapped : false;
        public bool HasImplementation => AttributeInfo != null ? AttributeInfo.HasImplementation : false;
        public bool IsAutoProperty => AttributeInfo != null ? AttributeInfo.IsAutoProperty : true;
        public bool JsonIgnore => AttributeInfo != null ? AttributeInfo.JsonIgnore : false;

        public int Order => AttributeInfo != null ? AttributeInfo.Order : 10_000;
        public string ColumnName => AttributeInfo?.ColumnName;
        public string NavigationName => AttributeInfo?.NavigationName;

        public bool IsRequired => AttributeInfo != null && AttributeInfo.Required;
        public bool HasIndex => AttributeInfo != null && AttributeInfo.HasIndex;
        public bool IsUnique => AttributeInfo != null && AttributeInfo.IsUnique;

        public bool IsFixedLength => AttributeInfo != null && AttributeInfo.IsFixedLength;
        public int Precision => AttributeInfo != null ? AttributeInfo.Precision : 2;
        public int MinLength => AttributeInfo != null ? AttributeInfo.MinLength : -1;
        public int MaxLength => AttributeInfo != null ? AttributeInfo.MaxLength : -1;
        public string RegularExpression => AttributeInfo?.RegularExpression ?? string.Empty;

        public bool HasUniqueIndexWithName => AttributeInfo != null && AttributeInfo.HasUniqueIndexWithName;
        public string IndexName => AttributeInfo != null ? AttributeInfo.IndexName : string.Empty;
        public int IndexColumnOrder => AttributeInfo != null ? AttributeInfo.IndexColumnOrder : 0;

        public string DefaultValue => AttributeInfo?.DefaultValue ?? string.Empty;
        public string DefaultValueSql => AttributeInfo?.DefaultValueSql ?? string.Empty;
        public string Description => AttributeInfo?.Description ?? string.Empty;

        public ContractPropertyHelper(Type type, PropertyInfo propertyInfo)
        {
            type.CheckArgument(nameof(type));
            propertyInfo.CheckArgument(nameof(propertyInfo));

            Type = type;
            Property = propertyInfo;
            AttributeInfo = Type.GetCustomAttributes<ContractMemberInfoAttribute>()
                                .FirstOrDefault(cmi => cmi.PropertyName.Equals(propertyInfo.Name));
            AttributeInfo ??= Property.GetCustomAttribute<ContractPropertyInfoAttribute>();
        }
    }
}
//MdEnd