//@BaseCode
//MdStart

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

        public bool Handled
        {
            get => ViewBag.Handled != null ? (bool)ViewBag.Handled : false;
            set => ViewBag.Handled = value;
        }
        public PropertyInfo DisplayProperty
        {
            get => ViewBag.DisplayProperty as PropertyInfo;
            set => ViewBag.DisplayProperty = value;
        }
        public string[] HiddenNames
        {
            get => ViewBag.HiddenNames as string[];
            set => ViewBag.HiddenNames = value;
        }
        public string[] IgnoreNames
        {
            get => ViewBag.IgnoreNames as string[];
            set => ViewBag.IgnoreNames = value;
        }
        public string[] DisplayNames
        {
            get => ViewBag.DisplayNames as string[];
            set => ViewBag.DisplayNames = value;
        }

        public ViewModelCreator ViewModelCreator
        {
            get => ViewBag.ViewModelCreator as ViewModelCreator;
            set => ViewBag.ViewModelCreator = value;
        }

        public Func<string, string> Translate
        {
            get
            {
                var result = ViewBag.Translate as Func<string, string>;

                return result != null ? result : s => s;
            }
            set => ViewBag.Translate = value;
        }
        public Func<string, string> TranslateFor => text => Translate($"{Controller}.{text}");

        public IndexViewModel CreateIndexViewModel(IEnumerable<Models.IdentityModel> models)
        {
            return CreateIndexViewModel(Controller, models);
        }
        public IndexViewModel CreateIndexViewModel(string viewName, IEnumerable<Models.IdentityModel> models)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateIndexViewModel(viewName, models, this) 
                                            : new ViewModelCreator().CreateIndexViewModel(viewName, models, this);
        }
        public EditViewModel CreateEditViewModel(Models.IdentityModel model)
        {
            return CreateEditViewModel(Controller, model);
        }
        public EditViewModel CreateEditViewModel(string viewName, Models.IdentityModel model)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateEditViewModel(viewName, model, this)
                                            : new ViewModelCreator().CreateEditViewModel(viewName, model, this);
        }
        public DisplayViewModel CreateDisplayViewModel(Models.IdentityModel model)
        {
            return CreateDisplayViewModel(Controller, model);
        }
        public DisplayViewModel CreateDisplayViewModel(string viewName, Models.IdentityModel model)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateDisplayViewModel(viewName, model, this)
                                            : new ViewModelCreator().CreateDisplayViewModel(viewName, model, this);
        }
    }
}
//MdEnd