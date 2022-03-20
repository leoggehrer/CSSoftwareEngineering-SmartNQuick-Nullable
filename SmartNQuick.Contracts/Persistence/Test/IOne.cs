﻿//@Ignore

namespace SmartNQuick.Contracts.Persistence.Test
{
    [ContractInfo]
    public partial interface IOne : IVersionable, ICopyable<IOne>
    {
        [ContractPropertyInfo(Required = true, MaxLength = 100, Description = "Textbox mit max. 100 Zeichen")]
        string Name { get; set; }
        [ContractPropertyInfo(MaxLength = 256, Description = "Textarea mit max. 256 Zeichen")]
        string Description { get; set; }
    }
}
