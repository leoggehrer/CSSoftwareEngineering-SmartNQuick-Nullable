//@Ignore

using SmartNQuick.Contracts.Modules.Common;
using SmartNQuick.Contracts.Persistence.Test;

namespace SmartNQuick.Contracts.Shadow.Test
{
    public interface IMinMaster : IIdentifiable, IShadow<IMaster>, ICopyable<IMinMaster>
    {
        string Title { get; set; }
        State State { get; set; }
    }
}
