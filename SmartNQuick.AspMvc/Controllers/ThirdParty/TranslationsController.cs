namespace SmartNQuick.AspMvc.Controllers.Persistence.MusicStore
{
    using SmartNQuick.Contracts.Client;
    using TContract = Contracts.ThirdParty.ITranslation;
    using TModel = Models.ThirdParty.Translation;
    public partial class TranslationsController : GenericController<TContract, TModel>
    {
        protected override IAdapterAccess<TContract> CreateController()
        {
            return Adapters.Factory.CreateThridParty<TContract>("http://localhost:32885/api");
        }
    }
}
