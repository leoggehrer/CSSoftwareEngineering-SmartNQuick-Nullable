//@BaseCode
//MdStart
using CommonBase.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartNQuick.Logic.Modules.Exception;
#if ACCOUNT_ON
using System.Reflection;
#endif

namespace SmartNQuick.Logic.Controllers
{
    internal abstract partial class GenericController<C, E> : ControllerObject, Contracts.Client.IControllerAccess<C>
        where C : Contracts.IIdentifiable
        where E : Entities.IdentityEntity, C, Contracts.ICopyable<C>, new()
    {
        #region Class-Constructors
        static GenericController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region Properties
        public abstract bool IsTransient { get; }
        #endregion Properties

        #region Instance-Constructors
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
        #endregion Instance-Constructors

        #region Converter
        protected virtual E ConvertTo(C contract)
        {
            contract.CheckArgument(nameof(contract));

            var result = new E();

            result.CopyProperties(contract);
            return result;
        }
        protected virtual IQueryable<E> ConvertTo(IQueryable<C> contracts)
        {
            contracts.CheckArgument(nameof(contracts));

            var result = new List<E>();

            foreach (var item in contracts)
            {
                result.Add(ConvertTo(item));
            }
            return result.AsQueryable();
        }
        #endregion Converter

        #region Count
        public virtual async Task<int> CountAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.QueryCount).ConfigureAwait(false);
#endif
            return await ExecuteCountAsync().ConfigureAwait(false);
        }
        internal abstract Task<int> ExecuteCountAsync();

        public virtual async Task<int> CountByAsync(string predicate)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.QueryCountBy).ConfigureAwait(false);
#endif
            return await ExecuteCountByAsync(predicate).ConfigureAwait(false);
        }
        internal abstract Task<int> ExecuteCountByAsync(string predicate);
        #endregion Count

        #region Before-Return
        protected virtual E BeforeReturn(E entity) { return entity; }
        protected virtual IEnumerable<E> BeforeReturn(IEnumerable<E> entities)
        {
            var result = new List<E>();

            foreach (var item in entities)
            {
                result.Add(BeforeReturn(item));
            }
            return result;
        }
        protected virtual Task<E> BeforeReturnAsync(E entity) => Task.FromResult(entity);
        protected virtual async Task<IEnumerable<E>> BeforeReturnAsync(IEnumerable<E> entities)
        {
            var result = new List<E>();

            foreach (var item in entities)
            {
                result.Add(await BeforeReturnAsync(item).ConfigureAwait(false));
            }
            return result;
        }
        #endregion Before-Return

        #region Query
        public virtual async Task<C> GetByIdAsync(int id)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.GetBy).ConfigureAwait(false);
#endif
            return await GetEntityByIdAsync(id).ConfigureAwait(false);
        }
        internal virtual async Task<E> GetEntityByIdAsync(int id)
        {
            var result = await ExecuteGetEntityByIdAsync(id).ConfigureAwait(false);

            if (result == null)
            {
                throw new LogicException(ErrorType.InvalidId, $"Invalid id '{id}'.");
            }
            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        internal abstract Task<E> ExecuteGetEntityByIdAsync(int id);

        public virtual async Task<IEnumerable<C>> GetAllAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.GetAll).ConfigureAwait(false);
#endif
            return await GetEntityAllAsync().ConfigureAwait(false);
        }
        internal virtual async Task<IEnumerable<E>> GetEntityAllAsync()
        {
            var result = await ExecuteGetEntityAllAsync().ConfigureAwait(false);

            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        internal abstract Task<IEnumerable<E>> ExecuteGetEntityAllAsync();

        public virtual async Task<IEnumerable<C>> QueryAllAsync(string predicate)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.QueryAll).ConfigureAwait(false);
#endif
            return await QueryEntityAllAsync(predicate).ConfigureAwait(false);
        }
        internal virtual async Task<IEnumerable<E>> QueryEntityAllAsync(string predicate)
        {
            var result = await ExecuteQueryEntityAllAsync(predicate).ConfigureAwait(false);

            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        internal abstract Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(string predicate);
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
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.InsertArray).ConfigureAwait(false);
#endif
            var result = new List<C>();

            foreach (var entity in entities)
            {
                result.Add(await InsertEntityAsync(ConvertTo(entity)).ConfigureAwait(false));
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
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.UpdateArray).ConfigureAwait(false);
#endif
            var result = new List<C>();

            foreach (var entity in entities)
            {
                result.Add(await UpdateEntityAsync(ConvertTo(entity)).ConfigureAwait(false));
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
                throw new LogicException(ErrorType.InvalidId);

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
    }
}
//MdEnd