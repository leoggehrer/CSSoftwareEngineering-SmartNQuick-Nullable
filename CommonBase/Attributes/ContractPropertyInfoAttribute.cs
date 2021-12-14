//@BaseCode
using System;

namespace CommonBase.Attributes
{
    /// <summary>
    /// These attributes contain additional information for a 
    /// property that is evaluated during generating.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public partial class ContractPropertyInfoAttribute : PropertyInfoAttribute
    {
    }
}
