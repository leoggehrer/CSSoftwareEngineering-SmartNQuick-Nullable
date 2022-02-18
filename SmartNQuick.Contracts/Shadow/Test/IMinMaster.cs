//@Ignore

using SmartNQuick.Contracts.Modules.Common;
using SmartNQuick.Contracts.Persistence.Test;

namespace SmartNQuick.Contracts.Shadow.Test
{
    public interface IMinMaster : IIdentifiable, IShadow<IMaster>, ICopyable<IMinMaster>
    {
        [ContractPropertyInfo(IsAutoProperty = false)]
        string Title { get; set; }
        string Note { get; set; }
        State State { get; set; }
    }
}
