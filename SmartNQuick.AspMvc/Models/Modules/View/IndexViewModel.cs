//@BaseCode
//MdStart
using CommonBase.Extensions;
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
        public IndexViewModel(IEnumerable<IdentityModel> models)
            : this(models, null, null, null)
        {
        }
        public IndexViewModel(IEnumerable<IdentityModel> models,dynamic viewBag)
            : this(models, viewBag.HiddenNames as string[],
                           viewBag.IgnoreNames as string[],
                           viewBag.DisplayNames as string[])
        {
        }
        public IndexViewModel(IEnumerable<IdentityModel> models, string[] hiddenNames, string[] ignoreNames, string[] displayNames)
            : base(hiddenNames, ignoreNames, displayNames)
        {
            models.CheckArgument(nameof(models));

            Models = models;
            Init();
        }
        private void Init()
        {
            foreach (var item in ModelType.GetAllInterfacePropertyInfos())
            {
                if (item.CanRead && HiddenNames.Any(e => e.Equals(item.Name)))
                {
                    HiddenProperties.Add(item);
                }
                if (item.CanRead && DisplayNames.Any(e => e.Equals(item.Name)))
                {
                    DisplayProperties.Add(item);
                }
                else if (item.CanRead && DisplayNames.Count == 0 && IgnoreNames.Any(e => e.Equals(item.Name)) == false)
                {
                    DisplayProperties.Add(item);
                }
            }
        }

        public virtual object GetValue(object model, PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            return propertyInfo.GetValue(model);
        }
        public virtual string GetDisplayValue(object model, PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var value = propertyInfo.GetValue(model);

            return value != null ? value.ToString() : string.Empty;
        }
    }
}
//MdEnd