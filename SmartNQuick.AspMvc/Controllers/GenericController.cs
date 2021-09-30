//@BaseCode
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
		protected string LastError { get; set; }
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
		public virtual async Task<IActionResult> Index()
		{
			using var ctrl = CreateController();
			var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

			return View("Index", entities.Select(e => ToModel(e)));
		}
		[HttpGet]
		public virtual async Task<IActionResult> Create()
		{
			using var ctrl = CreateController();
			var entity = await ctrl.CreateAsync().ConfigureAwait(false);

			return View(ToModel(entity));
		}
		[HttpPost]
		public virtual async Task<IActionResult> Insert(TModel model)
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
			return RedirectToAction("Index");
		}
		partial void BeforeInsertEntity(TModel model, ref bool handled);
		partial void AfterInsertEntity(TModel model);
		[HttpGet]
		public virtual async Task<IActionResult> Edit(int id)
		{
			using var ctrl = CreateController();
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return View(ToModel(entity));
		}
		[HttpPost]
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
			return RedirectToAction("Index");
		}
		partial void BeforeUpdateEntity(TModel model, ref bool handled);
		partial void AfterUpdateEntity(TModel model);
		[HttpGet]
		public virtual async Task<IActionResult> Delete(int id)
		{
			using var ctrl = CreateController();
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return View(ToModel(entity));
		}
		[HttpDelete]
		public virtual async Task<IActionResult> DeleteEntity(int id)
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
			return RedirectToAction("Index");
		}
		partial void BeforeDeleteEntity(int id, ref bool handled);
		partial void AfterDeleteEntity(int id);
	}
}
