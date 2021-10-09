//@BaseCode
//MdStart
using CommonBase.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SmartNQuick.Logic.Entities
{
    internal abstract partial class OneToManyEntity<TOne, TOneEntity, TMany, TManyEntity> : IdentityEntity
        where TOne : Contracts.IIdentifiable
        where TMany : Contracts.IIdentifiable
        where TOneEntity : IdentityEntity, Contracts.ICopyable<TOne>, TOne, new()
        where TManyEntity : IdentityEntity, Contracts.ICopyable<TMany>, TMany, new()
    {
        public virtual TOneEntity OneEntity { get; set; } = new TOneEntity();
        public virtual TOne OneItem => OneEntity;

        public virtual List<TManyEntity> ManyEntities { get; } = new List<TManyEntity>();
        public virtual IEnumerable<TMany> ManyItems => ManyEntities;

        public virtual void ClearManyItems()
        {
            ManyEntities.Clear();
        }
        public virtual TMany CreateManyItem()
        {
            return new TManyEntity();
        }
        internal virtual void AddManyEntity(TManyEntity manyEntity)
        {
            manyEntity.CheckArgument(nameof(manyEntity));

            ManyEntities.Add(manyEntity);
        }
        public virtual void AddManyItem(TMany manyItem)
        {
            manyItem.CheckArgument(nameof(manyItem));

            var newSecond = new TManyEntity();

            newSecond.CopyProperties(manyItem);
            ManyEntities.Add(newSecond);
        }
        public virtual void RemoveManyItem(TMany manyItem)
        {
            manyItem.CheckArgument(nameof(manyItem));

            var item = ManyEntities.FirstOrDefault(i => i.Id == manyItem.Id);

            if (item != null)
            {
                ManyEntities.Remove(item);
            }
        }

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