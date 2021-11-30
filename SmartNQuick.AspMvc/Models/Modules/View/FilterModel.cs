//@BaseCode
//MdStart
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartNQuick.AspMvc.Modules.Session;
using SmartNQuick.AspMvc.Modules.View;
using System.Collections.Generic;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public class FilterModel : Dictionary<string, string>
    {
        public SessionWrapper Session { get; init; }
        public ViewBagWrapper ViewBagInfo { get; init; }
        public IndexViewModel IndexViewModel { get; init; }

        public FilterModel(SessionWrapper session, ViewBagWrapper viewBagInfo, IndexViewModel indexViewModel)
        {
            session.CheckArgument(nameof(session));
            viewBagInfo.CheckArgument(nameof(viewBagInfo));
            indexViewModel.CheckArgument(nameof(indexViewModel));

            Session = session;
            ViewBagInfo = viewBagInfo;
            IndexViewModel = indexViewModel;
        }

        public SelectList GetFieldOperations()
        {
            var operations = new List<SelectListItem>();

            operations.Add(new SelectListItem { Selected = true, Value = string.Empty, Text = string.Empty });
            operations.Add(new SelectListItem { Selected = true, Value = "and", Text = "And" });
            operations.Add(new SelectListItem { Selected = true, Value = "or", Text = "Or" });
            return new SelectList(operations, "Value", "Text");
        }
        public SelectList GetTypeOperations(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var operations = new List<SelectListItem>();

            operations.Add(new SelectListItem { Selected = true, Value = string.Empty, Text = string.Empty });
            if (propertyInfo.PropertyType == typeof(string))
            {
                operations.Add(new SelectListItem { Selected = true, Value = "in", Text = "In" });
                operations.Add(new SelectListItem { Selected = true, Value = "==", Text = "=" });
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                operations.Add(new SelectListItem { Selected = true, Value = "==", Text = "=" });
                operations.Add(new SelectListItem { Selected = true, Value = ">", Text = ">" });
                operations.Add(new SelectListItem { Selected = true, Value = "<", Text = "<" });
            }
            else
            {
                operations.Add(new SelectListItem { Selected = true, Value = "==", Text = "=" });
            }
            return new SelectList(operations, "Value", "Text");
        }
    }
}
//MdEnd