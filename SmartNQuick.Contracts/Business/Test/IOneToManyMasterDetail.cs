//@Ignore

namespace SmartNQuick.Contracts.Business.Test
{
    public interface IOneToManyMasterDetail : IOneToMany<Persistence.Test.IMaster, Persistence.Test.IDetail>, ICopyable<IOneToManyMasterDetail>
    {
    }
}
