//@BaseCode
//MdStart
namespace SmartNQuick.Logic.Entities
{
    internal abstract partial class CompositeEntity<TConnector, TConnectorEntity, TOne, TOneEntity, TAnother, TAnotherEntity> : IdentityEntity
        where TConnector : Contracts.IIdentifiable
        where TOne : Contracts.IIdentifiable
        where TAnother : Contracts.IIdentifiable
        where TConnectorEntity : IdentityEntity, Contracts.ICopyable<TConnector>, TConnector, new()
        where TOneEntity : IdentityEntity, Contracts.ICopyable<TOne>, TOne, new()
        where TAnotherEntity : IdentityEntity, Contracts.ICopyable<TAnother>, TAnother, new()
    {
        public virtual TConnectorEntity ConnectorEntity { get; set; } = new TConnectorEntity();
        public virtual TConnector ConnectorItem => ConnectorEntity;

        public virtual TOneEntity OneEntity { get; set; } = new TOneEntity();
        public virtual TOne OneItem => OneEntity;
        public bool OneItemIncludeSave { get; set; }

        public virtual TAnotherEntity AnotherEntity { get; set; } = new TAnotherEntity();
        public virtual TAnother AnotherItem => AnotherEntity;
        public bool AnotherItemIncludeSave { get; set; }

        public override int Id { get => ConnectorEntity.Id; set => ConnectorEntity.Id = value; }
        public byte[] RowVersion
        {
            get
            {
                var result = default(byte[]);

                if (ConnectorEntity is VersionEntity ve)
                    result = ve.RowVersion;

                return result;
            }
            set
            {
                if (ConnectorEntity is VersionEntity ve)
                    ve.RowVersion = value;
            }
        }
    }
}
//MdEnd