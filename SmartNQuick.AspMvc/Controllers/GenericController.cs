//@BaseCode
//MdStart
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
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
            }
            AfterCreateController(result);
            return result;
        }
        partial void BeforeCreateController(ref Contracts.Client.IAdapterAccess<TContract> controller, ref bool handled);
        partial void AfterCreateController(Contracts.Client.IAdapterAccess<TContract> controller);

        protected bool FromCreateToEdit { get; set; } = true;
        protected bool FromEditToIndex { get; set; } = false;
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
        [HttpGet]
        [ActionName("Index")]
        public virtual async Task<IActionResult> IndexAsync()
        {
            using var ctrl = CreateController();
            var entities = await ctrl.GetAllAsync().ConfigureAwait(false);
            var models = entities.Select(e => ToModel(e));

            models = BeforeView(models, Action.Index);
            models = await BeforeViewAsync(models, Action.Index).ConfigureAwait(false);
            return View("Index", models);
        }

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
                    LastError = string.Empty;
                    model = await CreateModelAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LastError = ex.GetError();
                }
            }
            AfterCreate(model);
            if (HasError == false)
            {
                model = BeforeView(model, Action.Create);
                model = await BeforeViewAsync(model, Action.Edit).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : View("Create", model);
        }
        partial void BeforeCreate(ref TModel model, ref bool handled);
        partial void AfterCreate(TModel model);

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
                    LastError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastError = ex.GetError();
                }
            }
            AfterInsertModel(model);
            if (HasError == false)
            {
                model = BeforeView(model, Action.Create);
                model = await BeforeViewAsync(model, Action.Create).ConfigureAwait(false);
            }
            return HasError ? View("Create", model) : FromCreateToEdit ? RedirectToAction("Edit", new { model.Id }) : RedirectToAction("Index");
        }
        partial void BeforeInsertModel(TModel model, ref bool handled);
        partial void AfterInsertModel(TModel model);

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
                    LastError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastError = ex.GetError();
                }
            }
            AfterEdit(model);
            if (HasError == false)
            {
                model = BeforeView(model, Action.Edit);
                model = await BeforeViewAsync(model, Action.Edit).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : View("Edit", model);
        }
        partial void BeforeEdit(ref TModel model, ref bool handled);
        partial void AfterEdit(TModel model);

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
                    LastError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastError = ex.GetError();
                }
            }
            AfterUpdateModel(model);
            if (HasError == false)
            {
                model = BeforeView(model, Action.Edit);
                model = await BeforeViewAsync(model, Action.Edit).ConfigureAwait(false);
            }
            return HasError ? View("Edit", model) : FromEditToIndex ? RedirectToAction("Index") : RedirectToAction("Edit", new { model.Id });
        }
        partial void BeforeUpdateModel(TModel model, ref bool handled);
        partial void AfterUpdateModel(TModel model);

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
                    LastError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastError = ex.GetError();
                }
            }
            AfterDelete(model);
            if (HasError == false)
            {
                model = BeforeView(model, Action.Delete);
                model = await BeforeViewAsync(model, Action.Delete).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : View("Delete", model);
        }
        partial void BeforeDelete(ref TModel model, ref bool handled);
        partial void AfterDelete(TModel model);

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
                    LastError = string.Empty;
                }
                catch (Exception ex)
                {
                    LastError = ex.GetError();
                }
            }
            AfterDeleteModel(id);
            if (HasError)
            {
                using var ctrl = CreateController();
                var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

                model = ToModel(entity);
            }
            return HasError ? View("Delete", model) : RedirectToAction("Index");
        }
        partial void BeforeDeleteModel(int id, ref bool handled);
        partial void AfterDeleteModel(int id);
    }
}
//MdEnd
