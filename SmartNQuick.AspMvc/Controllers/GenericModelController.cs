//@BaseCode
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Controllers
{
	public abstract class GenericModelController<Contract, Model> : Controller
		where Contract : Contracts.IIdentifiable, Contracts.ICopyable<Contract>
		where Model : Contract, new()
	{
		protected string ControllerName => GetType().Name.Replace("Controller", string.Empty);
		public async Task<IActionResult> Index()
		{
			using var ctrl = Logic.Factory.Create<Contract>();
			var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

			return View("Index", entities.Select(e => ToModel(e)));
		}
		protected Model ToModel(Contract entity)
		{
			entity.CheckArgument(nameof(entity));

			var result = new Model();

			result.CopyProperties(entity);
			return result;
		}
	}
}
