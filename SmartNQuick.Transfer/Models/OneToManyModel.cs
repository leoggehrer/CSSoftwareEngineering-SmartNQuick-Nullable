//@BaseCode
//MdStart
using CommonBase.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SmartNQuick.Transfer.Models
{
    public abstract partial class OneToManyModel<TOne, TOneModel, TMany, TManyModel> : IdentityModel
        where TOne : Contracts.IIdentifiable
        where TMany : Contracts.IIdentifiable
        where TOneModel : IdentityModel, Contracts.ICopyable<TOne>, TOne, new()
        where TManyModel : IdentityModel, Contracts.ICopyable<TMany>, TMany, new()
    {
        public virtual TOneModel OneModel { get; set; } = new TOneModel();
        [JsonIgnore]
        public virtual TOne OneItem => OneModel;

        public virtual List<TManyModel> ManyModels { get; set; } = new List<TManyModel>();
        [JsonIgnore]
        public virtual IEnumerable<TMany> ManyItems => ManyModels as IEnumerable<TMany>;

        public virtual void ClearManyItems()
        {
            ManyModels.Clear();
        }
        public virtual TMany CreateManyItem()
        {
            return new TManyModel();
        }
        public virtual void AddManyItem(TMany manyItem)
        {
            manyItem.CheckArgument(nameof(manyItem));

            var newDetail = new TManyModel();

            newDetail.CopyProperties(manyItem);
            ManyModels.Add(newDetail);
        }
        public virtual void RemoveManyItem(TMany manyItem)
        {
            manyItem.CheckArgument(nameof(manyItem));

            var removeDetail = ManyModels.FirstOrDefault(i => i.Id == manyItem.Id);

            if (removeDetail != null)
            {
                ManyModels.Remove(removeDetail);
            }
        }

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