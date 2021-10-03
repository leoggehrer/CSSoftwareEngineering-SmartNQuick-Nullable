//@BaseCode
//MdStart
using CommonBase.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Persistence
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;
    using System.Reflection;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class GenericPersistenceController<C, E> : GenericController<C, E>
        where C : Contracts.IIdentifiable
        where E : Entities.IdentityEntity, Contracts.ICopyable<C>, C, new()
    {
        public override bool IsTransient => false;
        public DbSet<E> Set() => Context.Set<C, E>();
        public IQueryable<E> QueryableSet() => Context.Set<C, E>();
        protected GenericPersistenceController(DataContext.IContext context) : base(context)
        {
        }
        protected GenericPersistenceController(ControllerObject other) : base(other)
        {
        }

        public override Task<int> CountAsync()
        {
            return Context.CountAsync<C, E>();
        }
        public override Task<int> CountByAsync(string predicate)
        {
            return Context.CountByAsync<C, E>(predicate);
        }

        protected virtual E BeforeReturn(E entity) { return entity; }
        protected virtual Task<E> BeforeReturnAsync(E entity) => Task.FromResult(entity);

        #region Query
        public override async Task<C> GetByIdAsync(int id)
        {
            var result = await Context.GetByIdAsync<C, E>(id).ConfigureAwait(false);

            if (result == null)
            {
                throw new Exception($"Invalid id '{id}'.");
            }
            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        public override async Task<IEnumerable<C>> GetAllAsync()
        {
            return (await Context.GetAllAsync<C, E>()
                                 .ConfigureAwait(false))
                                 .Select(e => BeforeReturn(e));
        }
        public override async Task<IEnumerable<C>> QueryAllAsync(string predicate)
        {
            return (await Context.QueryAllAsync<C, E>(predicate)
                                 .ConfigureAwait(false))
                                 .Select(e => BeforeReturn(e));
        }
        #endregion Query

        #region InsertUpdate
        protected virtual E BeforeInsertUpdate(E entity) { return entity; }
        protected virtual Task<E> BeforeInsertUpdateAsync(E entity) => Task.FromResult(entity);
        protected virtual E AfterInsertUpdate(E entity) { return entity; }
        protected virtual Task<E> AfterInsertUpdateAsync(E entity) => Task.FromResult(entity);
        #endregion InsertUpdate

        #region Insert
        protected virtual E BeforeInsert(E entity) { return entity; }
        protected virtual Task<E> BeforeInsertAsync(E entity) => Task.FromResult(entity);
        public override async Task<C> InsertAsync(C entity)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Insert).ConfigureAwait(false);
#endif
            return await ExecuteInsertAsync(entity).ConfigureAwait(false);
        }
        public virtual async Task<C> ExecuteInsertAsync(C entity)
        {
            entity.CheckArgument(nameof(entity));

            var innerEntity = new E();

            innerEntity.CopyProperties(entity);

            innerEntity = BeforeInsertUpdate(innerEntity);
            innerEntity = await BeforeInsertUpdateAsync(innerEntity).ConfigureAwait(false);
            innerEntity = BeforeInsert(innerEntity);
            innerEntity = await BeforeInsertAsync(innerEntity).ConfigureAwait(false);
            var result = await Context.InsertAsync<C, E>(innerEntity).ConfigureAwait(false);
            result = AfterInsert(result);
            result = await AfterInsertAsync(result).ConfigureAwait(false);
            result = AfterInsertUpdate(result);
            result = await AfterInsertUpdateAsync(result).ConfigureAwait(false);
            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        protected virtual E AfterInsert(E entity) { return entity; }
        protected virtual Task<E> AfterInsertAsync(E entity) => Task.FromResult(entity);
        #endregion Insert

        #region Update
        protected virtual E BeforeUpdate(E entity) { return entity; }
        protected virtual Task<E> BeforeUpdateAsync(E entity) => Task.FromResult(entity);
        public override async Task<C> UpdateAsync(C entity)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Update).ConfigureAwait(false);
#endif
            return await ExecuteUpdateAsync(entity).ConfigureAwait(false);
        }
        public virtual async Task<C> ExecuteUpdateAsync(C entity)
        {
            entity.CheckArgument(nameof(entity));

            var innerEntity = await Context.GetByIdAsync<C, E>(entity.Id).ConfigureAwait(false);

            innerEntity.CopyProperties(entity);

            innerEntity = BeforeInsertUpdate(innerEntity);
            innerEntity = await BeforeInsertUpdateAsync(innerEntity).ConfigureAwait(false);
            innerEntity = BeforeUpdate(innerEntity);
            innerEntity = await BeforeUpdateAsync(innerEntity).ConfigureAwait(false);
            var result = await Context.UpdateAsync<C, E>(innerEntity).ConfigureAwait(false);
            result = AfterUpdate(result);
            result = await AfterUpdateAsync(result).ConfigureAwait(false);
            result = AfterInsertUpdate(result);
            result = await AfterInsertUpdateAsync(result).ConfigureAwait(false);
            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        protected virtual E AfterUpdate(E entity) { return entity; }
        protected virtual Task<E> AfterUpdateAsync(E entity) => Task.FromResult(entity);
        #endregion Update

        #region Delete
        protected virtual void BeforeDelete(E entity) { }
        protected virtual Task BeforeDeleteAsync(E entity) => Task.FromResult(0);
        public override async Task DeleteAsync(int id)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Delete).ConfigureAwait(false);
#endif
            await ExecuteDeleteAsync(id).ConfigureAwait(false);
        }
        public virtual async Task ExecuteDeleteAsync(int id)
        {
            var entity = await Context.GetByIdAsync<C, E>(id).ConfigureAwait(false);

            if (entity == null)
                throw new Exception($"Invalid id: '{id}'");

            BeforeDelete(entity);
            await BeforeDeleteAsync(entity).ConfigureAwait(false);
            await Context.DeleteAsync<C, E>(id).ConfigureAwait(false);
            AfterDelete(entity);
            await AfterDeleteAsync(entity).ConfigureAwait(false);
        }
        protected virtual void AfterDelete(E entity) { }
        protected virtual Task AfterDeleteAsync(E entity) => Task.FromResult(0);
        #endregion Delete

#if ACCOUNT_ON
        #region Authorization
		protected virtual Task CheckAuthorizationAsync(Type subjectType, MethodBase methodBase, AccessType accessType)
		{
			return CheckAuthorizationAsync(subjectType, methodBase, accessType, null);
		}
		protected virtual async Task CheckAuthorizationAsync(Type subjectType, MethodBase methodBase, AccessType accessType, string infoData)
		{
			subjectType.CheckArgument(nameof(subjectType));
			methodBase.CheckArgument(nameof(methodBase));

			bool handled = false;

			BeforeCheckAuthorization(subjectType, methodBase, accessType, ref handled);
			if (handled == false)
			{
				await Authorization.CheckAuthorizationAsync(SessionToken, subjectType, methodBase, accessType, infoData).ConfigureAwait(false);
			}
			AfterCheckAuthorization(subjectType, methodBase, accessType);
		}
		partial void BeforeCheckAuthorization(Type subjectType, MethodBase methodBase, AccessType accessType, ref bool handled);
		partial void AfterCheckAuthorization(Type subjectType, MethodBase methodBase, AccessType accessType);
        #endregion Authorization
#endif
    }
}
//MdEnd