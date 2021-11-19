//@BaseCode
//MdStart
#if ACCOUNT_ON

namespace SmartNQuick.Contracts.Persistence.Account
{
    [ContractInfo]
    public partial interface IRole : IVersionable, ICopyable<IRole>
    {
        [ContractPropertyInfo(IsUnique = true, Required = true, MaxLength = 64)]
        string Designation { get; set; }
        [ContractPropertyInfo(MaxLength = 256)]
        string Description { get; set; }
    }
}
#endif
//MdEnd