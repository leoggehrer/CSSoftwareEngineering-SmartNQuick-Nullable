//@Ignore
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Contract = SmartNQuick.Contracts.Persistence.MusicStore.IGenre;
using Model = SmartNQuick.AspMvc.Models.MusicStore.Genre;

namespace SmartNQuick.AspMvc.Controllers
{
	public class GenreController : GenericController<Contract, Model>
	{
		//public async Task<IActionResult> Index()
		//{
		//	using var ctrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
		//	var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

		//	return View(entities.Select(e => ToModel(e)));
		//}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			using var ctrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
			var entity = await ctrl.CreateAsync().ConfigureAwait(false);

			return View(ToModel(entity));
		}
		[HttpPost]
		public async Task<IActionResult> Insert(Models.MusicStore.Genre model)
		{
			using var ctrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();

			await ctrl.InsertAsync(model).ConfigureAwait(false);
			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			using var ctrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return View(ToModel(entity));
		}
		[HttpPost]
		public async Task<IActionResult> Update(Models.MusicStore.Genre model)
		{
			using var ctrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
			var entity = await ctrl.GetByIdAsync(model.Id).ConfigureAwait(false);

			if (entity != null)
			{
				entity.Name = model.Name;
				await ctrl.UpdateAsync(entity).ConfigureAwait(false);
				await ctrl.SaveChangesAsync().ConfigureAwait(false);
			}
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			using var ctrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return View(ToModel(entity));
		}

//		[HttpDelete]
		public async Task<IActionResult> DeleteEntity(int id)
		{
			using var ctrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
			
			await ctrl.DeleteAsync(id).ConfigureAwait(false);
			await ctrl.SaveChangesAsync().ConfigureAwait(false);

			return RedirectToAction("Index");
		}

	}
}
