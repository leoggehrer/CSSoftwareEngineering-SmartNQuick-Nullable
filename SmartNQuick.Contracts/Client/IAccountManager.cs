//@BaseCode
//MdStart
#if ACCOUNT_ON
using SmartNQuick.Contracts.Persistence.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartNQuick.Contracts.Client
{
    public partial interface IAccountManager
    {
        Task ChangePasswordAsync(string sessionToken, string oldPassword, string newPassword);
        Task ChangePasswordForAsync(string sessionToken, string email, string newPassword);
        Task<bool> HasRoleAsync(string sessionToken, string role);
        Task InitAppAccessAsync(string name, string email, string password, bool enableJwtAuth);
        Task<bool> IsSessionAliveAsync(string sessionToken);
        Task<ILoginSession> LogonAsync(string jsonWebToken);
        Task<ILoginSession> LogonAsync(string email, string password);
        Task<ILoginSession> LogonAsync(string email, string password, string optionalInfo);
        Task LogoutAsync(string sessionToken);
        Task<ILoginSession> QueryLoginAsync(string sessionToken);
        Task<IEnumerable<string>> QueryRolesAsync(string sessionToken);
        Task ResetFailedCountForAsync(string sessionToken, string email);
    }
}
#endif
//MdEnd