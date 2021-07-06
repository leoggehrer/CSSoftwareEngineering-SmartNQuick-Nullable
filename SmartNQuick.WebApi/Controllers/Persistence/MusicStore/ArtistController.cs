//@Ignore
using Microsoft.AspNetCore.Mvc;
using TContract = SmartNQuick.Contracts.Persistence.MusicStore.IArtist;
using TModel = SmartNQuick.Transfer.Persistence.MusicStore.Artist;

namespace SmartNQuick.WebApi.Controllers.Persistence.MusicStore
{
	[Route("api/[controller]")]
	[ApiController]
	public sealed class ArtistController : GenericController<TContract, TModel>
	{
	}
}
