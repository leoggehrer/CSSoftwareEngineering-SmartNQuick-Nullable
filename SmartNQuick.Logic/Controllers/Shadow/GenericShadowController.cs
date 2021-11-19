//@BaseCode
//MdStart
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Shadow
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class GenericShadowController<I, E, TSourceContract, TSourceEntity> : GenericController<I, E>
        where I : Contracts.IIdentifiable
        where E : Entities.ShadowEntity, I, Contracts.ICopyable<I>, new()
        where TSourceContract : Contracts.IIdentifiable, Contracts.ICopyable<TSourceContract>
        where TSourceEntity : Entities.IdentityEntity, TSourceContract, Contracts.ICopyable<TSourceContract>, new()
    {
        static GenericShadowController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        protected abstract GenericController<TSourceContract, TSourceEntity> SourceEntityController { get; set; }

        public override bool IsTransient => false;

        public GenericShadowController(DataContext.IContext context) : base(context)
        {
            Constructing();
#if ACCOUNT_ON
            ChangedSessionToken += GenericViewController_ChangedSessionToken;
#endif
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public GenericShadowController(ControllerObject controller) : base(controller)
        {
            Constructing();
#if ACCOUNT_ON
            ChangedSessionToken += GenericViewController_ChangedSessionToken;
#endif
            Constructed();
        }

#if ACCOUNT_ON 
        protected virtual void GenericViewController_ChangedSessionToken(object sender, EventArgs e)
        {
            SourceEntityController.SessionToken = SessionToken;
        }
#endif

        protected virtual E ConvertTo(TSourceContract contract)
        {
            contract.CheckArgument(nameof(contract));

            var result = new E();

            result.CopyFrom(contract);
            return result;
        }
        protected virtual IEnumerable<E> ConvertTo(IEnumerable<TSourceContract> contracts)
        {
            contracts.CheckArgument(nameof(contracts));

            var result = new List<E>();

            foreach (var item in contracts)
            {
                result.Add(ConvertTo(item));
            }
            return result;
        }

        #region Count
        internal override Task<int> ExecuteCountAsync()
        {
            return SourceEntityController.CountAsync();
        }
        internal override Task<int> ExecuteCountByAsync(string predicate)
        {
            return SourceEntityController.CountByAsync(predicate);
        }
        #endregion Count

        #region Query
        internal override async Task<E> ExecuteGetEntityByIdAsync(int id)
        {
            var entity = await SourceEntityController.GetByIdAsync(id).ConfigureAwait(false);
            var result = ConvertTo(entity);

            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteGetEntityAllAsync()
        {
            var entities = await SourceEntityController.GetAllAsync().ConfigureAwait(false);
            var result = ConvertTo(entities);

            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(string predicate)
        {
            var entities = await SourceEntityController.QueryAllAsync(predicate).ConfigureAwait(false);
            var result = ConvertTo(entities);

            return result;
        }
        #endregion Query

        #region Create
        internal override async Task<E> ExecuteCreateEntityAsync()
        {
            var entity = await SourceEntityController.CreateEntityAsync().ConfigureAwait(false);
            
            return ConvertTo(entity);
        }
        #endregion Create

        #region Insert
        internal override async Task<E> ExecuteInsertEntityAsync(E entity)
        {
            var sourceEntity = await SourceEntityController.CreateAsync().ConfigureAwait(false);

            sourceEntity.CopyFrom(entity);
            sourceEntity = await SourceEntityController.InsertAsync(sourceEntity).ConfigureAwait(false);

            return ConvertTo(sourceEntity);
        }
        #endregion Insert

        #region Update
        internal override async Task<E> ExecuteUpdateEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));

            var sourceEntity = new TSourceEntity();
            sourceEntity.CopyFrom(entity);
            var result = await SourceEntityController.UpdateAsync(sourceEntity).ConfigureAwait(false);

            return ConvertTo(result);
        }
        #endregion Update

        #region Delete
        internal override async Task<E> ExecuteDeleteEntityAsync(E entity)
        {
            await SourceEntityController.DeleteAsync(entity.Id).ConfigureAwait(false);

            return entity;
        }
        #endregion Delete

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                SourceEntityController.Dispose();

                SourceEntityController = null;
            }
        }
    }
}
//MdEnd