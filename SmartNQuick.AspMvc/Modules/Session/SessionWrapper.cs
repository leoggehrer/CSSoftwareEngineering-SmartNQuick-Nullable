//@BaseCode
//MdStart
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using SmartNQuick.AspMvc.Extensions;
using System.Linq;
using SmartNQuick.AspMvc.Models.Modules.View;

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

        #region Filter
        public void SetSearchFilter(string controllerName, string value)
        {
            SetStringValue($"{StaticLiterals.SearchFilterKeyPrefix}{controllerName}", value);
        }
        public string GetSearchFilter(string controllerName)
        {
            return GetStringValue($"{StaticLiterals.SearchFilterKeyPrefix}{controllerName}");
        }
        public void SetFilterPredicate(string controllerName, string value)
        {
            SetStringValue($"{StaticLiterals.FilterPredicateKeyPrefix}{controllerName}", value);
        }
        public string GetFilterPredicate(string controllerName)
        {
            return GetStringValue($"{StaticLiterals.FilterPredicateKeyPrefix}{controllerName}");
        }
        public void SetFilterModel(string controllerName, FilterModel filterModel)
        {
            Session.Set<FilterModel>($"{StaticLiterals.FilterModelKey}{controllerName}", filterModel);
        }
        public FilterModel GetFilterModel(string controllerName)
        {
            return Session.Get<FilterModel>($"{StaticLiterals.FilterModelKey}{controllerName}");
        }
        public void SetFilterValues(string controllerName, FilterValues filterValues)
        {
            Session.Set<FilterValues>($"{StaticLiterals.FilterValuesKey}{controllerName}", filterValues);
        }
        public FilterValues GetFilterValues(string controllerName)
        {
            return Session.Get<FilterValues>($"{StaticLiterals.FilterValuesKey}{controllerName}");
        }
        #endregion Filter

        #region Page-Properties
        public void SetPageCount(string controllerName, int value)
        {
            SetIntValue($"{StaticLiterals.PageCountKeyPrefix}{controllerName}", value);
        }
        public int GetPageCount(string controllerName)
        {
            return GetIntValue($"{StaticLiterals.PageCountKeyPrefix}{controllerName}");
        }

        public void SetPageSizes(string controllerName, int[] values)
        {
            SetValue($"{StaticLiterals.PageSizesKeyPrefix}{controllerName}", values);
        }
        public int[] GetPageSizes(string controllerName)
        {
            return GetValue($"{StaticLiterals.PageSizesKeyPrefix}{controllerName}") is not int[] result ? StaticLiterals.DefaultPageSizes : result;
        }

        public void SetPageSize(string controllerName, int value)
        {
            SetIntValue($"{StaticLiterals.PageSizeKeyPrefix}{controllerName}", value);
        }
        public int GetPageSize(string controllerName)
        {
            int result = GetIntValue($"{StaticLiterals.PageSizeKeyPrefix}{controllerName}");

            return result == 0 ? StaticLiterals.DefaultPageSizes.Min() : result;
        }
        public void SetPageIndex(string controllerName, int value)
        {
            SetIntValue($"{StaticLiterals.PageIndexKeyPrefix}{controllerName}", value);
        }
        public int GetPageIndex(string controllerName)
        {
            return GetIntValue($"{StaticLiterals.PageIndexKeyPrefix}{controllerName}");
        }
        #endregion Page-Properties

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
