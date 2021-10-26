//@BaseCode
//MdStart
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using SmartNQuick.AspMvc.Extensions;

namespace SmartNQuick.AspMvc.Modules.Session
{
    public partial class SessionWrapper : ISessionWrapper
    {
        private ISession Session { get; }

        public SessionWrapper(ISession session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        #region General
        public bool HasValue(string key)
        {
            return Session.TryGetValue(key, out _);
        }
        public void Remove(string key)
        {
            Session.Remove(key);
        }
        #endregion General

        #region Object-Access
        public void SetValue(string key, object value)
        {
            Session.Set<object>(key, value);
        }
        public object GetValue(string key)
        {
            return Session.Get<object>(key);
        }
        #endregion Object-Access

        #region Int-Access
        public void SetIntValue(string key, int value)
        {
            Session.Set<int>(key, value);
        }
        public int GetIntValue(string key)
        {
            return Session.Get<int>(key);
        }
        #endregion Int-Access

        #region String-Access
        public void SetStringValue(string key, string value)
        {
            Session.Set<string>(key, value);
        }
        public string GetStringValue(string key)
        {
            return Session.Get<string>(key);
        }
        public string GetStringValue(string key, string defaultValue)
        {
            var result = Session.Get<string>(key);

            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }
        #endregion String-Access

        #region Properties
        public string ReturnUrl
        {
            get
            {
                return GetStringValue(nameof(ReturnUrl));
            }
            set
            {
                SetStringValue(nameof(ReturnUrl), value);
            }
        }
        public string Hint
        {
            get
            {
                return GetStringValue(nameof(Hint));
            }
            set
            {
                SetStringValue(nameof(Hint), value);
            }
        }
        public string Error
        {
            get
            {
                return GetStringValue(nameof(Error));
            }
            set
            {
                SetStringValue(nameof(Error), value);
            }
        }
        #endregion Properties

#if ACCOUNT_ON
        #region Authentication
        public Models.Persistence.Account.LoginSession LoginSession
        {
            get => Session.Get<Models.Persistence.Account.LoginSession>(nameof(LoginSession));
            set => Session.Set(nameof(LoginSession), value);
        }
        public string SessionToken
        {
            get
            {
                var loginSession = LoginSession;

                return loginSession != null ? loginSession.SessionToken : string.Empty;
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return LoginSession != null;
            }
        }
        public bool HasRole(string role, params string[] further)
        {
            var result = false;
            var loginSession = LoginSession;

            if (loginSession != null)
            {
                var accMngr = new Adapters.Modules.Account.AccountManager();

                Task.Run(async () => result = await accMngr.HasRoleAsync(loginSession.SessionToken, role).ConfigureAwait(false)).Wait();
                for (int i = 0; result == false && i < further.Length; i++)
                {
                    Task.Run(async () => result = await accMngr.HasRoleAsync(loginSession.SessionToken, further[i]).ConfigureAwait(false)).Wait();
                }
            }
            return result;
        }
        #endregion Authentication
#endif
    }
}
//MdEnd
