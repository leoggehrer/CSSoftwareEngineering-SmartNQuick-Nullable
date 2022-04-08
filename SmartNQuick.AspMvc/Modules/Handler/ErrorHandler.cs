//@BaseCode
//MdStart

using System;

namespace SmartNQuick.AspMvc.Modules.Handler
{
    public static class ErrorHandler
    {
        private static string? lastViewError;
        private static readonly List<string> viewErrorList = new();
        private static string? lastLogicError;
        private static readonly List<string> logicErrorList = new();

        public static bool HasViewError => string.IsNullOrEmpty(LastViewError) == false;
        public static string? LastViewError
        {
            get => lastViewError;
            set
            {
                if (string.IsNullOrEmpty(lastViewError) == false)
                {
                    if (viewErrorList.Count >= 10)
                        viewErrorList.RemoveAt(0);

                    viewErrorList.Add(lastViewError);
                }
                lastViewError = value;
            }
        }
        public static bool HasLogicError => string.IsNullOrEmpty(lastLogicError) == false;
        public static string? LastLogicError
        {
            get => lastLogicError;
            set
            {
                if (string.IsNullOrEmpty(lastLogicError) == false)
                {
                    if (logicErrorList.Count >= 10)
                        logicErrorList.RemoveAt(0);

                    logicErrorList.Add(lastLogicError);
                }
                lastLogicError = value;
            }
        }

        public static void Clear()
        {
            lastViewError = null;
            viewErrorList.Clear();
            logicErrorList.Clear();
        }
        public static string? GetLastViewErrorAndClear()
        {
            var result = LastViewError;

            LastViewError = null;
            return result;
        }
        public static IEnumerable<string> GetViewErrors()
        {
            return viewErrorList;
        }
        public static IEnumerable<string> GetLogicErrors()
        {
            return logicErrorList;
        }

        #region ExceptionExtension
        public static string GetFullError(this Exception source)
        {
            source.CheckArgument(nameof(source));

            var tab = string.Empty;
            var errMsg = source.Message;
            Exception? innerException = source.InnerException;

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
