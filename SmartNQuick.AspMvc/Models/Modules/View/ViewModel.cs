//@BaseCode
//MdStart
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public abstract partial class ViewModel
    {
        private List<PropertyInfo> hiddenProperties = null;
        private List<PropertyInfo> displayProperties = null;
        public List<string> HiddenNames { get; } = new List<string>()
        {
            nameof(IdentityModel.Id),
            nameof(VersionModel.RowVersion),
        };
        public List<string> IgnoreNames { get; } = new List<string>()
        {
            nameof(IdentityModel.Id),
            nameof(VersionModel.RowVersion),
            nameof(ModelObject.ViewBagInfo)
        };
        public List<string> DisplayNames { get; } = new List<string>()
        {
        };

        public abstract Type ModelType { get; }
        public virtual List<PropertyInfo> HiddenProperties
        {
            get => hiddenProperties ??= InitHiddenProperties();
        }
        public virtual List<PropertyInfo> DisplayProperties 
        { 
            get => displayProperties ??= InitDisplayProperties();
        }
        protected ViewModel(string[] hiddenNames, string[] ignoreNames, string[] displayNames)
        {
            if (hiddenNames != null)
                HiddenNames.AddRange(hiddenNames);

            if (ignoreNames != null)
                IgnoreNames.AddRange(ignoreNames);

            if (displayNames != null)
                DisplayNames.AddRange(displayNames);
        }

        protected virtual List<PropertyInfo> InitHiddenProperties()
        {
            var result = new List<PropertyInfo>();

            foreach (var item in ModelType.GetAllInterfacePropertyInfos())
            {
                if (item.CanRead && HiddenNames.Any(e => e.Equals(item.Name)))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        protected virtual List<PropertyInfo> InitDisplayProperties()
        {
            var result = new List<PropertyInfo>();

            foreach (var item in ModelType.GetAllInterfacePropertyInfos())
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
        public virtual string GetLabel(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            return propertyInfo.Name;
        }
    }
}
//MdEnd