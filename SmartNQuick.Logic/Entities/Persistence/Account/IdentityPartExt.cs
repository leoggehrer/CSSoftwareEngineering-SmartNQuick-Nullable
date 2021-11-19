//@BaseCode
//MdStart
#if ACCOUNT_ON

namespace SmartNQuick.Logic.Entities.Persistence.Account
{
    using CommonBase.Extensions;
    internal partial class Identity
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public void CopyProperties(Identity identity)
        {
            identity.CheckArgument(nameof(identity));

            CopyProperties(identity as Contracts.Persistence.Account.IIdentity);

            PasswordHash = identity.PasswordHash;
            PasswordSalt = identity.PasswordSalt;
        }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
#endif
//MdEnd
