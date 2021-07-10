//@BaseCode
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Controllers
{
	public abstract class GenericController<TContract, TModel> : Controller
		where TContract : Contracts.IIdentifiable, Contracts.ICopyable<TContract>
		where TModel : TContract, new()
	{
		protected string ControllerName => GetType().Name.Replace("Controller", string.Empty);
		protected static Contracts.Client.IControllerAccess<TContract> CreateController()
        {
			return Logic.Factory.Create<TContract>();
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
			using var ctrl = CreateController();

			await ctrl.InsertAsync(model).ConfigureAwait(false);
			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public virtual async Task<IActionResult> Edit(int id)
		{
			using var ctrl = CreateController();
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return View(ToModel(entity));
		}
		[HttpPost]
		public async Task<IActionResult> Update(TModel model)
		{
			using var ctrl = CreateController();
			var entity = await ctrl.GetByIdAsync(model.Id).ConfigureAwait(false);

			if (entity != null)
			{
				await ctrl.UpdateAsync(model).ConfigureAwait(false);
				await ctrl.SaveChangesAsync().ConfigureAwait(false);
			}
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			using var ctrl = CreateController();
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return View(ToModel(entity));
		}

		public async Task<IActionResult> DeleteEntity(int id)
		{
			using var ctrl = CreateController();

			await ctrl.DeleteAsync(id).ConfigureAwait(false);
			await ctrl.SaveChangesAsync().ConfigureAwait(false);

			return RedirectToAction("Index");
		}
	}
}
