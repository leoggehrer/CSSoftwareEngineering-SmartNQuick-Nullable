//@BaseCode
//MdStart
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartNQuick.AspMvc.Models.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Controllers
{
    public abstract partial class GenericController<TContract, TModel> : MvcController
        where TContract : Contracts.IIdentifiable, Contracts.ICopyable<TContract>
        where TModel : TContract, new()
    {
        static GenericController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        internal GenericController()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        protected virtual Contracts.Client.IAdapterAccess<TContract> CreateController()
        {
            return CreateController<TContract>();
        }
        protected virtual Contracts.Client.IAdapterAccess<T> CreateController<T>()
            where T : Contracts.IIdentifiable, Contracts.ICopyable<T>
        {
            var handled = false;
            var result = default(Contracts.Client.IAdapterAccess<T>);

            BeforeCreateController(ref result, ref handled);
            if (handled == false)
            {
                result = Adapters.Factory.Create<T>();
#if ACCOUNT_ON
                result.SessionToken = SessionWrapper?.SessionToken;
#endif
            }
            AfterCreateController(result);
            return result;
        }
        partial void BeforeCreateController<T>(ref Contracts.Client.IAdapterAccess<T> controller, ref bool handled);
        partial void AfterCreateController<T>(Contracts.Client.IAdapterAccess<T> controller);

        protected bool FromCreateToEdit { get; set; } = false;
        protected bool FromEditToIndex { get; set; } = true;
        protected string ControllerName => GetType().Name.Replace("Controller", string.Empty);

        protected virtual TModel ToModel(TContract entity)
        {
            entity.CheckArgument(nameof(entity));

            var result = new TModel();

            result.CopyProperties(entity);
            return result;
        }
        protected virtual TModel BeforeView(TModel model, ActionMode action) => model;
        protected virtual IEnumerable<TModel> BeforeView(IEnumerable<TModel> models, ActionMode action) => models;
        protected virtual Task<TModel> BeforeViewAsync(TModel model, ActionMode action) => Task.FromResult(model);
        protected virtual Task<IEnumerable<TModel>> BeforeViewAsync(IEnumerable<TModel> models, ActionMode action) => Task.FromResult(models);

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.ViewModelCreator = new Modules.View.ViewModelCreator();

            base.OnActionExecuted(context);
        }

        protected virtual async Task<TModel> GetModelAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeGetModel(ref model, ref handled);
            if (handled == false)
            {
                using var ctrl = CreateController();
                var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

                model = ToModel(entity);
            }
            AfterGetModel(model);
            return model;
        }
        partial void BeforeGetModel(ref TModel model, ref bool handled);
        partial void AfterGetModel(TModel model);

        [HttpGet]
        [ActionName("Index")]
        public virtual async Task<IActionResult> IndexAsync()
        {
            var handled = false;
            var models = default(IEnumerable<TModel>);

            BeforeIndex(ref models, ref handled);
            if (handled == false)
            {
                try
                {
                    using var ctrl = CreateController();
                    var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

                    models = entities.Select(e => ToModel(e));
                    models = BeforeView(models, ActionMode.Index);
                    models = await BeforeViewAsync(models, ActionMode.Index).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterIndex(models);
            return ReturnIndexView(models);
        }
        partial void BeforeIndex(ref IEnumerable<TModel> models, ref bool handled);
        partial void AfterIndex(IEnumerable<TModel> models);
        protected virtual IActionResult ReturnIndexView(IEnumerable<TModel> models) => View("Index", models);

        [HttpGet]
        [ActionName("Create")]
        public virtual async Task<IActionResult> CreateAsync()
        {
            var handled = false;
            var model = default(TModel);

            BeforeCreate(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    LastViewError = string.Empty;
                    model = await CreateModelAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterCreate(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.Create);
                model = await BeforeViewAsync(model, ActionMode.Create).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnCreateView(model);
        }
        partial void BeforeCreate(ref TModel model, ref bool handled);
        partial void AfterCreate(TModel model);
        protected virtual IActionResult ReturnCreateView(TModel model) => View("Create", model);

        protected virtual async Task<TModel> CreateModelAsync()
        {
            var handled = false;
            var model = default(TModel);

            BeforeCreateModel(ref model, ref handled);
            if (handled == false)
            {
                using var ctrl = CreateController();
                var entity = await ctrl.CreateAsync().ConfigureAwait(false);

                model = ToModel(entity);
            }
            AfterCreateModel(model);
            return model;
        }
        partial void BeforeCreateModel(ref TModel model, ref bool handled);
        partial void AfterCreateModel(TModel model);


        [HttpPost]
        [ActionName("Create")]
        public virtual async Task<IActionResult> InsertAsync(TModel model)
        {
            var handled = false;

            BeforeInsertModel(model, ref handled);
            if (handled == false)
            {
                try
                {
                    using var ctrl = CreateController();
                    var entity = await ctrl.InsertAsync(model).ConfigureAwait(false);

                    model.CopyProperties(entity);
                    LastViewError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterInsertModel(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.Create);
                model = await BeforeViewAsync(model, ActionMode.Create).ConfigureAwait(false);
            }
            return ReturnAfterCreate(HasError, model);
        }
        partial void BeforeInsertModel(TModel model, ref bool handled);
        partial void AfterInsertModel(TModel model);
        protected virtual IActionResult ReturnAfterCreate(bool hasError, TModel model) => hasError ? View("Create", model) : FromCreateToEdit ? RedirectToAction("Edit", new { model.Id }) : RedirectToAction("Index");

        [HttpGet]
        [ActionName("CreateDetail")]
        public virtual async Task<IActionResult> CreateDetailAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeCreateDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    LastViewError = string.Empty;
                    model = await GetModelAsync(id).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterCreateDetail(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.CreateDetail);
                model = await BeforeViewAsync(model, ActionMode.CreateDetail).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnCreateDetailView(model);
        }
        partial void BeforeCreateDetail(ref TModel model, ref bool handled);
        partial void AfterCreateDetail(TModel model);
        protected virtual IActionResult ReturnCreateDetailView(TModel model) => View("CreateDetail", model);

        [HttpPost]
        [ActionName("CreateDetail")]
        public virtual async Task<IActionResult> AddDetailAsync(int id, IFormCollection formCollection)
        {
            var handled = false;
            var model = default(TModel);

            BeforeAddDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    model = await GetModelAsync(id).ConfigureAwait(false);
                    if (model is Models.IMasterDetails mds)
                    {
                        var detail = mds.CreateDetail();

                        SetModelValues(detail, nameof(ActionMode.CreateDetail), formCollection);
                        mds.AddDetail(detail);

                        using var ctrl = CreateController();
                        var entity = await ctrl.UpdateAsync(model).ConfigureAwait(false);

                        model.CopyProperties(entity);
                    }
                    LastViewError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterAddDetail(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.CreateDetail);
                model = await BeforeViewAsync(model, ActionMode.CreateDetail).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnCreateDetailView(model);
        }
        partial void BeforeAddDetail(ref TModel model, ref bool handled);
        partial void AfterAddDetail(TModel model);

        [HttpGet]
        [ActionName("Edit")]
        public virtual async Task<IActionResult> EditAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeEdit(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    model = await EditModelAsync(id).ConfigureAwait(false);
                    LastViewError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterEdit(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.Edit);
                model = await BeforeViewAsync(model, ActionMode.Edit).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnEditView(model);
        }
        partial void BeforeEdit(ref TModel model, ref bool handled);
        partial void AfterEdit(TModel model);
        protected virtual IActionResult ReturnEditView(TModel model) => View("Edit", model);

        protected virtual async Task<TModel> EditModelAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeEditModel(ref model, ref handled);
            if (handled == false)
            {
                using var ctrl = CreateController();
                var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

                model = ToModel(entity);
            }
            AfterEditModel(model);
            return model;
        }
        partial void BeforeEditModel(ref TModel model, ref bool handled);
        partial void AfterEditModel(TModel model);

        [HttpPost]
        [ActionName("Edit")]
        public virtual async Task<IActionResult> Update(TModel model)
        {
            var handled = false;

            BeforeUpdateModel(model, ref handled);
            if (handled == false)
            {
                try
                {
                    using var ctrl = CreateController();
                    var entity = await ctrl.UpdateAsync(model).ConfigureAwait(false);

                    model.CopyProperties(entity);
                    LastViewError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterUpdateModel(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.Edit);
                model = await BeforeViewAsync(model, ActionMode.Edit).ConfigureAwait(false);
            }
            return ReturnAfterEdit(HasError, model);
        }
        partial void BeforeUpdateModel(TModel model, ref bool handled);
        partial void AfterUpdateModel(TModel model);
        protected virtual IActionResult ReturnAfterEdit(bool hasError, TModel model) => hasError ? View("Edit", model) : FromEditToIndex ? RedirectToAction("Index") : RedirectToAction("Edit", new { model.Id });

        [HttpGet]
        [ActionName("Delete")]
        public virtual async Task<IActionResult> ViewDeleteAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeDelete(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    using var ctrl = CreateController();
                    var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

                    model = ToModel(entity);
                    LastViewError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterDelete(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.Delete);
                model = await BeforeViewAsync(model, ActionMode.Delete).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnDeleteView(model);
        }
        partial void BeforeDelete(ref TModel model, ref bool handled);
        partial void AfterDelete(TModel model);
        protected virtual IActionResult ReturnDeleteView(TModel model) => View("Delete", model);

        [ActionName("Delete")]
        public virtual async Task<IActionResult> DeleteAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeDeleteModel(id, ref handled);
            if (handled == false)
            {
                try
                {
                    using var ctrl = CreateController();

                    await ctrl.DeleteAsync(id).ConfigureAwait(false);
                    LastViewError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterDeleteModel(id);
            if (HasError)
            {
                using var ctrl = CreateController();
                var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

                model = ToModel(entity);
            }
            return ReturnAfterDelete(HasError, model);
        }
        partial void BeforeDeleteModel(int id, ref bool handled);
        partial void AfterDeleteModel(int id);
        protected virtual IActionResult ReturnAfterDelete(bool hasError, TModel model) => hasError ? View("Delete", model) : RedirectToAction("Index");

        [HttpGet]
        [ActionName("Details")]
        public virtual async Task<IActionResult> DetailsAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeDetails(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    model = await GetModelAsync(id).ConfigureAwait(false);
                    LastViewError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterEdit(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.Details);
                model = await BeforeViewAsync(model, ActionMode.Details).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnDetailsView(model);
        }
        partial void BeforeDetails(ref TModel model, ref bool handled);
        partial void AfterDetails(TModel model);
        protected virtual IActionResult ReturnDetailsView(TModel model) => View("Details", model);

        protected static void SetModelValues(object model, string prefix, IFormCollection formCollection)
        {
            model.CheckArgument(nameof(model));
            formCollection.CheckArgument(nameof(formCollection));

            foreach (var pi in model.GetType().GetProperties().Where(pi => pi.CanWrite))
            {
                string key = $"{prefix}.{pi.Name}";

                if (formCollection.Keys.Contains(key))
                {
                    if (pi.PropertyType.IsEnum)
                    {
                        if (Int32.TryParse(formCollection[key].FirstOrDefault(), out int enumVal))
                        {
                            object value = Enum.Parse(pi.PropertyType, enumVal.ToString());

                            pi.SetValue(model, value);
                        }
                    }
                    else
                    {
                        object value = Convert.ChangeType(formCollection[key].FirstOrDefault(), pi.PropertyType);

                        pi.SetValue(model, value);
                    }
                }
            }
        }
    }
}
//MdEnd
