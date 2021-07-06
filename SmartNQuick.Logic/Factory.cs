//@BaseCode
using CommonBase.Extensions;
using ClientContracts = SmartNQuick.Contracts.Client;

namespace SmartNQuick.Logic
{
	public static partial class Factory
	{
		private static DataContext.IContext CreateContext()
		{
			return new DataContext.SmartNQuickDbContext();
		}
		static partial void CreateController<C>(DataContext.IContext context, ref ClientContracts.IControllerAccess<C> controller) 
			where C : SmartNQuick.Contracts.IIdentifiable;
		static partial void CreateController<C>(Controllers.ControllerObject controllerObject, ref ClientContracts.IControllerAccess<C> controller)
			where C : SmartNQuick.Contracts.IIdentifiable;
	}
}
