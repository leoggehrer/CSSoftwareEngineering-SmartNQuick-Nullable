//@BaseCode
//MdStart
#if ACCOUNT_ON
using SmartNQuick.Contracts.Persistence.Account;

namespace SmartNQuick.Contracts.Business.Account
{
    public partial interface IIdentityUser : IOneToAnother<IIdentity, IUser>, ICopyable<IIdentityUser>
    {
    }
}
#endif
//MdEnd