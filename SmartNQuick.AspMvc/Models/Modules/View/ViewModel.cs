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

            if (viewBagWrapper.HiddenNames != null)
                HiddenNames.AddRange(viewBagWrapper.HiddenNames);

            if (viewBagWrapper.IgnoreNames != null)
                IgnoreNames.AddRange(viewBagWrapper.IgnoreNames);

            if (viewBagWrapper.DisplayNames != null)
                DisplayNames.AddRange(viewBagWrapper.DisplayNames);
        }

        public virtual IEnumerable<PropertyInfo> GetHiddenProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            return HiddenNames.Select(n => type.GetProperty(n)).Where(p => p != null && p.CanRead).ToArray();
        }
        public virtual IEnumerable<PropertyInfo> GetDisplayProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new List<PropertyInfo>();

            foreach (var item in type.GetAllInterfacePropertyInfos())
            {
                if (item.CanRead && DisplayNames.Any(e => e.Equals(item.Name)))
                {
                    result.Add(item);
                }
                else if (item.CanRead && DisplayNames.Count == 0 && IgnoreNames.Any(e => e.Equals(item.Name)) == false)
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