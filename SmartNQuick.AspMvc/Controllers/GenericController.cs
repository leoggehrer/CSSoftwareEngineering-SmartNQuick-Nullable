//@BaseCode
//MdStart
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Controllers
{
    public abstract partial class GenericController<TContract, TModel> : Controller
		where TContract : Contracts.IIdentifiable, Contracts.ICopyable<TContract>
		where TModel : TContract, new()
	{
		private string lastError;

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

		protected string LastError 
		{
			get => lastError;
			set
			{
				lastError = value;
				Modules.Handler.ErrorHandler.LastError = value;
			}
		}
		protected bool HasError => string.IsNullOrEmpty(LastError) == false;
		protected string ControllerName => GetType().Name.Replace("Controller", string.Empty);
		protected static Contracts.Client.IAdapterAccess<TContract> CreateController()
		{
			return Adapters.Factory.Create<TContract>();
		}
		protected TModel ToModel(TContract entity)
		{
			entity.CheckArgument(nameof(entity));

			var result = new TModel();

			result.CopyProperties(entity);
			return result;
		}

		[HttpGet]
		public virtual async Task<IActionResult> Index()
		{
			using var ctrl = CreateController();
			var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

			return View("Index", entities.Select(e => ToModel(e)));
		}
		[HttpGet]
		[ActionName("Create")]
		public virtual async Task<IActionResult> CreateAsync()
		{
			var model = await CreateModelAsync().ConfigureAwait(false);

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

					await ctrl.InsertAsync(model).ConfigureAwait(false);
					LastError = string.Empty;
				}
				catch (Exception ex)
				{
					LastError = ex.GetError();
				}
			}
			AfterInsertEntity(model);
			return HasError ? View("Create", model) : RedirectToAction("Index");
		}
		partial void BeforeInsertEntity(TModel model, ref bool handled);
		partial void AfterInsertEntity(TModel model);

		[HttpGet]
		[ActionName("Edit")]
		public virtual async Task<IActionResult> EditAsync(int id)
		{
			var model = await EditModelAsync(id).ConfigureAwait(false);

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
					var entity = await ctrl.GetByIdAsync(model.Id).ConfigureAwait(false);

					if (entity != null)
					{
						await ctrl.UpdateAsync(model).ConfigureAwait(false);
					}
					LastError = string.Empty;
				}
				catch (Exception ex)
				{
					LastError = ex.GetError();
				}
			}
			AfterUpdateEntity(model);
			return HasError ? View("Edit", model) : RedirectToAction("Index");
		}
		partial void BeforeUpdateEntity(TModel model, ref bool handled);
		partial void AfterUpdateEntity(TModel model);

		[HttpGet]
		[ActionName("Delete")]
		public virtual async Task<IActionResult> ViewDeleteAsync(int id)
		{
			using var ctrl = CreateController();
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return View("Delete", ToModel(entity));
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
