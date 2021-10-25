//@BaseCode
//MdStart
using System;

namespace CommonBase.Attributes
{
    /// <summary>
    /// These attribute identify the extension method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public partial class MethodExtensionAttribute : Attribute
    {
        public string MappingName { get; set; }
    }
}
//MdEnd