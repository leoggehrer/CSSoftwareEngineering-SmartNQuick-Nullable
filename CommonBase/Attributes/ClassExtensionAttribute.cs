//@BaseCode
//MdStart
using System;

namespace CommonBase.Attributes
{
    /// <summary>
    /// These attributes identify the class which has extension methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public partial class ClassExtensionAttribute : Attribute
    {
        public string MappingName { get; set; }
    }
}
//MdEnd