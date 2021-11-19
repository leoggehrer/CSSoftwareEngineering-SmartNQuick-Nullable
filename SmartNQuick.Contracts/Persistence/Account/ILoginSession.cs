//@BaseCode
//MdStart
#if ACCOUNT_ON
using System;

namespace SmartNQuick.Contracts.Persistence.Account
{
    [ContractInfo]
    public partial interface ILoginSession : IVersionable, ICopyable<ILoginSession>
    {
        [ContractPropertyInfo(IsAutoProperty = false)]
        int IdentityId { get; }
        [ContractPropertyInfo(NotMapped = true)]
        bool IsRemoteAuth { get; }
        [ContractPropertyInfo(NotMapped = true, IsAutoProperty = false)]
        string Origin { get; }
        [ContractPropertyInfo(NotMapped = true, IsAutoProperty = false)]
        string Name { get; }
        [ContractPropertyInfo(NotMapped = true, IsAutoProperty = false)]
        string Email { get; }
        [ContractPropertyInfo(NotMapped = true, IsAutoProperty = false)]
        int TimeOutInMinutes { get; }
        [ContractPropertyInfo(NotMapped = true)]
        string JsonWebToken { get; }
        [ContractPropertyInfo(Required = true, MaxLength = 128)]
        string SessionToken { get; }
        [ContractPropertyInfo()]
        DateTime LoginTime { get; }
        [ContractPropertyInfo(IsAutoProperty = false)]
        DateTime LastAccess { get; }
        [ContractPropertyInfo(IsAutoProperty = false)]
        DateTime? LogoutTime { get; }
        [ContractPropertyInfo(MaxLength = 4096)]
        string OptionalInfo { get; set; }
    }
}
#endif
//MdEnd