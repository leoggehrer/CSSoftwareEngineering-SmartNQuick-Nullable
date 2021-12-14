//@BaseCode
//MdStart
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartNQuick.AspMvc.Modules.Session;
using SmartNQuick.AspMvc.Modules.View;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public class SorterModel
    {
        public ISessionWrapper SessionInfo { get; init; }
        public ViewBagWrapper ViewBagInfo => IndexViewModel.ViewBagInfo;
        public IndexViewModel IndexViewModel { get; init; }

        public SorterModel(ISessionWrapper sessionInfo, IndexViewModel indexViewModel)
        {
            sessionInfo.CheckArgument(nameof(sessionInfo));
            indexViewModel.CheckArgument(nameof(indexViewModel));

            SessionInfo = sessionInfo;
            IndexViewModel = indexViewModel;
        }

        public virtual string GetId(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            if (ViewBagInfo.GetMappingProperty(propertyInfo.Name, out var property) == false)
            {
                property = propertyInfo;
            }
            var result = ViewBagInfo.ItemPrefix;

            if (result.HasContent())
            {
                result = $"{result}_{property.DeclaringType.Name}_{property.Name}";
            }
            else
            {
                result = $"{property.DeclaringType.Name}_{property.Name}";
            }
            return result;
        }
        public virtual string GetName(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            if (ViewBagInfo.GetMappingProperty(propertyInfo.Name, out var property) == false)
            {
                property = propertyInfo;
            }
            var result = ViewBagInfo.ItemPrefix;

            if (result.HasContent())
            {
                result = $"{result}.{property.DeclaringType.Name}.{property.Name}";
            }
            else
            {
                result = $"{property.DeclaringType.Name}.{property.Name}";
            }
            return result;
        }
        public virtual string GetSortOperationId(PropertyInfo propertyInfo)
        {
            return $"{GetId(propertyInfo)}_{StaticLiterals.SortOperationPostfix}";
        }
        public virtual string GetSortOperationName(PropertyInfo propertyInfo)
        {
            return $"{GetName(propertyInfo)}.{StaticLiterals.SortOperationPostfix}";
        }

        public SelectList GetSortOperations(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var translate = ViewBagInfo.Translate;
            var operations = new List<SelectListItem>();

            if (ViewBagInfo.GetMappingProperty(propertyInfo.Name, out var property) == false)
            {
                property = propertyInfo;
            }

            operations.Add(new SelectListItem { Value = string.Empty, Text = string.Empty });
            operations.Add(new SelectListItem { Value = StaticLiterals.SortAscending, Text = translate(StaticLiterals.SortAscending) });
            operations.Add(new SelectListItem { Value = StaticLiterals.SortDescending, Text = translate(StaticLiterals.SortDescending) });
            return new SelectList(operations, "Value", "Text");
        }
        public SorterValues GetSorterValues(IFormCollection formCollection)
        {
            formCollection.CheckArgument(nameof(formCollection));

            var result = new SorterValues();

            foreach (var property in IndexViewModel.GetDisplayProperties())
            {
                var operationName = GetSortOperationName(property);
                var operationValue = formCollection[operationName];

                if (operationValue.Count > 0 && string.IsNullOrEmpty(operationValue[0]) == false)
                {
                    var sorterItem = new SorterItem()
                    {
                        Name = property.Name,
                        Operation = operationValue[0],
                    };
                    result[property.Name] = sorterItem;
                }
            }
            return result;
        }
    }
}
//MdEnd