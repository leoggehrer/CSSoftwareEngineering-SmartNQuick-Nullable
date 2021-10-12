//@BaseCode
//MdStart

namespace SmartNQuick.AspMvc.Models
{
    public abstract partial class CompositeModel<TConnector, TConnectorModel, TOne, TOneModel, TAnother, TAnotherModel> : IdentityModel
        where TConnector : Contracts.IIdentifiable
        where TOne : Contracts.IIdentifiable
        where TAnother : Contracts.IIdentifiable
        where TConnectorModel : IdentityModel, Contracts.ICopyable<TConnector>, TConnector, new()
        where TOneModel : IdentityModel, Contracts.ICopyable<TOne>, TOne, new()
        where TAnotherModel : IdentityModel, Contracts.ICopyable<TAnother>, TAnother, new()
    {
        public virtual TConnectorModel ConnectorModel { get; } = new TConnectorModel();
        public virtual TConnector ConnectorItem => ConnectorModel;

        public virtual TOneModel OneModel { get; } = new TOneModel();
        public virtual TOne OneItem => OneModel;
        public bool OneItemIncludeSave { get; set; }

        public virtual TAnotherModel AnotherModel { get; } = new TAnotherModel();
        public virtual TAnother AnotherItem => AnotherModel;
        public bool AnotherItemIncludeSave { get; set; }

        public override int Id { get => ConnectorModel.Id; set => ConnectorModel.Id = value; }
        public byte[] RowVersion
        {
            get
            {
                var result = default(byte[]);

                if (ConnectorModel is VersionModel ve)
                    result = ve.RowVersion;

                return result;
            }
            set
            {
                if (ConnectorModel is VersionModel ve)
                    ve.RowVersion = value;
            }
        }
    }
}
//MdEnd