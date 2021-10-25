//@BaseCode
//MdStart
using System;

namespace CommonBase.Helpers
{
    public partial class OperationNotFoundException : Exception
    {
        public OperationNotFoundException(string operationName)
            : base(string.Format("The operation called '{0}' was not found!", operationName))
        {

        }
    }
}
//MdEnd