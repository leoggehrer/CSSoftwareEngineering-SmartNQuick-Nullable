using SmartNQuick.Logic.Contracts;
using TContract = SmartNQuick.Contracts.Persistence.MusicStore.IAlbum;
using TEntity = SmartNQuick.Logic.Entities.MusicStore.Album;

namespace SmartNQuick.Logic.Controllers.Persistence.MusicStore
{
	class AlbumController : GenericPersistenceController<TContract, TEntity>
	{
		public AlbumController(IContext context) : base(context)
		{
		}
		public AlbumController(ControllerObject controllerObject) : base(controllerObject)
		{
		}
	}
}
