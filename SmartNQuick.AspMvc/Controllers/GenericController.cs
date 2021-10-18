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

        protected Contracts.Client.IAdapterAccess<TContract> CreateController()
        {
            var handled = false;
            var result = default(Contracts.Client.IAdapterAccess<TContract>);

            BeforeCreateController(ref result, ref handled);
            if (handled == false)
            {
                result = Adapters.Factory.Create<TContract>();
            }
            AfterCreateController(result);
            return Adapters.Factory.Create<TContract>();
        }
        partial void BeforeCreateController(ref Contracts.Client.IAdapterAccess<TContract> controller, ref bool handled);
        partial void AfterCreateController(Contracts.Client.IAdapterAccess<TContract> controller);

        internal GenericController()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

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
            var model = await CreateModelAsync().ConfigureAwait(false);

            model = BeforeView(model, Action.Create);
            model = await BeforeViewAsync(model, Action.Create).ConfigureAwait(false);
            return View("Create", model);
        }
        protected virtual async Task<TModel> CreateModelAsync()
        {
            using var ctrl = CreateController();
            var entity = await ctrl.CreateAsync().ConfigureAwait(false);

            return ToModel(entity);
        }

        [HttpPost]
        [ActionName("Create")]
        public virtual async Task<IActionResult> InsertAsync(TModel model)
        {
            var handled = false;

            BeforeInsertEntity(model, ref handled);
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
            AfterInsertEntity(model);
            if (HasError)
            {
                model = BeforeView(model, Action.Create);
                model = await BeforeViewAsync(model, Action.Create).ConfigureAwait(false);
            }
            return HasError ? View("Create", model) : FromCreateToEdit ? RedirectToAction("Edit", new { model.Id }) : RedirectToAction("Index");
        }
        partial void BeforeInsertEntity(TModel model, ref bool handled);
        partial void AfterInsertEntity(TModel model);

        [HttpGet]
        [ActionName("Edit")]
        public virtual async Task<IActionResult> EditAsync(int id)
        {
            var model = await EditModelAsync(id).ConfigureAwait(false);

            model = BeforeView(model, Action.Edit);
            model = await BeforeViewAsync(model, Action.Edit).ConfigureAwait(false);
            return View("Edit", model);
        }
        protected virtual async Task<TModel> EditModelAsync(int id)
        {
            using var ctrl = CreateController();
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

            return ToModel(entity);
        }

        [HttpPost]
        [ActionName("Edit")]
        public virtual async Task<IActionResult> Update(TModel model)
        {
            var handled = false;

            BeforeUpdateEntity(model, ref handled);
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
            AfterUpdateEntity(model);
            if (HasError)
            {
                model = BeforeView(model, Action.Edit);
                model = await BeforeViewAsync(model, Action.Edit).ConfigureAwait(false);
            }
            return HasError ? View("Edit", model) : FromEditToIndex ? RedirectToAction("Index") : RedirectToAction("Edit", new { model.Id });
        }
        partial void BeforeUpdateEntity(TModel model, ref bool handled);
        partial void AfterUpdateEntity(TModel model);

        [HttpGet]
        [ActionName("Delete")]
        public virtual async Task<IActionResult> ViewDeleteAsync(int id)
        {
            using var ctrl = CreateController();
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
            var model = ToModel(entity);

            model = BeforeView(model, Action.Delete);
            model = await BeforeViewAsync(model, Action.Delete).ConfigureAwait(false);
            return View("Delete", model);
        }
        [ActionName("Delete")]
        public virtual async Task<IActionResult> DeleteAsync(int id)
        {
            var handled = false;

            BeforeDeleteEntity(id, ref handled);
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
            AfterDeleteEntity(id);
            return HasError ? RedirectToAction("Delete", new { id }) : RedirectToAction("Index");
        }
        partial void BeforeDeleteEntity(int id, ref bool handled);
        partial void AfterDeleteEntity(int id);
    }
}
//MdEnd
