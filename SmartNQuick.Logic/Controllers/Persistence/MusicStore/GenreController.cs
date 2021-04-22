//@Ignore
using SmartNQuick.Logic.Contracts;
using TContract = SmartNQuick.Contracts.Persistence.MusicStore.IGenre;
using TEntity = SmartNQuick.Logic.Entities.MusicStore.Genre;

namespace SmartNQuick.Logic.Controllers.Persistence.MusicStore
{
	class GenreController : GenericPersistenceController<TContract, TEntity>
	{
		public GenreController(IContext context) : base(context)
		{
		}
		public GenreController(ControllerObject controllerObject) : base(controllerObject)
		{
		}
	}
}
