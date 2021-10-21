//@BaseCode
//MdStart
using System;
using System.Collections.Generic;

namespace SolutionDockerBuilder.ConApp
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
                if (string.IsNullOrEmpty(lastError) == false)
                {
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
            if (source == null)
                throw new ArgumentNullException(nameof(source));

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
