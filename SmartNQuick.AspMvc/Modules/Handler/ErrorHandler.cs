//@BaseCode
//MdStart

using CommonBase.Extensions;
using System.Collections.Generic;

namespace SmartNQuick.AspMvc.Modules.Handler
{
    public static class ErrorHandler
    {
        private static string lastError;
        private static readonly List<string> errorList = new ();

        public static string LastError 
        { 
            get => lastError; 
            set
            {
                if (lastError.HasContent())
                {
                    errorList.Add(lastError);
                }
                lastError = value;
            }
        }
        public static void Clear() => errorList.Clear();
        public static string GetLastErrorAndClear()
        {
            var result = LastError;

            LastError = null;
            return result;
        }
        public static IEnumerable<string> GetErrors()
        {
            return errorList;
        }
    }
}
//MdEnd
