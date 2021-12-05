//@BaseCode
//MdStart
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartNQuick.AspMvc.Models
{
    public abstract partial class OneToManyModel<TOne, TOneModel, TMany, TManyModel> : IdentityModel, IMasterDetails
        where TOne : Contracts.IIdentifiable
        where TOneModel : IdentityModel, Contracts.ICopyable<TOne>, TOne, new()
        where TMany : Contracts.IIdentifiable
        where TManyModel : IdentityModel, Contracts.ICopyable<TMany>, TMany, new()
    {
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

        public virtual TOneModel OneModel { get; } = new TOneModel();
        public virtual TOne OneItem => OneModel;

        public virtual List<TManyModel> ManyModels { get; } = new List<TManyModel>();
        public virtual IEnumerable<TMany> ManyItems => ManyModels as IEnumerable<TMany>;

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

        public virtual void ClearManyItems()
        {
            ManyModels.Clear();
        }
        public virtual TManyModel CreateManyModel() => new();
        public virtual TManyModel GetManyModelById(int id) => ManyModels.FirstOrDefault(x => x.Id == id);
        public void RemoveManyModel(int id)
        {
            var removeDetail = ManyModels.FirstOrDefault(i => i.Id == id);

            if (removeDetail != null)
            {
                ManyModels.Remove(removeDetail);
            }
        }

        public IdentityModel Master => OneModel;
        public Type MasterType => typeof(TOneModel);
        public IEnumerable<IdentityModel> Details => ManyModels;
        public Type DetailType => typeof(TManyModel);

        public void ClearDetails() => ClearManyItems();
        public IdentityModel CreateDetail() => new TManyModel();
        public void AddDetail(IdentityModel model) => ManyModels.Add(model as TManyModel);
        public void RemoveDetail(IdentityModel model) => ManyModels.Remove(model as TManyModel);
        public void RemoveDetailById(int id)
        {
            var manyModel = ManyModels.FirstOrDefault(e => e.Id == id);
            
            if (manyModel != null)
            {
                ManyModels.Remove(manyModel);
            }
        }
    }
}
//MdEnd