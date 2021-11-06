//@BaseCode
//MdStart
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        protected enum Action
        {
            Index,
            Create,
            Edit,
            Delete,
        }

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
            var handled = false;
            var result = default(Contracts.Client.IAdapterAccess<TContract>);

            BeforeCreateController(ref result, ref handled);
            if (handled == false)
            {
                result = Adapters.Factory.Create<TContract>();
#if ACCOUNT_ON
                result.SessionToken = SessionWrapper?.SessionToken;
#endif
            }
            AfterCreateController(result);
            return result;
        }
        partial void BeforeCreateController(ref Contracts.Client.IAdapterAccess<TContract> controller, ref bool handled);
        partial void AfterCreateController(Contracts.Client.IAdapterAccess<TContract> controller);

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
        protected virtual TModel BeforeView(TModel model, Action action) => model;
        protected virtual IEnumerable<TModel> BeforeView(IEnumerable<TModel> models, Action action) => models;
        protected virtual Task<TModel> BeforeViewAsync(TModel model, Action action) => Task.FromResult(model);
        protected virtual Task<IEnumerable<TModel>> BeforeViewAsync(IEnumerable<TModel> models, Action action) => Task.FromResult(models);

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.ViewModelCreator = new Modules.View.ViewModelCreator();

            base.OnActionExecuted(context);
        }
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
                    models = BeforeView(models, Action.Index);
                    models = await BeforeViewAsync(models, Action.Index).ConfigureAwait(false);
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
                model = BeforeView(model, Action.Create);
                model = await BeforeViewAsync(model, Action.Edit).ConfigureAwait(false);
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
                model = BeforeView(model, Action.Create);
                model = await BeforeViewAsync(model, Action.Create).ConfigureAwait(false);
            }
            return ReturnAfterCreate(HasError, model);
        }
        partial void BeforeInsertModel(TModel model, ref bool handled);
        partial void AfterInsertModel(TModel model);
        protected virtual IActionResult ReturnAfterCreate(bool hasError, TModel model) => hasError ? View("Create", model) : FromCreateToEdit ? RedirectToAction("Edit", new { model.Id }) : RedirectToAction("Index");

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
                model = BeforeView(model, Action.Edit);
                model = await BeforeViewAsync(model, Action.Edit).ConfigureAwait(false);
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
                model = BeforeView(model, Action.Edit);
                model = await BeforeViewAsync(model, Action.Edit).ConfigureAwait(false);
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
                model = BeforeView(model, Action.Delete);
                model = await BeforeViewAsync(model, Action.Delete).ConfigureAwait(false);
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
    }
}
//MdEnd
