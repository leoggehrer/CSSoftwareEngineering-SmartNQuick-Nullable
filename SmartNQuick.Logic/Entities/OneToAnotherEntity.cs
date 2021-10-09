//@BaseCode
//MdStart

namespace SmartNQuick.Logic.Entities
{
    internal abstract partial class OneToAnotherEntity<TOne, TOneEntity, TAnother, TAnotherEntity> : IdentityEntity
        where TOne : Contracts.IIdentifiable
        where TAnother : Contracts.IIdentifiable
        where TOneEntity : IdentityEntity, Contracts.ICopyable<TOne>, TOne, new()
        where TAnotherEntity : IdentityEntity, Contracts.ICopyable<TAnother>, TAnother, new()
    {
        public virtual TOneEntity OneEntity { get; set; } = new TOneEntity();
        public virtual TOne OneItem => OneEntity;

        public virtual TAnotherEntity AnotherEntity { get; set; } = new TAnotherEntity();
        public virtual TAnother AnotherItem => AnotherEntity;

        public override int Id { get => OneEntity.Id; set => OneEntity.Id = value; }
        public byte[] RowVersion
        {
            get
            {
                var result = default(byte[]);

                if (OneEntity is VersionEntity ve)
                    result = ve.RowVersion;

                return result;
            }
            set
            {
                if (OneEntity is VersionEntity ve)
                    ve.RowVersion = value;
            }
        }
    }
}
//MdEnd