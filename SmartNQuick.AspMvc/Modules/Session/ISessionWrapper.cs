//@BaseCode
//MdStart

using SmartNQuick.AspMvc.Models.Modules.View;

namespace SmartNQuick.AspMvc.Modules.Session
{
    public partial interface ISessionWrapper
    {
        #region General
        bool HasValue(string key);
        void Remove(string key);
        #endregion General

        #region Object-Access
        void SetValue(string key, object value);
        object GetValue(string key);
        #endregion Object-Access

        #region Int-Access
        void SetIntValue(string key, int value);
        int GetIntValue(string key);
        #endregion Int-Access

        #region String-Access
        void SetStringValue(string key, string value);
        string GetStringValue(string key);
        string GetStringValue(string key, string defaultValue);
        #endregion String-Access

        #region Properties
        string ReturnUrl { get; set; }
        string ReturnController { get; set; }
        string ReturnAction { get; set; }
        string Hint { get; set; }
        string Error { get; set; }
        #endregion Properties

        #region Filter properties
        void SetSearchFilter(string controllerName, string value);
        string GetSearchFilter(string controllerName);
        void SetFilterPredicate(string controllerName, string value);
        string GetFilterPredicate(string controllerName);
        void SetFilterModel(string controllerName, FilterModel filterModel);
        FilterModel GetFilterModel(string controllerName);
        void SetFilterValues(string controllerName, FilterValues filterValues);
        FilterValues GetFilterValues(string controllerName);
        #endregion Filter properties

        #region Sorter properties
        void SetSorterValues(string controllerName, SorterValues sorterValues);
        SorterValues GetSorterValues(string controllerName);
        #endregion Sorter properties

        #region Page properties
        void SetPageCount(string controllerName, int value);
        int GetPageCount(string controllerName);

        void SetPageSizes(string controllerName, int[] values);
        int[] GetPageSizes(string controllerName);

        void SetPageSize(string controllerName, int value);
        int GetPageSize(string controllerName);
        void SetPageIndex(string controllerName, int value);
        int GetPageIndex(string controllerName);
        #endregion Page properties

#if ACCOUNT_ON
        #region Authentication
        Models.Persistence.Account.LoginSession LoginSession { get; set; }
        string SessionToken => LoginSession?.SessionToken;
        bool IsAuthenticated { get; }
        bool IsSessionAlive { get; }
        #endregion Authentication
#endif
    }
}
//MdEnd
