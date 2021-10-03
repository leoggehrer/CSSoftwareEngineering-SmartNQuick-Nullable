//@BaseCode
//MdStart
#if ACCOUNT_ON
using SmartNQuick.Contracts.Persistence.Account;
using SmartNQuick.Transfer.Models.Modules.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartNQuick.Adapters.Modules.Account
{
    public partial class AccountManager
    {
        static AccountManager()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        /// <summary>
        /// The base url like https://localhost:5001/api
        /// </summary>
        public string BaseUri { get; set; } = Factory.BaseUri;
        public AdapterType Adapter { get; set; } = Factory.Adapter;

        public AccountManager()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public async Task<ILoginSession> LogonAsync(string jsonWebToken)
        {
            var result = default(ILoginSession);

            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                result = await accountManager.LogonAsync(jsonWebToken).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var logon = new JsonWebLogon { Token = jsonWebToken };
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.LogonAsync(logon).ConfigureAwait(false);
            }
            return result;
        }
        public async Task<ILoginSession> LogonAsync(string email, string password)
        {
            return await LogonAsync(email, password, string.Empty).ConfigureAwait(false);
        }
        public async Task<ILoginSession> LogonAsync(string email, string password, string optionalInfo)
        {
            var result = default(ILoginSession);

            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                result = await accountManager.LogonAsync(email, password, optionalInfo).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var logon = new Logon
                {
                    Email = email,
                    Password = password,
                    OptionalInfo = optionalInfo,
                };
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.LogonAsync(logon).ConfigureAwait(false);
            }
            return result;
        }
        public async Task LogoutAsync(string sessionToken)
        {
            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                await accountManager.LogoutAsync(sessionToken).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri)
                {
                    SessionToken = sessionToken
                };
                await serviceInvoker.LogoutAsync(sessionToken).ConfigureAwait(false);
            }
        }
        public async Task ChangePasswordAsync(string sessionToken, string oldPwd, string newPwd)
        {
            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                await accountManager.ChangePasswordAsync(sessionToken, oldPwd, newPwd).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                await serviceInvoker.ChangePasswordAsync(sessionToken, oldPwd, newPwd).ConfigureAwait(false);
            }
        }
        public async Task ChangePasswordForAsync(string sessionToken, string email, string password)
        {
            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                await accountManager.ChangePasswordForAsync(sessionToken, email, password).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                await serviceInvoker.ChangePasswordForAsync(sessionToken, email, password).ConfigureAwait(false);
            }
        }
        public async Task ResetFailedCountForAsync(string sessionToken, string email)
        {
            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                await accountManager.ResetFailedCountForAsync(sessionToken, email).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                await serviceInvoker.ResetFailedCountForAsync(sessionToken, email).ConfigureAwait(false);
            }
        }
        public async Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            var result = false;

            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                result = await accountManager.HasRoleAsync(sessionToken, role).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.HasRoleAsync(sessionToken, role).ConfigureAwait(false);
            }
            return result;
        }
        public async Task<bool> IsSessionAliveAsync(string sessionToken)
        {
            var result = false;

            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                result = await accountManager.IsSessionAliveAsync(sessionToken).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.IsSessionAliveAsync(sessionToken).ConfigureAwait(false);
            }
            return result;
        }
        public async Task<IEnumerable<string>> QueryRolesAsync(string sessionToken)
        {
            var result = default(IEnumerable<string>);

            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                result = await accountManager.QueryRolesAsync(sessionToken).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.QueryRolesAsync(sessionToken).ConfigureAwait(false);
            }
            return result;
        }
        public async Task<ILoginSession> QueryLoginAsync(string sessionToken)
        {
            var result = default(ILoginSession);

            if (Adapter == AdapterType.Controller)
            {
                var accountManager = Logic.Factory.CreateAccountManager();

                result = await accountManager.QueryLoginAsync(sessionToken).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                using var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.QueryLoginAsync(sessionToken).ConfigureAwait(false);
            }
            return result;
        }
    }
}
#endif
//MdEnd