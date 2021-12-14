namespace SmartNQuick.Contracts.Shadow.Test
{
    public interface IDetailInfo : IVersionable, Modules.Base.IDetail, IShadow<Persistence.Test.IDetail>, ICopyable<IDetailInfo>
    {
    }
}
