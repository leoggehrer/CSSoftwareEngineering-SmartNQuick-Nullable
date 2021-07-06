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
		public static ClientContracts.IControllerAccess<C> Create<C>()
			where C : SmartNQuick.Contracts.IIdentifiable
		{
			ClientContracts.IControllerAccess<C> result = null;

			CreateController<C>(CreateContext(), ref result);
			return result;
		}
		static partial void CreateController<C>(DataContext.IContext context, ref ClientContracts.IControllerAccess<C> controller) 
			where C : SmartNQuick.Contracts.IIdentifiable;
		public static ClientContracts.IControllerAccess<C> Create<C>(object controller)
			where C : SmartNQuick.Contracts.IIdentifiable
		{
			var controllerObject = controller as Controllers.ControllerObject;

			controllerObject.CheckArgument(nameof(controller));

			ClientContracts.IControllerAccess<C> result = null;

			CreateController<C>(controllerObject, ref result);
			return result;
		}
		static partial void CreateController<C>(Controllers.ControllerObject controllerObject, ref ClientContracts.IControllerAccess<C> controller)
			where C : SmartNQuick.Contracts.IIdentifiable;
	}
}
