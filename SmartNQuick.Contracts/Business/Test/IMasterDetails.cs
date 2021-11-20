//@Ignore

namespace SmartNQuick.Contracts.Business.Test
{
    public interface IMasterDetails : IOneToMany<Persistence.Test.IMaster, Persistence.Test.IDetail>, ICopyable<IMasterDetails>
    {
    }
}
