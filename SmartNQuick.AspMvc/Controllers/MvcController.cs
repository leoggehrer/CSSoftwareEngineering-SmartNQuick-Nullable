//@BaseCode
//MdStart
using Microsoft.AspNetCore.Mvc;
using SmartNQuick.AspMvc.Modules.Session;

namespace SmartNQuick.AspMvc.Controllers
{
    public class MvcController : Controller
	{
        private string lastError;
        protected string LastError
        {
            get => lastError;
            set
            {
                lastError = value;
                Modules.Handler.ErrorHandler.LastError = value;
            }
        }
        protected bool HasError => string.IsNullOrEmpty(LastError) == false;

        #region SessionWrapper
        public bool IsSessionAvailable => HttpContext?.Session != null;
        private ISessionWrapper sessionWrapper = null;
        internal ISessionWrapper SessionWrapper => sessionWrapper ??= new SessionWrapper(HttpContext.Session);
        #endregion
    }
}
//MdEnd