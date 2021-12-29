//@BaseCode
//MdStart
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartNQuick.AspMvc.Modules.Session;
using SmartNQuick.AspMvc.Modules.View;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public class FilterModel
    {
        public ISessionWrapper SessionInfo { get; init; }
        public ViewBagWrapper ViewBagInfo => IndexViewModel.ViewBagInfo;
        public IndexViewModel IndexViewModel { get; init; }

        public FilterModel(ISessionWrapper sessionInfo, IndexViewModel indexViewModel)
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
        public virtual string GetTypeOperationId(PropertyInfo propertyInfo)
        {
            return $"{GetId(propertyInfo)}_{StaticLiterals.TypeOperationPostfix}";
        }
        public virtual string GetTypeOperationName(PropertyInfo propertyInfo)
        {
            return $"{GetName(propertyInfo)}.{StaticLiterals.TypeOperationPostfix}";
        }
        public virtual string GetFieldOperationId(PropertyInfo propertyInfo)
        {
            return $"{GetId(propertyInfo)}_{StaticLiterals.FieldOperationPostfix}";
        }
        public virtual string GetFieldOperationName(PropertyInfo propertyInfo)
        {
            return $"{GetName(propertyInfo)}.{StaticLiterals.FieldOperationPostfix}";
        }

        public SelectList GetFieldOperations()
        {
            var operations = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Value = string.Empty, Text = string.Empty },
                new SelectListItem { Selected = true, Value = "and", Text = "And" },
                new SelectListItem { Selected = true, Value = "or", Text = "Or" }
            };
            return new SelectList(operations, "Value", "Text");
        }
        public SelectList GetTypeOperations(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var translate = ViewBagInfo.Translate;
            var operationItems = new List<SelectListItem>();

            if (ViewBagInfo.GetMappingProperty(propertyInfo.Name, out var property) == false)
            {
                property = propertyInfo;
            }

            operationItems.Add(new SelectListItem { Value = string.Empty, Text = string.Empty });
            if (property.PropertyType == typeof(string))
            {
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationEquals, Text = translate(StaticLiterals.OperationEquals) });
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationNotEquals, Text = translate(StaticLiterals.OperationNotEquals) });
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationContains, Text = translate(StaticLiterals.OperationContains) });
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationStartsWith, Text = translate(StaticLiterals.OperationStartsWith) });
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationEndsWith, Text = translate(StaticLiterals.OperationEndsWith) });
            }
            else if (property.PropertyType == typeof(int))
            {
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationNumEquals, Text = translate(StaticLiterals.OperationNumEquals) });
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationNumIsGreater, Text = translate(StaticLiterals.OperationNumIsGreater) });
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationNumIsLess, Text = translate(StaticLiterals.OperationNumIsLess) });
            }
            else
            {
                operationItems.Add(new SelectListItem { Value = StaticLiterals.OperationEquals, Text = translate(StaticLiterals.OperationEquals) });
            }
            return new SelectList(operationItems, "Value", "Text");
        }
        public FilterValues GetFilterValues(IFormCollection formCollection)
        {
            formCollection.CheckArgument(nameof(formCollection));

            var result = new FilterValues();

            foreach (var property in IndexViewModel.GetDisplayProperties())
            {
                var operationName = GetTypeOperationName(property);
                var operationValue = formCollection[operationName];

                if (operationValue.Count > 0 && string.IsNullOrEmpty(operationValue[0]) == false)
                {
                    var operandName = GetName(property);
                    var operandValue = formCollection[operandName];

                    if (operandValue.Count > 0 && string.IsNullOrEmpty(operandValue[0]) == false)
                    {
                        var filterItem = new FilterItem()
                        {
                            Name = property.Name,
                            Operation = operationValue,
                            Value = operandValue[0],
                        };
                        result[property.Name] = filterItem;
                    }
                }
            }
            return result;
        }
    }
}
//MdEnd