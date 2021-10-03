//@BaseCode
//MdStart

using System.Text.Json.Serialization;

namespace SmartNQuick.Transfer.Models
{
    public abstract partial class CompositeModel<TConnector, TConnectorModel, TOne, TOneModel, TAnother, TAnotherModel> : IdentityModel
        where TConnector : Contracts.IIdentifiable
        where TOne : Contracts.IIdentifiable
        where TAnother : Contracts.IIdentifiable
        where TConnectorModel : IdentityModel, Contracts.ICopyable<TConnector>, TConnector, new()
        where TOneModel : IdentityModel, Contracts.ICopyable<TOne>, TOne, new()
        where TAnotherModel : IdentityModel, Contracts.ICopyable<TAnother>, TAnother, new()
    {
        public virtual TConnectorModel ConnectorModel { get; set; } = new TConnectorModel();
        [JsonIgnore]
        public virtual TConnector ConnectorItem => ConnectorModel;

        public virtual TOneModel OneModel { get; set; } = new TOneModel();
        [JsonIgnore]
        public virtual TOne OneItem => OneModel;
        public bool OneItemIncludeSave { get; set; }

        public virtual TAnotherModel AnotherModel { get; set; } = new TAnotherModel();
        [JsonIgnore]
        public virtual TAnother AnotherItem => AnotherModel;
        public bool AnotherItemIncludeSave { get; set; }

        public override int Id { get => OneModel.Id; set => OneModel.Id = value; }
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