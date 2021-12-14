//@BaseCode
//MdStart
using System;

namespace CommonBase.Attributes
{
    /// <summary>
    /// This attribute is used to define a property at interface level. Typically, 
    /// this attribute is used to redefine inheritance properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public class ContractMemberInfoAttribute : PropertyInfoAttribute
    {
        public string PropertyName { get; set; }
    }
}
//MdEnd