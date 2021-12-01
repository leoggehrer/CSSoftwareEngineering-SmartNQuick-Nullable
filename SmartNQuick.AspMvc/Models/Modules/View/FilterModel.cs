//@BaseCode
//MdStart
using CommonBase.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartNQuick.AspMvc.Modules.Session;
using SmartNQuick.AspMvc.Modules.View;
using System.Collections.Generic;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public class FilterModel
    {
        public ISessionWrapper Session { get; init; }
        public ViewBagWrapper ViewBagInfo { get; init; }
        public IndexViewModel IndexViewModel { get; init; }

        public FilterModel(ISessionWrapper session, ViewBagWrapper viewBagInfo, IndexViewModel indexViewModel)
        {
            session.CheckArgument(nameof(session));
            viewBagInfo.CheckArgument(nameof(viewBagInfo));
            indexViewModel.CheckArgument(nameof(indexViewModel));

            Session = session;
            ViewBagInfo = viewBagInfo;
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

            var operations = new List<SelectListItem>();

            if (ViewBagInfo.GetMappingProperty(propertyInfo.Name, out var property) == false)
            {
                property = propertyInfo;
            }

            operations.Add(new SelectListItem { Value = string.Empty, Text = string.Empty });
            if (property.PropertyType == typeof(string))
            {
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationEquals, Text = StaticLiterals.OperationEquals });
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationNotEquals, Text = StaticLiterals.OperationNotEquals });
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationContains, Text = StaticLiterals.OperationContains });
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationStartsWith, Text = StaticLiterals.OperationStartsWith });
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationEndsWith, Text = StaticLiterals.OperationEndsWith });
            }
            else if (property.PropertyType == typeof(int))
            {
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationNumEquals, Text = StaticLiterals.OperationNumEquals });
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationNumIsGreater, Text = StaticLiterals.OperationNumIsGreater });
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationNumIsLess, Text = StaticLiterals.OperationNumIsLess });
            }
            else
            {
                operations.Add(new SelectListItem { Value = StaticLiterals.OperationEquals, Text = StaticLiterals.OperationEquals });
            }
            return new SelectList(operations, "Value", "Text");
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
                            Value = operandValue,
                        };
                        result[property.Name] = filterItem;
//                        result[$"{property.Name}.{StaticLiterals.TypeOperationPostfix}"] = operationValue;
                    }
                }
            }
            return result;
        }
    }
}
//MdEnd