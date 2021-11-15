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
                var result = CommandMode.Create | CommandMode.Edit | CommandMode.Remove | CommandMode.ShowDetails;

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
        public string ViewTypeName => ViewType?.FullName;
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
            return CreateIndexViewModel(ViewTypeName, models);
        }
        public IndexViewModel CreateIndexViewModel(string viewTypeName, IEnumerable<Models.IdentityModel> models)
        {
            return ViewModelCreator != null ? ViewModelCreator.CreateIndexViewModel(viewTypeName, models, this) 
                                            : new ViewModelCreator().CreateIndexViewModel(viewTypeName, models, this);
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