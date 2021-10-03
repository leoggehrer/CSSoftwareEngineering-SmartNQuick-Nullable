//@BaseCode
//MdStart
using CommonBase.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SmartNQuick.AspMvc.Models
{
    public abstract partial class OneToManyModel<TOne, TOneModel, TMany, TManyModel> : IdentityModel
        where TOne : Contracts.IIdentifiable
        where TMany : Contracts.IIdentifiable
        where TOneModel : IdentityModel, Contracts.ICopyable<TOne>, TOne, new()
        where TManyModel : IdentityModel, Contracts.ICopyable<TMany>, TMany, new()
    {
        public virtual TOneModel OneModel { get; } = new TOneModel();
        public virtual TOne OneItem => OneModel;

        public virtual List<TManyModel> ManyEntities { get; } = new List<TManyModel>();
        public virtual IEnumerable<TMany> ManyItems => ManyEntities as IEnumerable<TMany>;

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

        public virtual void ClearManyItems()
        {
            ManyEntities.Clear();
        }
        public virtual TMany CreateManyItem()
        {
            return new TManyModel();
        }
        public virtual void AddManyItem(TMany secondItem)
        {
            secondItem.CheckArgument(nameof(secondItem));

            var newDetail = new TManyModel();

            newDetail.CopyProperties(secondItem);
            ManyEntities.Add(newDetail);
        }
        public virtual void RemoveManyItem(TMany secondItem)
        {
            secondItem.CheckArgument(nameof(secondItem));

            var removeDetail = ManyEntities.FirstOrDefault(i => i.Id == secondItem.Id);

            if (removeDetail != null)
            {
                ManyEntities.Remove(removeDetail);
            }
        }
    }
}
//MdEnd