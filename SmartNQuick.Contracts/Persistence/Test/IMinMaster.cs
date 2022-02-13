//@Ignore

namespace SmartNQuick.Contracts.Persistence.Test
{
    [ContractInfo(DelegateType = typeof(IMaster))]
    public interface IMinMaster : IIdentifiable, ICopyable<IMinMaster>
    {
        string Title { get; set; }
    }
}
