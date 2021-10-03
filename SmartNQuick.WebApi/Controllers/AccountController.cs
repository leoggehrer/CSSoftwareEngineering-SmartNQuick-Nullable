//@BaseCode
//MdStart
#if ACCOUNT_ON
using Microsoft.AspNetCore.Mvc;
using SmartNQuick.Contracts.Client;
using SmartNQuick.Contracts.Persistence.Account;
using SmartNQuick.Transfer.Models.Modules.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartNQuick.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AccountController : ControllerBase
    {
        private IAccountManager accountManager = null;
        private IAccountManager AccountManager => accountManager ??= Logic.Factory.CreateAccountManager();
        private ILoginSession ConvertTo(ILoginSession loginSession)
        {
            var result = new Transfer.Models.Persistence.Account.LoginSession();

            result.CopyProperties(loginSession);
            return result;
        }
        [HttpPost("/api/[controller]/Logon")]
        public async Task<ILoginSession> LogonAsync([FromBody] Logon logon)
        {
            return ConvertTo(await AccountManager.LogonAsync(logon.Email, logon.Password, logon.OptionalInfo).ConfigureAwait(false));
        }

        [HttpPost("/api/[controller]/JsonWebLogon")]
        public async Task<ILoginSession> JsonWebLogonAsync([FromBody] JsonWebLogon logon)
        {
            return ConvertTo(await AccountManager.LogonAsync(logon.Token).ConfigureAwait(false));
        }
        [HttpGet("/api/[controller]/Logout/{sessionToken}")]
        public Task LogoutAsync(string sessionToken)
        {
            return AccountManager.LogoutAsync(sessionToken);
        }
        [HttpGet("/api/[controller]/ChangePassword/{sessionToken}/{oldPwd}/{newPwd}")]
        public Task ChangePasswordAsync(string sessionToken, string oldPwd, string newPwd)
        {
            return AccountManager.ChangePasswordAsync(sessionToken, oldPwd, newPwd);
        }
        [HttpGet("/api/[controller]/ChangePasswordFor/{sessionToken}/{email}/{newPwd}")]
        public Task ChangePasswordForAsync(string sessionToken, string email, string newPwd)
        {
            return AccountManager.ChangePasswordForAsync(sessionToken, email, newPwd);
        }
        [HttpGet("/api/[controller]/ResetFailedCountFor/{sessionToken}/{email}")]
        public Task ResetFailedCountForAsync(string sessionToken, string email)
        {
            return AccountManager.ResetFailedCountForAsync(sessionToken, email);
        }
        [HttpGet("/api/[controller]/HasRole/{sessionToken}/{role}")]
        public Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            return AccountManager.HasRoleAsync(sessionToken, role);
        }
        [HttpGet("/api/[controller]/IsSessionAlive/{sessionToken}")]
        public Task<bool> IsSessionAliveAsync(string sessionToken)
        {
            return AccountManager.IsSessionAliveAsync(sessionToken);
        }
        [HttpGet("/api/[controller]/QueryRoles/{sessionToken}")]
        public Task<IEnumerable<string>> QueryRolesAsync(string sessionToken)
        {
            return AccountManager.QueryRolesAsync(sessionToken);
        }
        [HttpGet("/api/[controller]/QueryLogin/{sessionToken}")]
        public async Task<ILoginSession> QueryLoginAsync(string sessionToken)
        {
            return ConvertTo(await AccountManager.QueryLoginAsync(sessionToken).ConfigureAwait(false));
        }
    }
}
#endif
//MdEnd