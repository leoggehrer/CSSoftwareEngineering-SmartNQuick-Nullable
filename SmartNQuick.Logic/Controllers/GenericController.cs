//@BaseCode
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if ACCOUNT_ON
using SmartNQuick.Logic.Modules.Security;
using System.Reflection;
#endif

namespace SmartNQuick.Logic.Controllers
{
    internal abstract partial class GenericController<C, E> : ControllerObject, Contracts.Client.IControllerAccess<C>
        where C : Contracts.IIdentifiable
        where E : Entities.IdentityEntity, C, Contracts.ICopyable<C>, new()
    {
        static GenericController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public abstract bool IsTransient { get; }

        protected GenericController(DataContext.IContext context) : base(context)
        {
            Constructing();
            Constructed();
        }
        protected GenericController(ControllerObject other) : base(other)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        #region Count
        public abstract Task<int> CountAsync();
        public abstract Task<int> CountByAsync(string predicate);
        #endregion Count

        #region Before return
        protected virtual E BeforeReturn(E entity) { return entity; }
        protected virtual Task<E> BeforeReturnAsync(E entity) => Task.FromResult(entity);
        #endregion Before return

        #region Query
        public abstract Task<C> GetByIdAsync(int id);
        public abstract Task<IEnumerable<C>> GetAllAsync();
        public abstract Task<IEnumerable<C>> QueryAllAsync(string predicate);
        #endregion Query

        #region Create
        public virtual async Task<C> CreateAsync()
        {
            return await CreateEntityAsync().ConfigureAwait(false);
        }
        internal virtual async Task<E> CreateEntityAsync()
        {
            var entity = new E();

            AfterCreate(entity);
            return await BeforeReturnAsync(entity).ConfigureAwait(false);
        }
        internal virtual async Task<E> ExecuteCreateEntityAsync()
        {
            E entity = new();

            AfterCreate(entity);
            return await BeforeReturnAsync(entity).ConfigureAwait(false);
        }
        protected virtual void AfterCreate(E entity)
        {
        }
        #endregion Create

        #region InsertUpdate
        protected virtual E BeforeInsertUpdate(E entity) { return entity; }
        protected virtual Task<E> BeforeInsertUpdateAsync(E entity) => Task.FromResult(entity);
        protected virtual E AfterInsertUpdate(E entity) { return entity; }
        protected virtual Task<E> AfterInsertUpdateAsync(E entity) => Task.FromResult(entity);
        #endregion InsertUpdate

        #region Insert
        protected virtual E BeforeInsert(E entity) { return entity; }
        protected virtual Task<E> BeforeInsertAsync(E entity) => Task.FromResult(entity);
        public virtual async Task<C> InsertAsync(C entity)
        {
            entity.CheckArgument(nameof(entity));
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Insert).ConfigureAwait(false);
#endif
            var innerEntity = new E();

            innerEntity.CopyProperties(entity);

            return await InsertEntityAsync(innerEntity).ConfigureAwait(false);
        }
        internal virtual async Task<E> InsertEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));

            entity = BeforeInsertUpdate(entity);
            entity = await BeforeInsertUpdateAsync(entity).ConfigureAwait(false);
            entity = BeforeInsert(entity);
            entity = await BeforeInsertAsync(entity).ConfigureAwait(false);
            var result = await ExecuteInsertEntityAsync(entity).ConfigureAwait(false);
            result = AfterInsert(result);
            result = await AfterInsertAsync(result).ConfigureAwait(false);
            result = AfterInsertUpdate(result);
            result = await AfterInsertUpdateAsync(result).ConfigureAwait(false);
            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        internal abstract Task<E> ExecuteInsertEntityAsync(E entity);
        protected virtual E AfterInsert(E entity) { return entity; }
        protected virtual Task<E> AfterInsertAsync(E entity) => Task.FromResult(entity);
        public virtual async Task<IEnumerable<C>> InsertAsync(IEnumerable<C> entities)
        {
            entities.CheckArgument(nameof(entities));

            var result = new List<C>();

            foreach (var entity in entities)
            {
                result.Add(await InsertAsync(entity).ConfigureAwait(false));
            }
            return result.AsQueryable();
        }
        #endregion Insert

        #region Update
        protected virtual E BeforeUpdate(E entity) { return entity; }
        protected virtual Task<E> BeforeUpdateAsync(E entity) => Task.FromResult(entity);
        public virtual async Task<C> UpdateAsync(C entity)
        {
            entity.CheckArgument(nameof(entity));
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Update).ConfigureAwait(false);
#endif
            var innerEntity = await Context.GetByIdAsync<C, E>(entity.Id).ConfigureAwait(false);

            innerEntity.CopyProperties(entity);

            return await UpdateEntityAsync(innerEntity).ConfigureAwait(false);
        }
        internal virtual async Task<E> UpdateEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));

            entity = BeforeInsertUpdate(entity);
            entity = await BeforeInsertUpdateAsync(entity).ConfigureAwait(false);
            entity = BeforeUpdate(entity);
            entity = await BeforeUpdateAsync(entity).ConfigureAwait(false);
            var result = await ExecuteUpdateEntityAsync(entity).ConfigureAwait(false);
            result = AfterUpdate(result);
            result = await AfterUpdateAsync(result).ConfigureAwait(false);
            result = AfterInsertUpdate(result);
            result = await AfterInsertUpdateAsync(result).ConfigureAwait(false);
            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        internal abstract Task<E> ExecuteUpdateEntityAsync(E entity);
        protected virtual E AfterUpdate(E entity) { return entity; }
        protected virtual Task<E> AfterUpdateAsync(E entity) => Task.FromResult(entity);
        public virtual async Task<IEnumerable<C>> UpdateAsync(IEnumerable<C> entities)
        {
            entities.CheckArgument(nameof(entities));

            var result = new List<C>();

            foreach (var entity in entities)
            {
                result.Add(await UpdateAsync(entity).ConfigureAwait(false));
            }
            return result.AsQueryable();
        }
        #endregion Update

        #region Delete
        protected virtual void BeforeDelete(E entity) { }
        protected virtual Task BeforeDeleteAsync(E entity) => Task.FromResult(0);
        public virtual async Task DeleteAsync(int id)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Delete).ConfigureAwait(false);
#endif
            var entity = await Context.GetByIdAsync<C, E>(id).ConfigureAwait(false);

            if (entity == null)
                throw new Exception($"Invalid id: '{id}'");

            await DeleteEntityAsync(entity).ConfigureAwait(false);
        }
        internal virtual async Task DeleteEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));

            BeforeDelete(entity);
            await BeforeDeleteAsync(entity).ConfigureAwait(false);
            await ExecuteDeleteEntityAsync(entity).ConfigureAwait(false);
            AfterDelete(entity);
            await AfterDeleteAsync(entity).ConfigureAwait(false);
        }
        internal abstract Task ExecuteDeleteEntityAsync(E entity);
        protected virtual void AfterDelete(E entity) { }
        protected virtual Task AfterDeleteAsync(E entity) => Task.FromResult(0);
        #endregion Delete

        public virtual Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
        public virtual Task<int> RejectChangesAsync()
        {
            return Context.RejectChangesAsync();
        }

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