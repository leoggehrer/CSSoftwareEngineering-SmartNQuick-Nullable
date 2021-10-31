//@BaseCode
//MdStart

using CommonBase.Extensions;
using System;
using System.Collections.Generic;

namespace SmartNQuick.AspMvc.Modules.Handler
{
    public static class ErrorHandler
    {
        private static string lastError;
        private static readonly List<string> errorList = new();

        public static string LastError
        {
            get => lastError;
            set
            {
                if (string.IsNullOrEmpty(lastError) == false)
                {
                    if (errorList.Count >= 10)
                        errorList.RemoveAt(0);

                    errorList.Add(lastError);
                }
                lastError = value;
            }
        }
        public static bool HasError => string.IsNullOrEmpty(LastError) == false;
        public static void Clear()
        {
            lastError = null;
            errorList.Clear();
        }
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
        #region ExceptionExtension
        public static string GetFullError(this Exception source)
        {
            source.CheckArgument(nameof(source));

            var tab = string.Empty;
            var errMsg = source.Message;
            Exception innerException = source.InnerException;

            while (innerException != null)
            {
                tab += "\t";
                errMsg = $"{errMsg}{Environment.NewLine}{tab}{innerException.Message}";
                innerException = innerException.InnerException;
            }
            return errMsg;
        }
        #endregion ExceptionExtension
    }
}
//MdEnd
