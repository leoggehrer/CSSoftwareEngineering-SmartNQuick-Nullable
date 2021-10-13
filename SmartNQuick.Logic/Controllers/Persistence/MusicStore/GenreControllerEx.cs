using SmartNQuick.Contracts.Persistence.MusicStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Persistence.MusicStore
{
	partial class GenreController
	{
		[Modules.Security.AllowAnonymous]
		public override async Task<IEnumerable<IGenre>> GetAllAsync()
		{
			return await base.ExecuteGetEntityAllAsync().ConfigureAwait(false);
		}
	}
}
