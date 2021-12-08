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
        private IEnumerable<IdentityModel> displayModels = null;
        private IEnumerable<PropertyInfo> displayProperties = null;
        private IEnumerable<PropertyInfo> filterProperties = null;
        private IEnumerable<PropertyInfo> orderProperties = null;

        public IEnumerable<IdentityModel> Models { get; init; }
        public IEnumerable<IdentityModel> DisplayModels
        {
            get
            {
                if (displayModels == null)
                {
                    if (Models is IEnumerable<IMasterDetails> mds)
                    {
                        displayModels = mds.Select(m => m.Master);
                    }
                    else
                    {
                        displayModels = Models;
                    }
                }
                return displayModels;
            }
        }
        public List<string> IgnoreFilters { get; } = new List<string>();
        public IEnumerable<string> AllIgnoreFilters
        {
            get 
            {
                return IgnoreNames.Union(IgnoreFilters)
                                  .Union(ViewBagInfo.IgnoreFilters)
                                  .Distinct(); 
            }
        }
        public List<string> IgnoreOrders { get; } = new List<string>();
        public IEnumerable<string> AllIgnoreOrders
        {
            get
            {
                return IgnoreNames.Union(IgnoreOrders)
                                  .Union(ViewBagInfo.IgnoreOrders)
                                  .Distinct();
            }
        }

        public IndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<IdentityModel> models, Type modelType, Type displayType)
            : base(viewBagWrapper, modelType, displayType)
        {
            models.CheckArgument(nameof(models));

            Constructing();
            Models = models;
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
            return displayProperties ??= GetDisplayProperties(DisplayType);
        }
        public virtual IEnumerable<PropertyInfo> GetFilterProperties()
        {
            return filterProperties ??= GetFilterProperties(DisplayType);
        }
        public virtual IEnumerable<PropertyInfo> GetOrderProperties()
        {
            return orderProperties ??= GetOrderProperties(DisplayType);
        }
        public virtual IEnumerable<PropertyInfo> GetFilterProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new List<PropertyInfo>();
            var typeProperties = type.GetAllPropertyInfos();

            foreach (var item in type.GetAllInterfacePropertyInfos())
            {
                var typeProperty = default(PropertyInfo);
                var mapName = ViewBagInfo.GetMapping(item.Name);

                typeProperty = typeProperties.FirstOrDefault(p => p.Name.Equals(mapName, StringComparison.OrdinalIgnoreCase));
                if (typeProperty != null)
                {
                    ViewBagInfo.AddMappingProperty(mapName, typeProperty);
                }

                if (item.CanRead && AllDisplayNames.Any(e => e.Equals(item.Name)))
                {
                    result.Add(item);
                }
                else if (item.CanRead && AllDisplayNames.Any() == false && AllIgnoreFilters.Any(e => e.Equals(item.Name)) == false)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public virtual IEnumerable<PropertyInfo> GetOrderProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new List<PropertyInfo>();
            var typeProperties = type.GetAllPropertyInfos();

            foreach (var item in type.GetAllInterfacePropertyInfos())
            {
                var typeProperty = default(PropertyInfo);
                var mapName = ViewBagInfo.GetMapping(item.Name);

                typeProperty = typeProperties.FirstOrDefault(p => p.Name.Equals(mapName, StringComparison.OrdinalIgnoreCase));
                if (typeProperty != null)
                {
                    ViewBagInfo.AddMappingProperty(mapName, typeProperty);
                }

                if (item.CanRead && AllDisplayNames.Any(e => e.Equals(item.Name)))
                {
                    result.Add(item);
                }
                else if (item.CanRead && AllDisplayNames.Any() == false && AllIgnoreOrders.Any(e => e.Equals(item.Name)) == false)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public override object GetValue(object model, PropertyInfo propertyInfo)
        {
            model.CheckArgument(nameof(model));
            propertyInfo.CheckArgument(nameof(propertyInfo));

            bool handled = false;
            object result = null;

            BeforeGetValue(model, propertyInfo, ref result, ref handled);
            if (handled == false)
            {
                result = base.GetValue(model, propertyInfo);
            }
            AfterGetValue(model, propertyInfo, result);
            return result;
        }
        partial void BeforeGetValue(object model, PropertyInfo propertyInfo, ref object result, ref bool handled);
        partial void AfterGetValue(object model, PropertyInfo propertyInfo, object result);
        public override string GetDisplayValue(object model, PropertyInfo propertyInfo)
        {
            model.CheckArgument(nameof(model));
            propertyInfo.CheckArgument(nameof(propertyInfo));

            bool handled = false;
            object result = null;

            BeforeGetDisplayValue(model, propertyInfo, ref result, ref handled);
            if (handled == false)
            {
                result = base.GetDisplayValue(model, propertyInfo);
            }
            AfterGetDisplayValue(model, propertyInfo, result);
            return result != null ? result.ToString() : string.Empty;
        }
        partial void BeforeGetDisplayValue(object model, PropertyInfo propertyInfo, ref object result, ref bool handled);
        partial void AfterGetDisplayValue(object model, PropertyInfo propertyInfo, object result);

        public virtual IndexDisplayViewModel CreateDisplayViewModel(ModelObject model)
        {
            return new IndexDisplayViewModel(ViewBagInfo, ModelType, DisplayType, model, GetDisplayProperties());
        }
    }
}
//MdEnd