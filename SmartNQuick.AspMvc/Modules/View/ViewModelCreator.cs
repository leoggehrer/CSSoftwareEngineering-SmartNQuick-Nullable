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
        public static IndexViewModel CreateIndexViewModel(ViewBagWrapper vieBagInfo, IEnumerable<Models.IdentityModel> models, Type modelType, Type displayType)
        {
            var handled = false;
            IndexViewModel result = null;

            BeforeCreateIndexViewModel(vieBagInfo, models, modelType, displayType, ref result, ref handled);
            if (handled == false)
            {
                result = new IndexViewModel(vieBagInfo, models, modelType, displayType);
            }
            AfterCreateIndexViewModel(result);
            return result;
        }
        static partial void BeforeCreateIndexViewModel(ViewBagWrapper viewBagInfo, IEnumerable<Models.IdentityModel> models, Type modelType, Type displayType, ref IndexViewModel result, ref bool handled);
        static partial void AfterCreateIndexViewModel(IndexViewModel result);

        public static DisplayViewModel CreateDisplayViewModel(ViewBagWrapper viewBagInfo, Models.ModelObject model)
        {
            viewBagInfo.CheckArgument(nameof(viewBagInfo));
            model.CheckArgument(nameof(model));

            var modelType = model.GetType();

            return CreateDisplayViewModel(viewBagInfo, model, modelType, modelType); 
        }
        public static DisplayViewModel CreateDisplayViewModel(ViewBagWrapper viewBagInfo, Models.ModelObject model, Type modelType, Type displayType)
        {
            var handled = false;
            DisplayViewModel result = null;

            BeforeCreateDisplayViewModel(viewBagInfo, model, modelType, displayType, ref result, ref handled);
            if (handled == false)
            {
                result = new DisplayViewModel(viewBagInfo, model, modelType, displayType);
            }
            AfterCreateDisplayViewModel(result);
            return result;
        }
        static partial void BeforeCreateDisplayViewModel(ViewBagWrapper viewBagInfo, Models.ModelObject model, Type modelType, Type displayType, ref DisplayViewModel result, ref bool handled);
        static partial void AfterCreateDisplayViewModel(DisplayViewModel result);

        public static EditViewModel CreateEditViewModel(ViewBagWrapper viewBagInfo, Models.IdentityModel model)
        {
            viewBagInfo.CheckArgument(nameof(viewBagInfo));
            model.CheckArgument(nameof(model));

            var modelType = model.GetType();

            return CreateEditViewModel(viewBagInfo, model, modelType, modelType);
        }
        public static EditViewModel CreateEditViewModel(ViewBagWrapper viewBagInfo, Models.IdentityModel model, Type modelType, Type displayType)
        {
            var handled = false;
            EditViewModel result = null;

            BeforeCreateEditViewModel(viewBagInfo, model, modelType, displayType, ref result, ref handled);
            if (handled == false)
            {
                result = new EditViewModel(viewBagInfo, model, modelType, displayType);
            }
            AfterCreateEditViewModel(result);
            return result;
        }
        static partial void BeforeCreateEditViewModel(ViewBagWrapper viewBagInfo, Models.IdentityModel model, Type modelType, Type displayType, ref EditViewModel result, ref bool handled);
        static partial void AfterCreateEditViewModel(EditViewModel result);
    }
}
//MdEnd