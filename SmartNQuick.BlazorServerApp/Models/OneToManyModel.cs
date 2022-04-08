//@BaseCode
//MdStart
namespace SmartNQuick.BlazorServerApp.Models
{
    public abstract partial class OneToManyModel<TOne, TOneModel, TMany, TManyModel> : IdentityModel
        where TOne : Contracts.IIdentifiable
        where TMany : Contracts.IIdentifiable
        where TOneModel : IdentityModel, Contracts.ICopyable<TOne>, TOne, new()
        where TManyModel : IdentityModel, Contracts.ICopyable<TMany>, TMany, new()
    {
        public virtual TOneModel OneModel { get; } = new TOneModel();
        public virtual TOne OneItem => OneModel;

        public virtual List<TManyModel> ManyModels { get; } = new List<TManyModel>();
        public virtual IEnumerable<TMany> ManyItems => ManyModels as IEnumerable<TMany>;

        public override int Id { get => OneModel.Id; set => OneModel.Id = value; }
        public byte[] RowVersion
        {
            get
            {
                var result = default(byte[]);

                if (OneModel is VersionModel ve)
                    result = ve.RowVersion;

                return result ?? Array.Empty<byte>();
            }
            set
            {
                if (OneModel is VersionModel ve)
                    ve.RowVersion = value;
            }
        }

        public virtual void ClearManyItems()
        {
            ManyModels.Clear();
        }
        public virtual TMany CreateManyItem()
        {
            return new TManyModel();
        }
        public virtual void AddManyItem(TMany item)
        {
            item.CheckArgument(nameof(item));

            var model = new TManyModel();

            model.CopyProperties(item);
            ManyModels.Add(model);
        }
        public virtual void RemoveManyItem(TMany item)
        {
            item.CheckArgument(nameof(item));

            var model = ManyModels.FirstOrDefault(i => i.Id == item.Id);

            if (model != null)
            {
                ManyModels.Remove(model);
            }
        }
    }
}
//MdEnd