//@BaseCode
//MdStart

using SmartNQuick.AspMvc.Models.Modules.Common;
using SmartNQuick.AspMvc.Models.Modules.View;
using System;
using System.Collections.Generic;
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
                var result = CommandMode.Create | CommandMode.Edit | CommandMode.Delete | CommandMode.Details;

                if (ViewBag.CommandMode != null)
                {
                    result = ViewBag.CommandMode;
                }
                return result;
            }
            set => ViewBag.CommandMode = value;
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
        public Type ViewType
        {
            get => ViewBag.ViewType as Type;
            set => ViewBag.ViewType = value;
        }
        public string ItemPrefix
        {
            get => ViewBag.ItemPrefix as string;
            set => ViewBag.ItemPrefix = value;
        }
        public string ViewTypeName => ViewType?.FullName;
        public ViewModelCreator ViewModelCreator
        {
            get => ViewBag.ViewModelCreator as ViewModelCreator;
            set => ViewBag.ViewModelCreator = value;
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
        public void AddIgnoreHidden(string name)
        {
            IgnoreNames.Add(name);
            HiddenNames.Add(name);
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

        public IndexViewModel CreateIndexViewModel(IEnumerable<Models.IdentityModel> models)
        {
            return CreateIndexViewModel(ViewTypeName, models);
        }
        public IndexViewModel CreateIndexViewModel(IEnumerable<Models.IdentityModel> models, Type elementType)
        {
            return CreateIndexViewModel(ViewTypeName, models, elementType);
        }
        public IndexViewModel CreateIndexViewModel(string viewTypeName, IEnumerable<Models.IdentityModel> models)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateIndexViewModel(viewTypeName, models, this) 
                                            : new ViewModelCreator().CreateIndexViewModel(viewTypeName, models, this);
        }
        public IndexViewModel CreateIndexViewModel(string viewTypeName, IEnumerable<Models.IdentityModel> models, Type elementType)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateIndexViewModel(viewTypeName, models, elementType, this)
                                            : new ViewModelCreator().CreateIndexViewModel(viewTypeName, models, elementType, this);
        }

        public EditViewModel CreateEditViewModel(Models.IdentityModel model)
        {
            return CreateEditViewModel(ViewTypeName, model);
        }
        public EditViewModel CreateEditViewModel(string viewTypeName, Models.IdentityModel model)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateEditViewModel(viewTypeName, model, this)
                                            : new ViewModelCreator().CreateEditViewModel(viewTypeName, model, this);
        }

        public DisplayViewModel CreateDisplayViewModel(Models.IdentityModel model)
        {
            return CreateDisplayViewModel(ViewTypeName, model);
        }
        public DisplayViewModel CreateDisplayViewModel(string viewTypeName, Models.IdentityModel model)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateDisplayViewModel(viewTypeName, model, this)
                                            : new ViewModelCreator().CreateDisplayViewModel(viewTypeName, model, this);
        }
    }
}
//MdEnd