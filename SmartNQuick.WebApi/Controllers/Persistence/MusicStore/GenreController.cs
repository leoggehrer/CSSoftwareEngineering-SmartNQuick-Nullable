using Microsoft.AspNetCore.Mvc;
using TContract = SmartNQuick.Contracts.Persistence.MusicStore.IGenre;
using TModel = SmartNQuick.Transfer.Models.Persistence.MusicStore.Genre;

namespace SmartNQuick.WebApi.Controllers.Persistence.MusicStore
{
	[Route("api/[controller]")]
	[ApiController]
	public sealed class GenreController : GenericController<TContract, TModel>
	{
	}
}
