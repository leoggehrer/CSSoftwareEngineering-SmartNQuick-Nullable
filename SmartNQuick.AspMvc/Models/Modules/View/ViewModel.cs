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
    public abstract partial class ViewModel
    {
        public ViewBagWrapper ViewBagWrapper { get; init; }
        public List<string> HiddenNames { get; } = new List<string>()
        {
            nameof(IdentityModel.Id),
            nameof(VersionModel.RowVersionString),
        };
        public IEnumerable<string> AllHiddenNames
        {
            get { return HiddenNames.Union(ViewBagWrapper.HiddenNames).Distinct(); }
        }
        public List<string> IgnoreNames { get; } = new List<string>()
        {
            nameof(IdentityModel.Id),
            nameof(VersionModel.RowVersion),
            nameof(VersionModel.RowVersionString),
            nameof(ModelObject.ViewBagInfo)
        };
        public IEnumerable<string> AllIgnoreNames
        {
            get { return IgnoreNames.Union(ViewBagWrapper.IgnoreNames).Distinct(); }
        }
        public List<string> DisplayNames { get; } = new List<string>()
        {
        };
        public IEnumerable<string> AllDisplayNames
        {
            get { return DisplayNames.Union(ViewBagWrapper.DisplayNames).Distinct(); }
        }

        public abstract Type ModelType { get; }
        public abstract Type ViewType { get; }
        protected ViewModel(ViewBagWrapper viewBagWrapper)
        {
            viewBagWrapper.CheckArgument(nameof(viewBagWrapper));

            Constructing();
            ViewBagWrapper = viewBagWrapper;
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public void AddIgnoreHidden(string name)
        {
            if (IgnoreNames.Contains(name) == false)
                IgnoreNames.Add(name);

            if (HiddenNames.Contains(name) == false)
                HiddenNames.Add(name);
        }
        public virtual IEnumerable<PropertyInfo> GetHiddenProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            return AllHiddenNames.Select(n => ViewBagWrapper.GetMapping(n))
                                 .Select(n => type.GetProperty(n))
                                 .Where(p => p != null && p.CanRead)
                                 .ToArray();
        }
        public virtual IEnumerable<PropertyInfo> GetDisplayProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new List<PropertyInfo>();
            var typeProperties = type.GetAllPropertyInfos();

            foreach (var item in type.GetAllInterfacePropertyInfos())
            {
                var typeProperty = default(PropertyInfo);
                var mapName = ViewBagWrapper.GetMapping(item.Name);

                typeProperty = typeProperties.FirstOrDefault(p => p.Name.Equals(mapName, StringComparison.OrdinalIgnoreCase));
                if (typeProperty != null)
                {
                    ViewBagWrapper.AddMappingProperty(mapName, typeProperty);
                }

                if (item.CanRead && AllDisplayNames.Any(e => e.Equals(item.Name)))
                {
                    result.Add(item);
                }
                else if (item.CanRead && AllDisplayNames.Any() == false && AllIgnoreNames.Any(e => e.Equals(item.Name)) == false)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public virtual string GetId(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var itemPrefix = ViewBagWrapper.ItemPrefix;

            return string.IsNullOrEmpty(itemPrefix) ? propertyInfo.Name : $"{itemPrefix}_{propertyInfo.Name}";
        }
        public virtual string GetName(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var itemPrefix = ViewBagWrapper.ItemPrefix;

            return string.IsNullOrEmpty(itemPrefix) ? propertyInfo.Name : $"{itemPrefix}.{propertyInfo.Name}";
        }
        public virtual string GetLabel(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            return propertyInfo.Name;
        }
    }
}
//MdEnd