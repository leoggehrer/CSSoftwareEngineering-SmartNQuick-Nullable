//@BaseCode
//MdStart
namespace SmartNQuick.AspMvc.Models
{
    public abstract partial class OneToAnotherModel<TOne, TOneModel, TAnother, TAnotherModel> : IdentityModel
        where TOne : Contracts.IIdentifiable
        where TAnother : Contracts.IIdentifiable
        where TOneModel : IdentityModel, Contracts.ICopyable<TOne>, TOne, new()
        where TAnotherModel : IdentityModel, Contracts.ICopyable<TAnother>, TAnother, new()
    {
        public virtual TOneModel OneModel { get; } = new TOneModel();
        public virtual TOne OneItem => OneModel;

        public virtual TAnotherModel AnotherEntity { get; } = new TAnotherModel();
        public virtual TAnother AnotherItem => AnotherEntity;

        public override int Id { get => OneModel.Id; set => OneModel.Id = value; }
        public byte[] RowVersion
        {
            get
            {
                var result = default(byte[]);

                if (OneModel is VersionModel ve)
                    result = ve.RowVersion;

                return result;
            }
            set
            {
                if (OneModel is VersionModel ve)
                    ve.RowVersion = value;
            }
        }
    }
}
//MdEnd