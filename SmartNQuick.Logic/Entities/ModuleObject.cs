//@BaseCode
//MdStart
using CommonBase.Extensions;
using System;

namespace SmartNQuick.Logic.Entities
{
    internal partial class ModuleObject : EntityObject
    {
        public void CopyProperties(Object other)
        {
            other.CheckArgument(nameof(other));

            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                this.CopyFrom(other);
            }
            AfterCopyProperties(other);
        }
        partial void BeforeCopyProperties(Object other, ref bool handled);
        partial void AfterCopyProperties(Object other);
    }
}
//MdEnd