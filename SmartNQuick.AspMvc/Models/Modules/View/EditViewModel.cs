//@BaseCode
//MdStart
using CommonBase.Extensions;
using SmartNQuick.AspMvc.Modules.View;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public partial class EditViewModel : ViewModel
    {
        public IdentityModel Model { get; init; }

        public EditViewModel(ViewBagWrapper viewBagWrapper, IdentityModel model, Type modelType, Type displayType)
            : base(viewBagWrapper, modelType, displayType)
        {
            model.CheckArgument(nameof(model));

            Constructing();
            Model = model;
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public virtual IEnumerable<PropertyInfo> GetHiddenProperties()
        {
            return GetHiddenProperties(DisplayType);
        }
        public virtual IEnumerable<PropertyInfo> GetDisplayProperties()
        {
            return GetDisplayProperties(DisplayType);
        }
        public virtual object GetValue(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            return propertyInfo.GetValue(Model);
        }
        public virtual string GetDisplayValue(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var value = propertyInfo.GetValue(Model);

            return value != null ? value.ToString() : string.Empty;
        }
    }
}
//MdEnd