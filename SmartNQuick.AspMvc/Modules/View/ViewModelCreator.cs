//@BaseCode
//MdStart
using SmartNQuick.AspMvc.Models.Modules.View;
using System;
using System.Collections.Generic;

namespace SmartNQuick.AspMvc.Modules.View
{
    public partial class ViewModelCreator
    {
        public virtual IndexViewModel CreateIndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<Models.IdentityModel> models)
        {
            var handled = false;
            IndexViewModel result = null;

            BeforeCreateIndexViewModel(viewBagWrapper, models, ref result, ref handled);
            if (handled == false)
            {
                result = new IndexViewModel(viewBagWrapper, models);
            }
            AfterCreateIndexViewModel(viewBagWrapper, models, result);
            return result;
        }
        partial void BeforeCreateIndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<Models.IdentityModel> models, ref IndexViewModel result, ref bool handled);
        partial void AfterCreateIndexViewModel(ViewBagWrapper viewBagWrapper, IEnumerable<Models.IdentityModel> models, IndexViewModel result);

        public virtual DisplayViewModel CreateDisplayViewModel( ViewBagWrapper viewBagWrapper, Models.IdentityModel model)
        {
            var handled = false;
            DisplayViewModel result = null;

            BeforeCreateDisplayViewModel(viewBagWrapper, model, ref result, ref handled);
            if (handled == false)
            {
                result = new DisplayViewModel(viewBagWrapper, model);
            }
            AfterCreateDisplayViewModel(viewBagWrapper, model, result);
            return result;
        }
        partial void BeforeCreateDisplayViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model, ref DisplayViewModel result, ref bool handled);
        partial void AfterCreateDisplayViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model, DisplayViewModel result);

        public virtual EditViewModel CreateEditViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model)
        {
            var handled = false;
            EditViewModel result = null;

            BeforeCreateEditViewModel(viewBagWrapper, model, ref result, ref handled);
            if (handled == false)
            {
                result = new EditViewModel(viewBagWrapper, model);
            }
            AfterCreateEditViewModel(viewBagWrapper, model, result);
            return result;
        }
        partial void BeforeCreateEditViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model, ref EditViewModel result, ref bool handled);
        partial void AfterCreateEditViewModel(ViewBagWrapper viewBagWrapper, Models.IdentityModel model, EditViewModel result);
    }
}
//MdEnd