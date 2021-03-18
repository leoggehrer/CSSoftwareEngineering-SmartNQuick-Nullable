using CommonBase.Extensions;
using SmartNQuick.Contracts;
using SmartNQuick.Contracts.Client;
using SmartNQuick.Logic.Contracts;
using SmartNQuick.Logic.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartNQuick.Logic
{
	partial class Factory
	{
		static partial void CreateController<C>(IContext context, ref IControllerAccess<C> controller) where C : IIdentifiable
		{
			if (typeof(C) == typeof(SmartNQuick.Contracts.Persistence.MusicStore.IGenre))
			{
				controller = new Controllers.Persistence.MusicStore.GenreController(context) as IControllerAccess<C>;
			}
			else if (typeof(C) == typeof(SmartNQuick.Contracts.Persistence.MusicStore.IArtist))
			{
				controller = new Controllers.Persistence.MusicStore.ArtistController(context) as IControllerAccess<C>;
			}
		}
		static partial void CreateController<C>(ControllerObject controllerObject, ref IControllerAccess<C> controller) where C : IIdentifiable
		{
			controllerObject.CheckArgument(nameof(controllerObject));

			if (typeof(C) == typeof(SmartNQuick.Contracts.Persistence.MusicStore.IGenre))
			{
				controller = new Controllers.Persistence.MusicStore.GenreController(controllerObject) as IControllerAccess<C>;
			}
			else if (typeof(C) == typeof(SmartNQuick.Contracts.Persistence.MusicStore.IArtist))
			{
				controller = new Controllers.Persistence.MusicStore.ArtistController(controllerObject) as IControllerAccess<C>;
			}
		}
	}
}
