//@Ignore

namespace SmartNQuick.Contracts.Persistence.Test
{
    public partial interface IDetail : IVersionable, Modules.Base.IDetail, ICopyable<IDetail>
    {
    }
}
