//@BaseCode
using System;

namespace CommonBase.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
    public partial class ContractPropertyInfoAttribute : Attribute
    {
        private bool notMapped = false;
        public bool NotMapped
        {
            get => notMapped && HasImplementation == false;
            set => notMapped = value;
        }
        public bool HasImplementation { get; init; } = false;
        public bool IsAutoProperty { get; init; } = true;

        public int Order { get; init; } = 10_000;
        public string ColumnName { get; init; }
        public string NavigationName { get; init; }

        public bool Required { get; init; } = false;
        public bool HasIndex { get; init; } = false;
        public bool IsUnique { get; init; } = false;

        public bool IsFixedLength { get; init; } = false;
        public int Precision { get; init; } = 2;
        public int MinLength { get; init; } = -1;
        public int MaxLength { get; init; } = -1;
        public string RegularExpression { get; init; }
        public ContentType ContentType { get; init; } = ContentType.Undefined;

        public bool HasUniqueIndexWithName { get; init; } = false;
        public string IndexName { get; init; } = string.Empty;
        public int IndexColumnOrder { get; init; }

        public string DefaultValue { get; init; }
        public string DefaultValueSql { get; init; }
        public string Description { get; init; }
    }
}
