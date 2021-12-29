//@BaseCode
//MdStart
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartNQuick.AspMvc.Modules.Session;
using SmartNQuick.AspMvc.Modules.View;
using System;
using System.Reflection;
using System.Text;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public class SearchModel
    {
        public ISessionWrapper SessionInfo { get; init; }
        public ViewBagWrapper ViewBagInfo => IndexViewModel.ViewBagInfo;
        public IndexViewModel IndexViewModel { get; init; }

        public SearchModel(ISessionWrapper sessionInfo, IndexViewModel indexViewModel)
        {
            sessionInfo.CheckArgument(nameof(sessionInfo));
            indexViewModel.CheckArgument(nameof(indexViewModel));

            SessionInfo = sessionInfo;
            IndexViewModel = indexViewModel;
        }

        public virtual string GetSearchValueId()
        {
            var prefix = ViewBagInfo.ItemPrefix;
            var result = "SearchValue";

            if (prefix.HasContent())
            {
                result = $"{prefix}_{result}";
            }
            return result;
        }
        public virtual string GetSearchValueName()
        {
            var prefix = ViewBagInfo.ItemPrefix;
            var result = "SearchValue";

            if (prefix.HasContent())
            {
                result = $"{prefix}.{result}";
            }
            return result;
        }
        public virtual string GetSearchItemsId()
        {
            var prefix = ViewBagInfo.ItemPrefix;
            var result = "SearchItems";

            if (prefix.HasContent())
            {
                result = $"{prefix}_{result}";
            }
            return result;
        }
        public virtual string GetSearchItemsName()
        {
            var prefix = ViewBagInfo.ItemPrefix;
            var result = "SearchItems";

            if (prefix.HasContent())
            {
                result = $"{prefix}.{result}";
            }
            return result;
        }

        public static string CreatePredicate(Type type, string value)
        {
            type.CheckArgument(nameof(type));

            var result = new StringBuilder();

            if (string.IsNullOrEmpty(value) == false)
            {
                foreach (var property in type.GetAllInterfacePropertyInfos())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var name = property.Name;

                        if (result.Length > 0)
                        {
                            result.Append(" || ");
                        }
                        result.Append($"{name} != null && {name}.ToLower().Contains(\"{value?.ToLower()}\")");
                    }
                }
            }
            return result.ToString();
        }

        public SelectList GetSearchItems()
        {
            var translate = ViewBagInfo.Translate;
            var items = new List<SelectListItem>();

            items.Add(new SelectListItem { Value = string.Empty, Text = translate("All") });
            foreach (var property in IndexViewModel.GetDisplayProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    items.Add(new SelectListItem { Value = property.Name, Text = translate(property.Name) });
                }
            }
            return new SelectList(items, "Value", "Text");
        }
    }
}
//MdEnd