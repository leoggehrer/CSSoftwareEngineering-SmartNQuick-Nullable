//@BaseCode
//MdStart
using SmartNQuick.AspMvc.Models.Modules.View;
using System.Collections.Generic;

namespace SmartNQuick.AspMvc.Modules.View
{
    public partial class ViewModelCreator
    {
        public virtual IndexViewModel CreateIndexViewModel(string viewTypeName, IEnumerable<Models.IdentityModel> models, ViewBagWrapper viewBagWrapper)
        {
            var handled = false;
            IndexViewModel result = null;

            BeforeCreateIndexViewModel(viewTypeName, models, viewBagWrapper, ref result, ref handled);
            if (handled == false)
            {
                result = new IndexViewModel(models, viewBagWrapper.HiddenNames, viewBagWrapper.IgnoreNames, viewBagWrapper.DisplayNames);
            }
            AfterCreateIndexViewModel(viewTypeName, models, viewBagWrapper, result);
            return result;
        }
        partial void BeforeCreateIndexViewModel(string viewTypeName, IEnumerable<Models.IdentityModel> models, ViewBagWrapper viewBagWrapper, ref IndexViewModel result, ref bool handled);
        partial void AfterCreateIndexViewModel(string viewTypeName, IEnumerable<Models.IdentityModel> models, ViewBagWrapper viewBagWrapper, IndexViewModel result);

        public virtual DisplayViewModel CreateDisplayViewModel(string viewTypeName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper)
        {
            var handled = false;
            DisplayViewModel result = null;

            BeforeCreateDisplayViewModel(viewTypeName, model, viewBagWrapper, ref result, ref handled);
            if (handled == false)
            {
                result = new DisplayViewModel(model, viewBagWrapper.HiddenNames, viewBagWrapper.IgnoreNames, viewBagWrapper.DisplayNames);
            }
            AfterCreateDisplayViewModel(viewTypeName, model, viewBagWrapper, result);
            return result;
        }
        partial void BeforeCreateDisplayViewModel(string viewTypeName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper, ref DisplayViewModel result, ref bool handled);
        partial void AfterCreateDisplayViewModel(string viewTypeName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper, DisplayViewModel result);

        public virtual EditViewModel CreateEditViewModel(string viewTypeName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper)
        {
            var handled = false;
            EditViewModel result = null;

            BeforeCreateEditViewModel(viewTypeName, model, viewBagWrapper, ref result, ref handled);
            if (handled == false)
            {
                result = new EditViewModel(model, viewBagWrapper.HiddenNames, viewBagWrapper.IgnoreNames, viewBagWrapper.DisplayNames);
            }
            AfterCreateEditViewModel(viewTypeName, model, viewBagWrapper, result);
            return result;
        }
        partial void BeforeCreateEditViewModel(string viewTypeName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper, ref EditViewModel result, ref bool handled);
        partial void AfterCreateEditViewModel(string viewTypeName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper, EditViewModel result);
    }
}
//MdEnd