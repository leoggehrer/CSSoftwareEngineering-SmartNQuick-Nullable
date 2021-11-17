//@BaseCode
//MdStart
using CommonBase.Extensions;
using SmartNQuick.AspMvc.Modules.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public partial class IndexViewModel : ViewModel
    {
        public IEnumerable<IdentityModel> Models { get; init; }
        public override Type ModelType => Models.GetType().GetGenericArguments()[0];

        public IndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<IdentityModel> models)
            : base(viewBagWrapper)
        {
            models.CheckArgument(nameof(models));

            Models = models;
        }

        private List<PropertyInfo> displayProperties = null;
        public virtual IEnumerable<PropertyInfo> DisplayProperties
        {
            get
            {
                if (displayProperties == null)
                {
                    displayProperties = new List<PropertyInfo>();

                    foreach (var item in ModelType.GetAllInterfacePropertyInfos())
                    {
                        if (item.CanRead && DisplayNames.Any(e => e.Equals(item.Name)))
                        {
                            displayProperties.Add(item);
                        }
                        else if (item.CanRead && DisplayNames.Count == 0 && IgnoreNames.Any(e => e.Equals(item.Name)) == false)
                        {
                            displayProperties.Add(item);
                        }
                    }
                }
                return displayProperties;
            }
        }

        public virtual object GetValue(object model, PropertyInfo propertyInfo)
        {
            model.CheckArgument(nameof(model));
            propertyInfo.CheckArgument(nameof(propertyInfo));

            bool handled = false;
            object result = null;

            BeforeGetValue(model, propertyInfo, ref result, ref handled);
            if (handled == false)
            {
                result = propertyInfo.GetValue(model);
            }
            AfterGetValue(model, propertyInfo, result);
            return result;
        }
        partial void BeforeGetValue(object model, PropertyInfo propertyInfo, ref object result, ref bool handled);
        partial void AfterGetValue(object model, PropertyInfo propertyInfo, object result);
        public virtual string GetDisplayValue(object model, PropertyInfo propertyInfo)
        {
            model.CheckArgument(nameof(model));
            propertyInfo.CheckArgument(nameof(propertyInfo));

            bool handled = false;
            object result = null;

            BeforeGetDisplayValue(model, propertyInfo, ref result, ref handled);
            if (handled == false)
            {
                result = propertyInfo.GetValue(model);
            }
            AfterGetDisplayValue(model, propertyInfo, result);
            return result != null ? result.ToString() : string.Empty;
        }
        partial void BeforeGetDisplayValue(object model, PropertyInfo propertyInfo, ref object result, ref bool handled);
        partial void AfterGetDisplayValue(object model, PropertyInfo propertyInfo, object result);
    }
}
//MdEnd