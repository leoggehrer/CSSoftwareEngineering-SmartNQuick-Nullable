using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Controllers
{
	public class GenreController : Controller
	{
		public async Task<IActionResult> Index()
		{
			using var ctrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
			var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

			return View(entities.Select(e => ToModel(e)));
		}

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
		private Models.MusicStore.Genre ToModel(Contracts.Persistence.MusicStore.IGenre entity)
		{
			entity.CheckArgument(nameof(entity));

			var result = new Models.MusicStore.Genre();

			result.CopyProperties(entity);
			return result;
		}
	}
}
