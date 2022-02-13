//@Ignore
using SmartNQuick.Contracts.Modules.Common;

namespace SmartNQuick.Contracts.Persistence.Test
{
    [ContractInfo]
    public partial interface IMaster : IVersionable, ICopyable<IMaster>
    {
        [ContractPropertyInfo(Required = true, MaxLength = 100, Description = "Textbox mit max. 100 Zeichen")]
        string Title { get; set; }
        [ContractPropertyInfo(MaxLength = 256, Description = "Textarea mit max. 256 Zeichen")]
        string Description { get; set; }
        [ContractPropertyInfo(DefaultValue = "Contracts.Modules.Common.State.Active")]
        State State { get; set; }
    }
}
