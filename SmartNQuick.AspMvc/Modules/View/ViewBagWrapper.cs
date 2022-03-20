//@BaseCode
//MdStart

using SmartNQuick.AspMvc.Models;
using SmartNQuick.AspMvc.Models.Modules.Common;
using System;
using System.Reflection;

namespace SmartNQuick.AspMvc.Modules.View
{
    public class ViewBagWrapper
    {
        private dynamic ViewBag { get; set; }
        public ViewBagWrapper(dynamic viewBag)
        {
            ViewBag = viewBag;
        }

        public bool HasSearchBox
        {
            get => ViewBag.HasSearchBox != null ? (bool)ViewBag.HasSearchBox : true;
            set => ViewBag.HasSearchBox = value;
        }
        public bool HasSorter
        {
            get => ViewBag.HasSorter != null ? (bool)ViewBag.HasSorter : true;
            set => ViewBag.HasSorter = value;
        }
        public bool HasPagerTop
        {
            get => ViewBag.HasPagerTop != null ? (bool)ViewBag.HasPagerTop : true;
            set => ViewBag.HasPagerTop = value;
        }
        public bool HasPagerBottom
        {
            get => ViewBag.HasPagerBottom != null ? (bool)ViewBag.HasPagerBottom : false;
            set => ViewBag.HasPagerBottom = value;
        }
        public bool HasFilter
        {
            get => ViewBag.HasFilter != null ? (bool)ViewBag.HasFilter : false;
            set => ViewBag.HasFilter = value;
        }
        public EditMode EditMode
        {
            get
            {
                var result = EditMode.Insert | EditMode.Edit | EditMode.Delete;

                if (ViewBag.EditMode != null)
                {
                    result = ViewBag.EditMode;
                }
                return result;
            }
            set => ViewBag.EditMode = value;
        }
        public CommandMode CommandMode
        {
            get
            {
                var result = CommandMode.All;

                if (ViewBag.CommandMode != null)
                {
                    result = ViewBag.CommandMode;
                }
                return result;
            }
            set => ViewBag.CommandMode = value;
        }
        public object ParentModel
        {
            get => ViewBag.ParentModel as object;
            set => ViewBag.ParentModel = value;
        }

        public string Title => Translate(Controller);
        public string Controller
        {
            get => ViewBag.Controller as string;
            set => ViewBag.Controller = value;
        }
        public string Action
        {
            get => ViewBag.Action as string;
            set => ViewBag.Action = value;
        }
        public string ItemPrefix
        {
            get => ViewBag.ItemPrefix as string;
            set => ViewBag.ItemPrefix = value;
        }
        public int Index
        {
            get => ViewBag.Index != null ? (int)ViewBag.Index : 0;
            set => ViewBag.Index = value;
        }
        public bool Handled
        {
            get => ViewBag.Handled != null ? (bool)ViewBag.Handled : false;
            set => ViewBag.Handled = value;
        }
        public List<string> HiddenNames
        {
            get
            {
                if (ViewBag.HiddenNames is not List<string> result)
                {
                    ViewBag.HiddenNames = result = new List<string>();
                }
                return result;
            }
        }
        public List<string> IgnoreNames
        {
            get
            {
                if (ViewBag.IgnoreNames is not List<string> result)
                {
                    ViewBag.IgnoreNames = result = new List<string>();
                }
                return result;
            }
        }
        public List<string> IgnoreSearchItems
        {
            get
            {
                if (ViewBag.IgnoreSearchItems is not List<string> result)
                {
                    ViewBag.IgnoreSearchItems = result = new List<string>();
                }
                return result;
            }
        }
        public List<string> IgnoreFilters
        {
            get
            {
                if (ViewBag.IgnoreFilters is not List<string> result)
                {
                    ViewBag.IgnoreFilters = result = new List<string>();
                }
                return result;
            }
        }
        public List<string> IgnoreOrders
        {
            get
            {
                if (ViewBag.IgnoreOrders is not List<string> result)
                {
                    ViewBag.IgnoreOrders = result = new List<string>();
                }
                return result;
            }
        }
        public List<string> DisplayNames
        {
            get
            {
                if (ViewBag.DisplayNames is not List<string> result)
                {
                    ViewBag.DisplayNames = result = new List<string>();
                }
                return result;
            }
        }
        public PropertyInfo DisplayProperty
        {
            get => ViewBag.DisplayProperty as PropertyInfo;
            set => ViewBag.DisplayProperty = value;
        }
        public Dictionary<string, string> MappingNames
        {
            get
            {
                if (ViewBag.MappingNames is not Dictionary<string, string> result)
                {
                    ViewBag.MappingNames = result = new Dictionary<string, string>();
                }
                return result;
            }
        }
        public Dictionary<string, PropertyInfo> MappingProperties
        {
            get
            {
                if (ViewBag.MappingProperties is not Dictionary<string, PropertyInfo> result)
                {
                    ViewBag.MappingProperties = result = new Dictionary<string, PropertyInfo>();
                }
                return result;
            }
        }

        public void AddIgnoreHidden(string name)
        {
            if (IgnoreNames.Contains(name) == false)
                IgnoreNames.Add(name);

            if (HiddenNames.Contains(name) == false)
                HiddenNames.Add(name);
        }
        public void AddIgnoreFilterOrder(string name)
        {
            if (IgnoreFilters.Contains(name) == false)
                IgnoreFilters.Add(name);

            if (IgnoreOrders.Contains(name) == false)
                IgnoreOrders.Add(name);
        }

        public string GetMapping(string key)
        {
            if (MappingNames.TryGetValue(key, out var result) == false)
            {
                result = key;
            }
            return result;
        }
        public void AddMapping(string key, string value)
        {
            if (MappingNames.ContainsKey(key) == false)
            {
               MappingNames.Add(key, value);
            }
        }
        public bool GetMappingProperty(string key, out PropertyInfo propertyInfo)
        {
            return MappingProperties.TryGetValue(key, out propertyInfo);
        }
        public void AddMappingProperty(string key, PropertyInfo propertyInfo)
        {
            if (MappingProperties.ContainsKey(key) == false)
            {
                MappingProperties.Add(key, propertyInfo);
            }
        }

        public Func<string, string> Translate
        {
            get
            {
                return ViewBag.Translate is Func<string, string> result ? result : s => s;
            }
            set => ViewBag.Translate = value;
        }
        public Func<string, string> TranslateFor => text => Translate($"{Controller}.{text}");

        public static Type GetDisplayType(Type modelType)
        {
            modelType.CheckArgument(nameof(modelType));

            var result = modelType;

            if (modelType.IsGenericTypeOf(typeof(OneToManyModel<,,,>)))
            {
                var genericTypes = modelType.BaseType.GetGenericArguments();

                if (genericTypes.Length > 1)
                {
                    result = genericTypes[1];
                }
            }
            else if (modelType.IsGenericTypeOf(typeof(OneToAnotherModel<,,,>)))
            {
                var genericTypes = modelType.BaseType.GetGenericArguments();

                if (genericTypes.Length > 1)
                {
                    result = genericTypes[1];
                }
            }
            else if (modelType.IsGenericTypeOf(typeof(CompositeModel<,,,,,>)))
            {
                var genericTypes = modelType.BaseType.GetGenericArguments();

                if (genericTypes.Length > 1)
                {
                    result = genericTypes[1];
                }
            }
            return result;
        }
    }
}
//MdEnd