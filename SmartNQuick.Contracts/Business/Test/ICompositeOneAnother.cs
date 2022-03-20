//@Ignore

namespace SmartNQuick.Contracts.Business.Test
{
    public interface ICompositeOneAnother : IComposite<Persistence.Test.IOneXAnother, Persistence.Test.IOne, Persistence.Test.IAnother>, ICopyable<ICompositeOneAnother>
    {
    }
}
