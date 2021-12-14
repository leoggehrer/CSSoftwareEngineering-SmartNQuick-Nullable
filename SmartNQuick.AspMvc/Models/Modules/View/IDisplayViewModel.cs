//@BaseCode
//MdStart
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public interface IDisplayViewModel : IViewModel
    {
        ModelObject Model { get; }
        ModelObject DisplayModel { get; }

        IEnumerable<PropertyInfo> GetDisplayProperties();
        string GetDisplayValue(PropertyInfo propertyInfo);
        object GetValue(PropertyInfo propertyInfo);
    }
}
//MdEnd