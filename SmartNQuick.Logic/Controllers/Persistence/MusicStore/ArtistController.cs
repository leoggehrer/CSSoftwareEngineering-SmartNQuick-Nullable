using SmartNQuick.Logic.Contracts;
using TContract = SmartNQuick.Contracts.Persistence.MusicStore.IArtist;
using TEntity = SmartNQuick.Logic.Entities.MusicStore.Artist;

namespace SmartNQuick.Logic.Controllers.Persistence.MusicStore
{
	class ArtistController : GenericPersistenceController<TContract, TEntity>
	{
		public ArtistController(IContext context) : base(context)
		{
		}
		public ArtistController(ControllerObject controllerObject) : base(controllerObject)
		{
		}
	}
}
