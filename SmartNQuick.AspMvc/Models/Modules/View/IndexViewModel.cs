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
        private Type modelType = null;
        public IEnumerable<IdentityModel> Models { get; init; }
        public override Type ModelType => modelType ??= Models.Any() ? Models.First().GetType() 
                                                                     : Models.GetType().GetGenericArguments().FirstOrDefault(e => e.IsClass || e.IsInterface);

        public IndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<IdentityModel> models)
            : base(viewBagWrapper)
        {
            models.CheckArgument(nameof(models));

            Constructing();
            Models = models;
            Constructed();
        }
        public IndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<IdentityModel> models, Type elementType)
            : base(viewBagWrapper)
        {
            models.CheckArgument(nameof(models));
            elementType.CheckArgument(nameof(elementType));

            Constructing();
            Models = models;
            modelType = elementType;
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        private IEnumerable<PropertyInfo> displayProperties = null;
        public virtual IEnumerable<PropertyInfo> GetHiddenProperties()
        {
            return GetHiddenProperties(ModelType);
        }
        public virtual IEnumerable<PropertyInfo> GetDisplayProperties()
        {
            if (displayProperties == null)
            {
                displayProperties = GetDisplayProperties(ModelType);
            }
            return displayProperties;
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