//@BaseCode
//MdStart
using System;
using System.Threading.Tasks;
#if ACCOUNT_ON
using SmartNQuick.Adapters.Modules.Account;
using System.Reflection;
#endif
using Factory = SmartNQuick.Adapters.Factory;

namespace SmartNQuick.ConApp
{
    internal partial class Program
    {
#if ACCOUNT_ON
        private static string SaUser => "LeoAdmin";
        private static string SaEmail => "LeoAdmin.SmartNQuick@gmx.at";
        private static string SaPwd => "1234LeoAdmin";

        private static string AaUser => "AppAdmin";
        private static string AaEmail => "AppAdmin.SmartNQuick@gmx.at";
        private static string AaPwd => "1234AppAdmin";
        private static string AaRole => "AppAdmin";

        private static string AppUser => "AppUser";
        private static string AppEmail => "AppUser.SmartNQuick@gmx.at";
        private static string AppPwd => "1234AppUser";
        private static string AppRole => "AppUser";

        private static bool AaEnableJwt => true;

        private static async Task InitAppAccessAsync()
        {
            await Logic.Factory.CreateAccountManager().InitAppAccessAsync(SaUser, SaEmail, SaPwd, true).ConfigureAwait(false);
        }
        private static async Task<Contracts.Business.Account.IAppAccess> AddAppAccessAsync(string loginEmail, string loginPwd, string user, string email, string pwd, bool enableJwtAuth, params string[] roles)
        {
            var accMngr = new AccountManager();
            var login = await accMngr.LogonAsync(loginEmail, loginPwd, string.Empty).ConfigureAwait(false);
            using var ctrl = Factory.Create<Contracts.Business.Account.IAppAccess>(login.SessionToken);
            var entity = await ctrl.CreateAsync();

            entity.OneItem.Name = user;
            entity.OneItem.Email = email;
            entity.OneItem.Password = pwd;
            entity.OneItem.EnableJwtAuth = enableJwtAuth;

            foreach (var item in roles)
            {
                var role = entity.CreateManyItem();

                role.Designation = item;
                entity.AddManyItem(role);
            }
            var identity = await ctrl.InsertAsync(entity).ConfigureAwait(false);
            await accMngr.LogoutAsync(login.SessionToken).ConfigureAwait(false);
            return identity;
        }

        private static Contracts.Persistence.Account.ILoginSession Login { get; set; }
        private static Contracts.Client.IAdapterAccess<C> Create<C>()
        {
            if (Login == null)
            {
                Task.Run(async() =>
                {
                    var accMngr = new AccountManager();
                    
                    Login = await accMngr.LogonAsync(AppEmail, AppPwd);
                }).Wait();
            }
            return Factory.Create<C>(Login.SessionToken);
        }
#else
        private static Contracts.Client.IAdapterAccess<C> Create<C>()
        {
            return Factory.Create<C>();
        }
#endif
        static partial void AfterRun()
        {
            Factory.BaseUri = "https://localhost:5003" + "/api";
            Factory.Adapter = Adapters.AdapterType.Controller;

#if ACCOUNT_ON
            Task.Run(async () =>
            {
                try
                {
                    Console.Write("Start: Create accounts:  ");
                    Console.WriteLine(DateTime.Now);
                    await InitAppAccessAsync();
                    await AddAppAccessAsync(SaEmail, SaPwd, AaUser, AaEmail, AaPwd, AaEnableJwt, AaRole);
                    await AddAppAccessAsync(AaEmail, AaPwd, AppUser, AppEmail, AppPwd, AaEnableJwt, AppRole);
                    Console.Write("Finish: Create accounts: ");
                    Console.WriteLine(DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            ).Wait();
#endif
            Task.Run(async () =>
            {
                Console.Write("Start: Import csv:  ");
                Console.WriteLine(DateTime.Now);
                await ImportDataAsync();
                Console.Write("Finish: Import csv: ");
                Console.WriteLine(DateTime.Now);
            }).Wait();
        }
    }
}
//MdEnd