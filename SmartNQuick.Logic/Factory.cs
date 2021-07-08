//@BaseCode
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
            var result = default(ClientContracts.IControllerAccess<C>);

            CreateController(ref result);
            return result;
        }
        public static ClientContracts.IControllerAccess<C> Create<C>(object controllerObject)
            where C : SmartNQuick.Contracts.IIdentifiable
        {
            var result = default(ClientContracts.IControllerAccess<C>);

            CreateController(controllerObject, ref result);
            return result;
        }
        static partial void CreateController<C>(ref ClientContracts.IControllerAccess<C> controller)
            where C : SmartNQuick.Contracts.IIdentifiable;
        static partial void CreateController<C>(object controllerObject, ref ClientContracts.IControllerAccess<C> controller)
            where C : SmartNQuick.Contracts.IIdentifiable;
    }
}
