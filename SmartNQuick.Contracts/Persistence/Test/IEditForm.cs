//@Ignore
using System;

namespace SmartNQuick.Contracts.Persistence.Test
{
    public partial interface IEditForm : IVersionable, ICopyable<IEditForm>
    {
        [ContractPropertyInfo(MaxLength = 100, Description = "Textbox mit max. 100 Zeichen")]
        string TextBox { get; set; }
        [ContractPropertyInfo(MaxLength = 256, Description = "Textarea mit max. 256 Zeichen")]
        string TextArea { get; set; }
        [ContractPropertyInfo(MaxLength = 50, Required = true, DefaultValue = "\"Dieses Feld ist ein MUSS_FELD!\"", Description = "Textbox mit max. 50 Zeichen (Required)")]
        string TextBoxRequired { get; set; }
        [ContractPropertyInfo(MaxLength = 256, DefaultValue = "\"Dieser Text kann nicht verändert werden\"", Description = "Textarea mit max. 256 Zeichen (Readonly)")]
        string TextAreaReadonly { get; set; }

        [ContractPropertyInfo(DefaultValue = "Contracts.Modules.Common.State.Active")]
        Modules.Common.State EnumState { get; set; }

        [ContractPropertyInfo(DefaultValue = "true", Description = "CheckBox")]
        bool CheckBox { get; set; }
        [ContractPropertyInfo(Description = "CheckBox (Nullable)")]
        bool? CheckBoxNullable { get; set; }

        [ContractPropertyInfo(DefaultValue = "12", Description = "Byte")]
        byte ByteValue { get; set; }
        [ContractPropertyInfo(Description = "Byte (Nullable)")]
        byte? ByteNullable { get; set; }

        [ContractPropertyInfo(DefaultValue = "1024", Description = "Short")]
        short ShortValue { get; set; }
        [ContractPropertyInfo(Description = "Short (Nullable)")]
        short? ShortNullable { get; set; }

        [ContractPropertyInfo(DefaultValue = "1024", Description = "Integer")]
        int IntegerValue { get; set; }
        [ContractPropertyInfo(Description = "Integer (Nullable)")]
        int? IntegerNullable { get; set; }

        [ContractPropertyInfo(DefaultValue = "99.9", Description = "Double")]
        double DoubleValue { get; set; }
        [ContractPropertyInfo(Description = "Double (Nullable)")]
        double? DoubleNullable { get; set; }

        [ContractPropertyInfo(DefaultValue = "TimeSpan.Parse(\"12:35:15\")", Description = "TimeSpan")]
        TimeSpan TimeSpanValue { get; set; }
        [ContractPropertyInfo(Description = "TimeSpan (Nullable)")]
        TimeSpan? TimeSpanNullable { get; set; }

        [ContractPropertyInfo(DefaultValue = "DateTime.Now", Description = "Date")]
        DateTime DateValue { get; set; }
        [ContractPropertyInfo(Description = "Date (Nullable)")]
        DateTime? DateNullable { get; set; }

        [ContractPropertyInfo(ContentType = ContentType.DateTime, Description = "DateTime")]
        DateTime DateTimeValue { get; set; }
        [ContractPropertyInfo(Description = "DateTime (Nullable)", ContentType = ContentType.DateTime)]
        DateTime? DateTimeNullable { get; set; }
    }
}
