//@Ignore

namespace SmartNQuick.Contracts.Persistence.Test
{
    [ContractInfo]
    public partial interface IOneXAnother : IVersionable, ICopyable<IOneXAnother>
    {
        public int OneId { get; set; }
        public int AnotherId { get; set; }
        [ContractPropertyInfo(MaxLength = 256, Description = "Textarea mit max. 256 Zeichen")]
        string Note { get; set; }
    }
}
