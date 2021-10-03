//@BaseCode
//MdStart
#if ACCOUNT_ON
using SmartNQuick.Contracts.Client;
using SmartNQuick.Contracts.Persistence.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Modules.Account
{
    internal partial class AccountManagerWrapper : IAccountManager
    {
        #region Public logon
        public Task InitAppAccessAsync(string name, string email, string password, bool enableJwtAuth)
        {
            return AccountManager.InitAppAccessAsync(name, email, password, enableJwtAuth);
        }
        public Task<ILoginSession> LogonAsync(string jsonWebToken)
        {
            return AccountManager.LogonAsync(jsonWebToken);
        }
        public Task<ILoginSession> LogonAsync(string email, string password)
        {
            return LogonAsync(email, password, string.Empty);
        }
        public Task<ILoginSession> LogonAsync(string email, string password, string optionalInfo)
        {
            return AccountManager.LogonAsync(email, password, optionalInfo);
        }
        public Task LogoutAsync(string sessionToken)
        {
            return AccountManager.LogoutAsync(sessionToken);
        }
        public Task<IEnumerable<string>> QueryRolesAsync(string sessionToken)
        {
            return AccountManager.QueryRolesAsync(sessionToken);
        }
        public Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            return AccountManager.HasRoleAsync(sessionToken, role);
        }
        public Task<ILoginSession> QueryLoginAsync(string sessionToken)
        {
            return AccountManager.QueryLoginAsync(sessionToken);
        }
        public Task<bool> IsSessionAliveAsync(string sessionToken)
        {
            return AccountManager.IsSessionAliveAsync(sessionToken);
        }
        public Task ChangePasswordAsync(string sessionToken, string oldPassword, string newPassword)
        {
            return AccountManager.ChangePasswordAsync(sessionToken, oldPassword, newPassword);
        }
        public Task ChangePasswordForAsync(string sessionToken, string email, string newPassword)
        {
            return AccountManager.ChangePasswordForAsync(sessionToken, email, newPassword);
        }
        public Task ResetFailedCountForAsync(string sessionToken, string email)
        {
            return AccountManager.ResetFailedCountForAsync(sessionToken, email);
        }
        #endregion Public logon
    }
}
#endif
//MdEnd