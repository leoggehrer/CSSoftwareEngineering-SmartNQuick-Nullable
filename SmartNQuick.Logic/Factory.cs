//@BaseCode
using SmartNQuick.Contracts.Client;

namespace SmartNQuick.Logic
{
    public static partial class Factory
    {
        internal static DataContext.IContext CreateContext()
        {
            return new DataContext.SmartNQuickDbContext();
        }

        public static IControllerAccess<C> Create<C>()
            where C : SmartNQuick.Contracts.IIdentifiable
        {
            var result = default(IControllerAccess<C>);

            CreateController(ref result);
            return result;
        }
        public static IControllerAccess<C> Create<C>(object controllerObject)
            where C : SmartNQuick.Contracts.IIdentifiable
        {
            var result = default(IControllerAccess<C>);

            CreateController(controllerObject, ref result);
            return result;
        }
#if ACCOUNT_ON
        public static IAccountManager CreateAccountManager() => new Modules.Account.AccountManagerWrapper();
#endif
        static partial void CreateController<C>(ref IControllerAccess<C> controller)
            where C : SmartNQuick.Contracts.IIdentifiable;
        static partial void CreateController<C>(object controllerObject, ref IControllerAccess<C> controller)
            where C : SmartNQuick.Contracts.IIdentifiable;
    }
}
