//@BaseCode
//MdStart
using CommonBase.Extensions;
using SmartNQuick.AspMvc.Models.Modules.View;
using System;
using System.Collections.Generic;

namespace SmartNQuick.AspMvc.Modules.View
{
    public static partial class ViewModelCreator
    {
        public static IndexViewModel CreateIndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<Models.IdentityModel> models, Type modelType, Type displayType)
        {
            var handled = false;
            IndexViewModel result = null;

            BeforeCreateIndexViewModel(viewBagWrapper, models, modelType, displayType, ref result, ref handled);
            if (handled == false)
            {
                result = new IndexViewModel(viewBagWrapper, models, modelType, displayType);
            }
            AfterCreateIndexViewModel(result);
            return result;
        }
        static partial void BeforeCreateIndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<Models.IdentityModel> models, Type modelType, Type displayType, ref IndexViewModel result, ref bool handled);
        static partial void AfterCreateIndexViewModel(IndexViewModel result);

        public static DisplayViewModel CreateDisplayViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model)
        {
            viewBagWrapper.CheckArgument(nameof(viewBagWrapper));
            model.CheckArgument(nameof(model));

            var modelType = model.GetType();

            return CreateDisplayViewModel(viewBagWrapper, model, modelType, modelType); 
        }
        public static DisplayViewModel CreateDisplayViewModel( ViewBagWrapper viewBagWrapper, Models.IdentityModel model, Type modelType, Type displayType)
        {
            var handled = false;
            DisplayViewModel result = null;

            BeforeCreateDisplayViewModel(viewBagWrapper, model, modelType, displayType, ref result, ref handled);
            if (handled == false)
            {
                result = new DisplayViewModel(viewBagWrapper, model, modelType, displayType);
            }
            AfterCreateDisplayViewModel(result);
            return result;
        }
        static partial void BeforeCreateDisplayViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model, Type modelType, Type displayType, ref DisplayViewModel result, ref bool handled);
        static partial void AfterCreateDisplayViewModel(DisplayViewModel result);

        public static EditViewModel CreateEditViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model)
        {
            viewBagWrapper.CheckArgument(nameof(viewBagWrapper));
            model.CheckArgument(nameof(model));

            var modelType = model.GetType();

            return CreateEditViewModel(viewBagWrapper, model, modelType, modelType);
        }
        public static EditViewModel CreateEditViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model, Type modelType, Type displayType)
        {
            var handled = false;
            EditViewModel result = null;

            BeforeCreateEditViewModel(viewBagWrapper, model, modelType, displayType, ref result, ref handled);
            if (handled == false)
            {
                result = new EditViewModel(viewBagWrapper, model, modelType, displayType);
            }
            AfterCreateEditViewModel(result);
            return result;
        }
        static partial void BeforeCreateEditViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model, Type modelType, Type displayType, ref EditViewModel result, ref bool handled);
        static partial void AfterCreateEditViewModel(EditViewModel result);
    }
}
//MdEnd