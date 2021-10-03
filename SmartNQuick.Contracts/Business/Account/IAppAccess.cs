//@BaseCode
//MdStart
#if ACCOUNT_ON
using SmartNQuick.Contracts.Persistence.Account;

namespace SmartNQuick.Contracts.Business.Account
{
    public partial interface IAppAccess : IOneToMany<IIdentity, IRole>, ICopyable<IAppAccess>
    {

    }
}
#endif
//MdEnd