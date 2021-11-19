//@BaseCode
//MdStart
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
        public List<string> IgnoreNames { get; } = new List<string>()
        {
            nameof(IdentityModel.Id),
            nameof(VersionModel.RowVersion),
            nameof(VersionModel.RowVersionString),
            nameof(ModelObject.ViewBagInfo)
        };
        public List<string> DisplayNames { get; } = new List<string>()
        {
        };

        public abstract Type ModelType { get; }
        protected ViewModel(ViewBagWrapper viewBagWrapper)
        {
            viewBagWrapper.CheckArgument(nameof(viewBagWrapper));

            ViewBagWrapper = viewBagWrapper;

            HiddenNames.AddRange(viewBagWrapper.HiddenNames);
            IgnoreNames.AddRange(viewBagWrapper.IgnoreNames);
            DisplayNames.AddRange(viewBagWrapper.DisplayNames);
        }

        public virtual IEnumerable<PropertyInfo> GetHiddenProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            return HiddenNames.Select(n => ViewBagWrapper.GetMapping(n))
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
                var property = default(PropertyInfo);
                var mapName = ViewBagWrapper.GetMapping(item.Name);

                if (mapName.Equals(item.Name) == false)
                {
                    property = typeProperties.FirstOrDefault(p => p.Name.Equals(mapName, StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        ViewBagWrapper.AddMappingProperty(mapName, item);
                    }
                }

                property ??= item;

                if (property.CanRead && DisplayNames.Any(e => e.Equals(property.Name)))
                {
                    result.Add(property);
                }
                else if (property.CanRead && DisplayNames.Count == 0 && IgnoreNames.Any(e => e.Equals(item.Name)) == false)
                {
                    result.Add(property);
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