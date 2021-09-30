//@BaseCode
//MdStart
using System;

namespace CommonBase.Extensions
{
	public static class ExceptionExtensions
	{
        public static string GetError(this Exception source)
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
    }
}
//MdEnd