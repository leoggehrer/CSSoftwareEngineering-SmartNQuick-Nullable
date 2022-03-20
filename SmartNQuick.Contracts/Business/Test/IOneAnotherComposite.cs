//@Ignore

namespace SmartNQuick.Contracts.Business.Test
{
    public interface IOneAnotherComposite : IComposite<Persistence.Test.IOneXAnother, Persistence.Test.IOne, Persistence.Test.IAnother>, ICopyable<IOneAnotherComposite>
    {
    }
}
