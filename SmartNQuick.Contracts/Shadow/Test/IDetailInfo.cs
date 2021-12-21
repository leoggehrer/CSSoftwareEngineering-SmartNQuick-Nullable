//@Ignore

namespace SmartNQuick.Contracts.Shadow.Test
{
    public interface IDetailInfo : Modules.Base.IDetail, IShadow<Persistence.Test.IDetail>, ICopyable<IDetailInfo>
    {
    }
}
