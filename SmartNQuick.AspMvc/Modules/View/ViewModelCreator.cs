//@BaseCode
//MdStart
using SmartNQuick.AspMvc.Models.Modules.View;
using System.Collections.Generic;

namespace SmartNQuick.AspMvc.Modules.View
{
    public partial class ViewModelCreator
    {
        public virtual IndexViewModel CreateIndexViewModel(string viewName, IEnumerable<Models.IdentityModel> models, dynamic viewBag)
        {
            var handled = false;
            var viewBagWrapper = new ViewBagWrapper(viewBag);
            IndexViewModel result = null;

            BeforeCreateIndexViewModel(viewName, models, viewBagWrapper, ref result, ref handled);
            if (handled == false)
            {
                result = new IndexViewModel(models, viewBagWrapper.HiddenNames, viewBagWrapper.IgnoreNames, viewBagWrapper.DisplayNames);
            }
            AfterCreateIndexViewModel(viewName, models, viewBagWrapper, result);
            return result;
        }
        partial void BeforeCreateIndexViewModel(string viewName, IEnumerable<Models.IdentityModel> models, ViewBagWrapper viewBagWrapper, ref IndexViewModel result, ref bool handled);
        partial void AfterCreateIndexViewModel(string viewName, IEnumerable<Models.IdentityModel> models, ViewBagWrapper viewBagWrapper, IndexViewModel result);

        public virtual DisplayViewModel CreateDisplayViewModel(string viewName, Models.IdentityModel model, dynamic viewBag)
        {
            var handled = false;
            var viewBagWrapper = new ViewBagWrapper(viewBag);
            DisplayViewModel result = null;

            BeforeCreateDisplayViewModel(viewName, model, viewBagWrapper, ref result, ref handled);
            if (handled == false)
            {
                result = new DisplayViewModel(model, viewBagWrapper.HiddenNames, viewBagWrapper.IgnoreNames, viewBagWrapper.DisplayNames);
            }
            AfterCreateDisplayViewModel(viewName, model, viewBagWrapper, result);
            return result;
        }
        partial void BeforeCreateDisplayViewModel(string viewName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper, ref DisplayViewModel result, ref bool handled);
        partial void AfterCreateDisplayViewModel(string viewName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper, DisplayViewModel result);

        public virtual EditViewModel CreateEditViewModel(string viewName, Models.IdentityModel model, dynamic viewBag)
        {
            var handled = false;
            var viewBagWrapper = new ViewBagWrapper(viewBag);
            EditViewModel result = null;

            BeforeCreateEditViewModel(viewName, model, viewBagWrapper, ref result, ref handled);
            if (handled == false)
            {
                result = new EditViewModel(model, viewBagWrapper.HiddenNames, viewBagWrapper.IgnoreNames, viewBagWrapper.DisplayNames);
            }
            AfterCreateEditViewModel(viewName, model, viewBagWrapper, result);
            return result;
        }
        partial void BeforeCreateEditViewModel(string viewName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper, ref EditViewModel result, ref bool handled);
        partial void AfterCreateEditViewModel(string viewName, Models.IdentityModel model, ViewBagWrapper viewBagWrapper, EditViewModel result);
    }
}
//MdEnd