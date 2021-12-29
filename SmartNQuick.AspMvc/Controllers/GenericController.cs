//@BaseCode
//MdStart
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartNQuick.AspMvc.Models;
using SmartNQuick.AspMvc.Models.Modules.Common;
using SmartNQuick.AspMvc.Models.Modules.View;
using SmartNQuick.AspMvc.Modules.View;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Controllers
{
    public abstract partial class GenericController<TContract, TModel> : MvcController
        where TContract : Contracts.IIdentifiable, Contracts.ICopyable<TContract>
        where TModel : IdentityModel, TContract, new()
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
                result.SessionToken = SessionInfo?.SessionToken;
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

        #region Before view
        protected virtual void BeforeView()
        {
        }
        protected virtual Task BeforeViewAsync() => Task.FromResult(0);
        protected virtual TModel BeforeView(TModel model, ActionMode action)
        {
            BeforeView();
            return model;
        }
        protected virtual IEnumerable<TModel> BeforeView(IEnumerable<TModel> models, ActionMode action)
        {
            BeforeView();
            return models;
        }

        protected virtual Task<TModel> BeforeViewAsync(TModel model, ActionMode action)
        {
            BeforeViewAsync();
            return Task.FromResult(model);
        }
        protected virtual Task<IEnumerable<TModel>> BeforeViewAsync(IEnumerable<TModel> models, ActionMode action)
        {
            BeforeViewAsync();
            return Task.FromResult(models);
        }

        protected virtual MasterDetailModel BeforeViewMasterDetail(MasterDetailModel model, ActionMode action)
        {
            BeforeView();
            return model;
        }
        protected virtual Task<MasterDetailModel> BeforeViewMasterDetailAsync(MasterDetailModel model, ActionMode action)
        {
            BeforeView();
            return Task.FromResult(model);
        }
        #endregion Before view

        protected virtual TModel ToModel(TContract entity)
        {
            entity.CheckArgument(nameof(entity));

            var result = new TModel();

            result.CopyProperties(entity);
            return result;
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

        protected virtual FilterModel CreateFilterModel()
        {
            var models = new TModel[] { new TModel() };
            var indexViewModel = CreateIndexViewModel(models);

            return new FilterModel(SessionInfo, indexViewModel);
        }
        protected virtual SorterModel CreateSorterModel()
        {
            var models = new TModel[] { new TModel() };
            var indexViewModel = CreateIndexViewModel(models);

            return new SorterModel(SessionInfo, indexViewModel);
        }

        protected virtual IndexViewModel CreateIndexViewModel(IEnumerable<TModel> models)
        {
            var modelType = typeof(TModel);
            var displayType = ViewBagWrapper.GetDisplayType(modelType);
            var viewBageInfo = new ViewBagWrapper(ViewBag);
            var viewModels = models ?? Array.Empty<TModel>();

            return ViewModelCreator.CreateIndexViewModel(viewBageInfo, viewModels, modelType, displayType);
        }
        protected virtual EditViewModel CreateEditViewModel(TModel model)
        {
            var modelType = typeof(TModel);
            var displayType = modelType;
            var viewBagInfo = new ViewBagWrapper(ViewBag);

            return ViewModelCreator.CreateEditViewModel(viewBagInfo, model, modelType, displayType);
        }
        protected virtual DisplayViewModel CreateDisplayViewModel(TModel model)
        {
            var modelType = typeof(TModel);
            var displayType = modelType;
            var viewBagInfo = new ViewBagWrapper(ViewBag);

            return ViewModelCreator.CreateDisplayViewModel(viewBagInfo, model, modelType, displayType);
        }

        protected virtual void SetSessionPageData(int pageCount, int pageIndex, int pageSize)
        {
            pageCount = pageCount < 0 ? 0 : pageCount;
            pageSize = pageSize < 1 ? 1 : pageSize;
            pageIndex = pageIndex < 0 || pageIndex * pageSize >= pageCount ? 0 : pageIndex;

            SessionInfo.SetPageCount(ControllerName, pageCount);
            SessionInfo.SetPageIndex(ControllerName, pageIndex);
            SessionInfo.SetPageSize(ControllerName, pageSize);
        }
        protected virtual void SetSessionFilterValues(FilterValues filterValues)
        {
            SessionInfo.SetFilterValues(ControllerName, filterValues);
        }
        protected virtual void SetSessionSorterValues(SorterValues sorterValues)
        {
            SessionInfo.SetSorterValues(ControllerName, sorterValues);
        }

        protected virtual async Task<IEnumerable<TContract>> QueryByFilterAndSortAsync()
        {
            IEnumerable<TContract> result;
            var filterValues = SessionInfo.GetFilterValues(ControllerName);
            var predicate = filterValues?.CreatePredicate();
            var sorterValues = SessionInfo.GetSorterValues(ControllerName);
            var orderBy = sorterValues?.CreateOrderBy();
            using var ctrl = CreateController();

            if (predicate.HasContent() && orderBy.HasContent())
            {
                result = await ctrl.QueryAllAsync(predicate, orderBy).ConfigureAwait(false);
            }
            else if (predicate.HasContent())
            {
                result = await ctrl.QueryAllAsync(predicate).ConfigureAwait(false);
            }
            else if (orderBy.HasContent())
            {
                result = await ctrl.GetAllAsync(orderBy).ConfigureAwait(false);
            }
            else
            {
                result = await ctrl.GetAllAsync().ConfigureAwait(false);
            }
            return result;
        }
        protected virtual async Task<IEnumerable<TContract>> QueryPageListAsync(int pageIndex, int pageSize)
        {
            IEnumerable<TContract> result;
            var pageCount = 0;
            var filterValues = SessionInfo.GetFilterValues(ControllerName);
            var predicate = filterValues?.CreatePredicate();
            var sorterValues = SessionInfo.GetSorterValues(ControllerName);
            var orderBy = sorterValues?.CreateOrderBy();
            using var ctrl = CreateController();

            SetSessionPageData(pageCount, pageIndex, pageSize);
            if (predicate.HasContent() && orderBy.HasContent())
            {
                pageCount = await ctrl.CountByAsync(predicate).ConfigureAwait(false);

                SetSessionPageData(pageCount, pageIndex, pageSize);
                result = await ctrl.QueryPageListAsync(predicate, orderBy, pageIndex, pageSize).ConfigureAwait(false);
            }
            else if (predicate.HasContent())
            {
                pageCount = await ctrl.CountByAsync(predicate).ConfigureAwait(false);

                SetSessionPageData(pageCount, pageIndex, pageSize);
                result = await ctrl.QueryPageListAsync(predicate, pageIndex, pageSize).ConfigureAwait(false);
            }
            else if (orderBy.HasContent())
            {
                pageCount = await ctrl.CountAsync().ConfigureAwait(false);

                SetSessionPageData(pageCount, pageIndex, pageSize);
                result = await ctrl.GetPageListAsync(orderBy, pageIndex, pageSize).ConfigureAwait(false);
            }
            else
            {
                pageCount = await ctrl.CountAsync().ConfigureAwait(false);

                SetSessionPageData(pageCount, pageIndex, pageSize);
                result = await ctrl.GetPageListAsync(pageIndex, pageSize).ConfigureAwait(false);
            }
            return result;
        }

        #region Index actions
        [HttpPost]
        [ActionName(nameof(ActionMode.Search))]
        public virtual async Task<IActionResult> SearchAsync(IFormCollection formCollection)
        {
            var handled = false;
            var models = default(IEnumerable<TModel>);

            BeforeIndex(ref models, ref handled);
            if (handled == false)
            {
                try
                {
                    var pageIndex = 0;
                    var filterModel = CreateFilterModel();
                    var filterValues = filterModel.GetFilterValues(formCollection);
                    var pageSize = SessionInfo.GetPageSize(ControllerName);

                    SetSessionFilterValues(filterValues);

                    var entities = await QueryPageListAsync(pageIndex, pageSize).ConfigureAwait(false);

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

        [HttpPost]
        [ActionName(nameof(ActionMode.Filter))]
        public virtual async Task<IActionResult> FilterAsync(IFormCollection formCollection)
        {
            var handled = false;
            var models = default(IEnumerable<TModel>);

            BeforeIndex(ref models, ref handled);
            if (handled == false)
            {
                try
                {
                    var pageIndex = 0;
                    var filterModel = CreateFilterModel();
                    var filterValues = filterModel.GetFilterValues(formCollection);
                    var pageSize = SessionInfo.GetPageSize(ControllerName);

                    SetSessionFilterValues(filterValues);

                    var entities = await QueryPageListAsync(pageIndex, pageSize).ConfigureAwait(false);

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

        [HttpPost]
        [ActionName(nameof(ActionMode.Sorter))]
        public virtual async Task<IActionResult> SorterAsync(IFormCollection formCollection)
        {
            var handled = false;
            var models = default(IEnumerable<TModel>);

            BeforeIndex(ref models, ref handled);
            if (handled == false)
            {
                try
                {
                    var pageIndex = 0;
                    var sorterModel = CreateSorterModel();
                    var sorterValues = sorterModel.GetSorterValues(formCollection);
                    var pageSize = SessionInfo.GetPageSize(ControllerName);

                    SetSessionSorterValues(sorterValues);

                    var entities = await QueryPageListAsync(pageIndex, pageSize).ConfigureAwait(false);

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

        [HttpGet]
        [ActionName(nameof(ActionMode.Index))]
        public virtual async Task<IActionResult> IndexAsync()
        {
            var handled = false;
            var models = default(IEnumerable<TModel>);

            BeforeIndex(ref models, ref handled);
            if (handled == false)
            {
                try
                {
                    var pageIndex = SessionInfo.GetPageIndex(ControllerName);
                    var pageSize = SessionInfo.GetPageSize(ControllerName);

                    var entities = await QueryPageListAsync(pageIndex, pageSize).ConfigureAwait(false);

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
        protected virtual IActionResult ReturnIndexView(IEnumerable<TModel> models) => View("Index", CreateIndexViewModel(models));

        [HttpGet]
        [ActionName(nameof(ActionMode.IndexByPageIndex))]
        public virtual async Task<IActionResult> IndexByPageIndexAsync(int pageIndex, int pageSize)
        {
            var handled = false;
            var models = default(IEnumerable<TModel>);

            BeforeIndexByPageIndex(pageIndex, pageSize, ref models, ref handled);
            if (handled == false)
            {
                try
                {
                    var entities = await QueryPageListAsync(pageIndex, pageSize).ConfigureAwait(false);

                    models = entities.Select(e => ToModel(e));
                    models = BeforeView(models, ActionMode.Index);
                    models = await BeforeViewAsync(models, ActionMode.Index).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterIndexByPageIndex(models);
            return ReturnIndexView(models);
        }
        partial void BeforeIndexByPageIndex(int pageIndex, int pageSize, ref IEnumerable<TModel> models, ref bool handled);
        partial void AfterIndexByPageIndex(IEnumerable<TModel> models);

        [HttpGet]
        [ActionName(nameof(ActionMode.IndexByPageSize))]
        public virtual async Task<IActionResult> IndexByPageSizeAsync(int pageSize)
        {
            var handled = false;
            var models = default(IEnumerable<TModel>);

            BeforeIndexByPageSize(pageSize, ref models, ref handled);
            if (handled == false)
            {
                try
                {
                    var pageIndex = 0;

                    var entities = await QueryPageListAsync(pageIndex, pageSize).ConfigureAwait(false);

                    models = entities.Select(e => ToModel(e));
                    models = BeforeView(models, ActionMode.Index);
                    models = await BeforeViewAsync(models, ActionMode.Index).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterIndexByPageSize(models);
            return ReturnIndexView(models);
        }
        partial void BeforeIndexByPageSize(int pageSize, ref IEnumerable<TModel> models, ref bool handled);
        partial void AfterIndexByPageSize(IEnumerable<TModel> models);
        #endregion Index actions

        #region Model actions
        [HttpPost]
        [ActionName(nameof(ActionMode.Apply))]
        public virtual Task<IActionResult> ApplyAsync(TModel model)
        {
            FromEditToIndex = false;
            FromCreateToEdit = true;
            if (model.Id == 0)
            {
                return InsertAsync(model);
            }
            return UpdateAsync(model);
        }

        [HttpGet]
        [ActionName(nameof(ActionMode.Create))]
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
        [HttpGet]
        [ActionName(nameof(ActionMode.CreateById))]
        public virtual async Task<IActionResult> CreateAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeCreate(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    LastViewError = string.Empty;
                    model = await EditModelAsync(id).ConfigureAwait(false);
                    model.Id = 0;
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
        protected virtual IActionResult ReturnCreateView(TModel model) => View("Create", CreateEditViewModel(model));

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
        [ActionName(nameof(ActionMode.Insert))]
        public virtual async Task<IActionResult> InsertAsync(TModel model)
        {
            var handled = false;

            BeforeInsertModel(model, ref handled);
            if (handled == false)
            {
                try
                {
                    model = await InsertModelAsync(model).ConfigureAwait(false);
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
        public virtual async Task<TModel> InsertModelAsync(TModel model)
        {
            var handled = false;

            BeforeInsertModel(model, ref handled);
            if (handled == false)
            {
                using var ctrl = CreateController();
                var entity = await ctrl.InsertAsync(model).ConfigureAwait(false);

                model.CopyProperties(entity);
            }
            AfterInsertModel(model);
            return model;
        }
        partial void BeforeInsertModel(TModel model, ref bool handled);
        partial void AfterInsertModel(TModel model);
        protected virtual IActionResult ReturnAfterCreate(bool hasError, TModel model) => hasError ? View("Create", CreateEditViewModel(model)) : FromCreateToEdit ? RedirectToAction("Edit", new { model.Id }) : RedirectToAction("Index");

        [HttpGet]
        [ActionName(nameof(ActionMode.Edit))]
        public virtual async Task<IActionResult> EditAsync(int id)
        {
            var handled = false;
            var model = default(TModel);

            BeforeEdit(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    LastViewError = string.Empty;
                    model = await EditModelAsync(id).ConfigureAwait(false);
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
        protected virtual IActionResult ReturnEditView(TModel model)
        {
            return View("Edit", CreateEditViewModel(model));
        }

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
        [ActionName(nameof(ActionMode.Update))]
        public virtual async Task<IActionResult> UpdateAsync(TModel model)
        {
            try
            {
                model = await UpdateModelAsync(model).ConfigureAwait(false);
                LastViewError = string.Empty;
            }
            catch (Exception ex)
            {
                LastViewError = ex.GetError();
            }
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.Edit);
                model = await BeforeViewAsync(model, ActionMode.Edit).ConfigureAwait(false);
            }
            return ReturnAfterEdit(HasError, model);
        }
        protected virtual async Task<TModel> UpdateModelAsync(TModel model)
        {
            var handled = false;

            BeforeUpdateModel(model, ref handled);
            if (handled == false)
            {
                using var ctrl = CreateController();

                if (model is IMasterDetails modelMasterDetail)
                {
                    var entity = await ctrl.GetByIdAsync(model.Id).ConfigureAwait(false);
                    var entityModel = ToModel(entity);

                    if (entityModel is IMasterDetails entityMasterDetail)
                    {
                        entityMasterDetail.Master.CopyFrom(modelMasterDetail.Master);
                        entity = await ctrl.UpdateAsync(entityModel).ConfigureAwait(false);
                    }
                    model.CopyProperties(entity);
                }
                else
                {
                    var entity = await ctrl.UpdateAsync(model).ConfigureAwait(false);

                    model.CopyProperties(entity);
                }
            }
            AfterUpdateModel(model);
            return model;
        }
        partial void BeforeUpdateModel(TModel model, ref bool handled);
        partial void AfterUpdateModel(TModel model);
        protected virtual IActionResult ReturnAfterEdit(bool hasError, TModel model) => hasError ? View("Edit", CreateEditViewModel(model)) : FromEditToIndex ? RedirectToAction("Index") : RedirectToAction("Edit", new { model.Id });

        [HttpGet]
        [ActionName(nameof(ActionMode.Delete))]
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
        protected virtual IActionResult ReturnDeleteView(TModel model) => View("Delete", CreateDisplayViewModel(model));

        [ActionName(nameof(ActionMode.Delete))]
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
        protected virtual IActionResult ReturnAfterDelete(bool hasError, TModel model) => hasError ? View("Delete", CreateDisplayViewModel(model)) : RedirectToAction("Index");
        #endregion Model actions

        #region Details
        [HttpGet]
        [ActionName(nameof(ActionMode.Details))]
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
        protected virtual IActionResult ReturnDetailsView(TModel model) => View("Details", CreateDisplayViewModel(model));
        #endregion Details

        #region Detail model actions
        [HttpGet]
        [ActionName(nameof(ActionMode.CreateDetail))]
        public virtual async Task<IActionResult> CreateDetailAsync(int id)
        {
            var handled = false;
            var model = default(TModel);
            var masterDetailModel = new MasterDetailModel();

            BeforeCreateDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    LastViewError = string.Empty;
                    model = await GetModelAsync(id).ConfigureAwait(false);

                    if (model != null)
                    {
                        var oneProperty = model.GetType().GetProperty("OneModel");
                        var oneModel = oneProperty?.GetValue(model) as IdentityModel;
                        var createManyMethod = model.GetType().GetMethod("CreateManyModel");
                        var manyModel = createManyMethod?.Invoke(model, Array.Empty<object>()) as IdentityModel;
                        var addManyMethod = model.GetType().GetMethod("AddManyModel");

                        masterDetailModel.Master = oneModel;
                        masterDetailModel.Detail = manyModel;
                        addManyMethod?.Invoke(model, new object[] { masterDetailModel.Detail });
                    }
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
                masterDetailModel = BeforeViewMasterDetail(masterDetailModel, ActionMode.CreateDetail);
                masterDetailModel = await BeforeViewMasterDetailAsync(masterDetailModel, ActionMode.CreateDetail).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnCreateDetailView(masterDetailModel);
        }
        [HttpGet]
        [ActionName(nameof(ActionMode.CreateDetailById))]
        public virtual async Task<IActionResult> CreateDetailAsync(int id, int detailId)
        {
            var handled = false;
            var model = default(TModel);
            var masterDetailModel = new MasterDetailModel();

            BeforeCreateDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    LastViewError = string.Empty;
                    model = await GetModelAsync(id).ConfigureAwait(false);

                    if (model != null)
                    {
                        var oneProperty = model.GetType().GetProperty("OneModel");
                        var oneModel = oneProperty?.GetValue(model) as IdentityModel;
                        var getManyMethod = model.GetType().GetMethod("GetManyModelById");
                        var manyModel = getManyMethod?.Invoke(model, new object[] { detailId }) as IdentityModel;
                        var addManyMethod = model.GetType().GetMethod("AddManyModel");

                        masterDetailModel.Master = oneModel;
                        masterDetailModel.Detail = manyModel;
                        manyModel.Id = 0;
                        addManyMethod?.Invoke(model, new object[] { masterDetailModel.Detail });
                    }
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
                masterDetailModel = BeforeViewMasterDetail(masterDetailModel, ActionMode.CreateDetailById);
                masterDetailModel = await BeforeViewMasterDetailAsync(masterDetailModel, ActionMode.CreateDetailById).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnCreateDetailView(masterDetailModel);
        }
        partial void BeforeCreateDetail(ref TModel model, ref bool handled);
        partial void AfterCreateDetail(TModel model);
        protected virtual IActionResult ReturnCreateDetailView(MasterDetailModel model) => View("CreateDetail", model);

        [HttpPost]
        [ActionName(nameof(ActionMode.InsertDetail))]
        public virtual async Task<IActionResult> InsertDetailAsync(int id, IFormCollection formCollection)
        {
            var handled = false;
            var model = default(TModel);
            var masterDetailModel = new MasterDetailModel();

            BeforeAddDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    model = await GetModelAsync(id).ConfigureAwait(false);
                    if (model != null)
                    {
                        var oneProperty = model.GetType().GetProperty("OneModel");
                        var oneModel = oneProperty?.GetValue(model) as IdentityModel;
                        var createManyMethod = model.GetType().GetMethod("CreateManyModel");
                        var manyModel = createManyMethod?.Invoke(model, Array.Empty<object>()) as IdentityModel;
                        var addManyMethod = model.GetType().GetMethod("AddManyModel");

                        masterDetailModel.Master = oneModel;
                        masterDetailModel.Detail = manyModel;
                        SetModelValues(masterDetailModel.Detail, nameof(MasterDetailModel.Detail), formCollection);
                        addManyMethod?.Invoke(model, new object[] { masterDetailModel.Detail });

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
            if (HasError)
            {
                model = BeforeView(model, ActionMode.CreateDetail);
                model = await BeforeViewAsync(model, ActionMode.CreateDetail).ConfigureAwait(false);
                masterDetailModel = BeforeViewMasterDetail(masterDetailModel, ActionMode.CreateDetail);
                masterDetailModel = await BeforeViewMasterDetailAsync(masterDetailModel, ActionMode.CreateDetail).ConfigureAwait(false);
            }
            return HasError ? ReturnCreateDetailView(masterDetailModel) : RedirectToAction("Details", new { id = model.Id });
        }
        partial void BeforeAddDetail(ref TModel model, ref bool handled);
        partial void AfterAddDetail(TModel model);

        [HttpGet]
        [ActionName(nameof(ActionMode.EditDetail))]
        public virtual async Task<IActionResult> EditDetailAsync(int id, int detailId)
        {
            var handled = false;
            var model = default(TModel);
            var masterDetailModel = new MasterDetailModel();

            BeforeEditDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    LastViewError = string.Empty;
                    model = await GetModelAsync(id).ConfigureAwait(false);

                    if (model != null)
                    {
                        var oneProperty = model.GetType().GetProperty("OneModel");
                        var oneModel = oneProperty?.GetValue(model) as IdentityModel;
                        var getManyMethod = model.GetType().GetMethod("GetManyModelById");
                        var manyModel = getManyMethod?.Invoke(model, new object[] { detailId }) as IdentityModel;

                        masterDetailModel.Master = oneModel;
                        masterDetailModel.Detail = manyModel;
                    }
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterEditDetail(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.EditDetail);
                model = await BeforeViewAsync(model, ActionMode.EditDetail).ConfigureAwait(false);
                masterDetailModel = BeforeViewMasterDetail(masterDetailModel, ActionMode.EditDetail);
                masterDetailModel = await BeforeViewMasterDetailAsync(masterDetailModel, ActionMode.EditDetail).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnEditDetailView(masterDetailModel);
        }
        partial void BeforeEditDetail(ref TModel model, ref bool handled);
        partial void AfterEditDetail(TModel model);
        protected virtual IActionResult ReturnEditDetailView(MasterDetailModel model) => View("EditDetail", model);

        [HttpPost]
        [ActionName(nameof(ActionMode.UpdateDetail))]
        public virtual async Task<IActionResult> UpdateDetailAsync(int id, IFormCollection formCollection)
        {
            var handled = false;
            var model = default(TModel);
            var masterDetailModel = new MasterDetailModel();

            BeforeUpdateDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    model = await GetModelAsync(id).ConfigureAwait(false);
                    if (model != null)
                    {
                        var oneProperty = model.GetType().GetProperty("OneModel");
                        var oneModel = oneProperty?.GetValue(model) as IdentityModel;
                        var getManyMethod = model.GetType().GetMethod("GetManyModelById");

                        if (GetObjectId(nameof(MasterDetailModel.Detail), formCollection, out int detailId))
                        {
                            var manyModel = getManyMethod?.Invoke(model, new object[] { detailId }) as IdentityModel;

                            masterDetailModel.Master = oneModel;
                            masterDetailModel.Detail = manyModel;
                            SetModelValues(masterDetailModel.Detail, nameof(MasterDetailModel.Detail), formCollection);
                        }

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
            AfterUpdateDetail(model);
            if (HasError)
            {
                model = BeforeView(model, ActionMode.EditDetail);
                model = await BeforeViewAsync(model, ActionMode.EditDetail).ConfigureAwait(false);
                masterDetailModel = BeforeViewMasterDetail(masterDetailModel, ActionMode.EditDetail);
                masterDetailModel = await BeforeViewMasterDetailAsync(masterDetailModel, ActionMode.EditDetail).ConfigureAwait(false);
            }
            return HasError ? ReturnEditDetailView(masterDetailModel) : RedirectToAction("Details", new { id = model.Id });
        }
        partial void BeforeUpdateDetail(ref TModel model, ref bool handled);
        partial void AfterUpdateDetail(TModel model);

        [HttpGet]
        [ActionName(nameof(ActionMode.DeleteDetail))]
        public virtual async Task<IActionResult> ViewDeleteDetailAsync(int id, int detailId)
        {
            var handled = false;
            var model = default(TModel);
            var masterDetailModel = new MasterDetailModel();

            BeforeViewDeleteDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    LastViewError = string.Empty;
                    model = await GetModelAsync(id).ConfigureAwait(false);

                    if (model != null)
                    {
                        var oneProperty = model.GetType().GetProperty("OneModel");
                        var oneModel = oneProperty?.GetValue(model) as Models.IdentityModel;
                        var getManyMethod = model.GetType().GetMethod("GetManyModelById");
                        var manyModel = getManyMethod?.Invoke(model, new object[] { detailId }) as Models.IdentityModel;

                        masterDetailModel.Master = oneModel;
                        masterDetailModel.Detail = manyModel;
                    }
                }
                catch (Exception ex)
                {
                    LastViewError = ex.GetError();
                }
            }
            AfterViewDeleteDetail(model);
            if (HasError == false)
            {
                model = BeforeView(model, ActionMode.DeleteDetail);
                model = await BeforeViewAsync(model, ActionMode.DeleteDetail).ConfigureAwait(false);
                masterDetailModel = BeforeViewMasterDetail(masterDetailModel, ActionMode.DeleteDetail);
                masterDetailModel = await BeforeViewMasterDetailAsync(masterDetailModel, ActionMode.DeleteDetail).ConfigureAwait(false);
            }
            return HasError ? RedirectToAction("Index") : ReturnDeleteDetailView(masterDetailModel);
        }
        partial void BeforeViewDeleteDetail(ref TModel model, ref bool handled);
        partial void AfterViewDeleteDetail(TModel model);
        protected virtual IActionResult ReturnDeleteDetailView(MasterDetailModel model) => View("DeleteDetail", model);

        [HttpPost]
        [ActionName(nameof(ActionMode.DeleteDetail))]
        public virtual async Task<IActionResult> DeleteDetailAsync(int id, IFormCollection formCollection)
        {
            var handled = false;
            var model = default(TModel);
            var masterDetailModel = new MasterDetailModel();

            BeforeDeleteDetail(ref model, ref handled);
            if (handled == false)
            {
                try
                {
                    model = await GetModelAsync(id).ConfigureAwait(false);
                    if (model != null)
                    {
                        var oneProperty = model.GetType().GetProperty("OneModel");
                        var oneModel = oneProperty?.GetValue(model) as IdentityModel;
                        var removeManyMethod = model.GetType().GetMethod("RemoveManyModel");

                        if (GetObjectId(nameof(MasterDetailModel.Detail), formCollection, out int detailId))
                        {
                            var getManyMethod = model.GetType().GetMethod("GetManyModelById");
                            var manyModel = getManyMethod?.Invoke(model, new object[] { detailId }) as IdentityModel;

                            removeManyMethod?.Invoke(model, new object[] { detailId });

                            masterDetailModel.Master = oneModel;
                            masterDetailModel.Detail = manyModel;
                        }

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
            AfterDeleteDetail(model);
            if (HasError)
            {
                model = BeforeView(model, ActionMode.DeleteDetail);
                model = await BeforeViewAsync(model, ActionMode.DeleteDetail).ConfigureAwait(false);
                masterDetailModel = BeforeViewMasterDetail(masterDetailModel, ActionMode.DeleteDetail);
                masterDetailModel = await BeforeViewMasterDetailAsync(masterDetailModel, ActionMode.DeleteDetail).ConfigureAwait(false);
            }
            return HasError ? ReturnDeleteDetailView(masterDetailModel) : RedirectToAction("Details", new { id = model.Id });
        }
        partial void BeforeDeleteDetail(ref TModel model, ref bool handled);
        partial void AfterDeleteDetail(TModel model);
        #endregion Detail model actions

        #region Helpers
        public static bool GetObjectId(string prefix, IFormCollection formCollection, out int id)
        {
            formCollection.CheckArgument(nameof(formCollection));

            var result = false;
            var formKey = $"{prefix}.Id";

            if (formCollection.Keys.Contains(formKey))
            {
                var formValue = formCollection[formKey].FirstOrDefault();

                result = Int32.TryParse(formValue, out id);
            }
            else
            {
                id = 0;
            }
            return result;
        }
        public static void SetModelValues(object model, string prefix, IFormCollection formCollection)
        {
            model.CheckArgument(nameof(model));
            formCollection.CheckArgument(nameof(formCollection));

            foreach (var pi in model.GetType().GetProperties().Where(pi => pi.CanWrite))
            {
                var formKey = $"{prefix}.{pi.Name}";

                if (formCollection.Keys.Contains(formKey))
                {
                    var formValue = formCollection[formKey].FirstOrDefault();

                    if (pi.PropertyType.IsEnum)
                    {
                        if (Int32.TryParse(formValue, out int enumVal))
                        {
                            object value = Enum.Parse(pi.PropertyType, enumVal.ToString());

                            pi.SetValue(model, value);
                        }
                    }
                    else if (pi.PropertyType == typeof(string))
                    {
                        pi.SetValue(model, string.IsNullOrEmpty(formValue) ? null : formValue);
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        pi.SetValue(model, string.IsNullOrEmpty(formValue) ? null : System.DateTime.Parse(formValue));
                    }
                    else if (pi.PropertyType == typeof(DateTime?))
                    {
                        pi.SetValue(model, string.IsNullOrEmpty(formValue) ? null : System.DateTime.Parse(formValue));
                    }
                    else if (pi.PropertyType == typeof(Guid))
                    {
                        if (Guid.TryParse(formValue, out Guid guidVal))
                        {
                            pi.SetValue(model, guidVal);
                        }
                    }
                    else if (pi.PropertyType == typeof(Guid?))
                    {
                        if (string.IsNullOrEmpty(formValue) == false && Guid.TryParse(formValue, out Guid guidVal))
                        {
                            pi.SetValue(model, guidVal);
                        }
                    }
                    else if (pi.PropertyType == typeof(float) || pi.PropertyType == typeof(float?))
                    {
                        if (string.IsNullOrEmpty(formValue) == false && float.TryParse(formValue, out float parseVal))
                        {
                            pi.SetValue(model, parseVal);
                        }
                    }
                    else if (pi.PropertyType == typeof(double) || pi.PropertyType == typeof(double?))
                    {
                        if (string.IsNullOrEmpty(formValue) == false && double.TryParse(formValue, out double parseVal))
                        {
                            pi.SetValue(model, parseVal);
                        }
                    }
                    else if (pi.PropertyType == typeof(decimal) || pi.PropertyType == typeof(decimal?))
                    {
                        if (string.IsNullOrEmpty(formValue) == false && decimal.TryParse(formValue, out decimal parseVal))
                        {
                            pi.SetValue(model, parseVal);
                        }
                    }
                    else
                    {
                        object value = Convert.ChangeType(formValue, pi.PropertyType);

                        pi.SetValue(model, value);
                    }
                }
            }
        }
        #endregion Helpers
    }
}
//MdEnd
