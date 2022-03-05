//@BaseCode
//MdStart
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using SmartNQuick.Logic.Modules.Exception;

namespace SmartNQuick.Logic.Controllers.Persistence
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class GenericPersistenceController<TContract, TEntity> : GenericController<TContract, TEntity>
        where TContract : Contracts.IIdentifiable
        where TEntity : Entities.IdentityEntity, Contracts.ICopyable<TContract>, TContract, new()
    {
        static GenericPersistenceController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public override bool IsTransient => false;

        public DbSet<TEntity> Set() => Context.Set<TContract, TEntity>();
        internal IQueryable<TEntity> QueryableSet() => Context.QueryableSet<TContract, TEntity>();

        protected GenericPersistenceController(DataContext.IContext context) : base(context)
        {
        }
        protected GenericPersistenceController(ControllerObject other) : base(other)
        {
        }

        #region Count
        internal override Task<int> ExecuteCountAsync()
        {
            return Context.CountAsync<TContract, TEntity>();
        }
        internal override Task<int> ExecuteCountByAsync(string predicate)
        {
            return Context.CountByAsync<TContract, TEntity>(predicate);
        }
        #endregion Count

        #region Query
        internal override Task<TEntity> ExecuteGetEntityByIdAsync(int id)
        {
            return Context.GetByIdAsync<TContract, TEntity>(id);
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityAllAsync()
        {
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            do
            {
                var qry = await QueryableSet().Skip(idx++ * MaxPageSize)
                                              .Take(MaxPageSize)
                                              .AsNoTracking()
                                              .ToArrayAsync()
                                              .ConfigureAwait(false);

                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityAllAsync(string orderBy)
        {
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            do
            {
                var qry = await QueryableSet().OrderBy(orderBy)
                                              .Skip(idx++ * MaxPageSize)
                                              .Take(MaxPageSize)
                                              .AsNoTracking()
                                              .ToArrayAsync()
                                              .ConfigureAwait(false);

                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return result;
        }

        internal override Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(string predicate)
        {
            return Context.QueryAllAsync<TContract, TEntity>(predicate);
        }
        internal override Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(string predicate, string orderBy)
        {
            return Context.QueryAllAsync<TContract, TEntity>(predicate, orderBy);
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            do
            {
                var qry = await QueryableSet().Where(predicate)
                                              .Skip(idx++ * MaxPageSize)
                                              .Take(MaxPageSize)
                                              .AsNoTracking()
                                              .ToArrayAsync()
                                              .ConfigureAwait(false);

                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return result;
        }

        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityPageListAsync(int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .AsNoTracking()
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);

            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().OrderBy(orderBy)
                                             .Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .AsNoTracking()
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);

            return result;
        }

        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().Where(predicate)
                                             .Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .AsNoTracking()
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().Where(predicate)
                                             .OrderBy(orderBy)
                                             .Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .AsNoTracking()
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().Where(predicate)
                                             .Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .AsNoTracking()
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);
            return result;
        }
        #endregion Query

        #region Insert
        internal override async Task<TEntity> ExecuteInsertEntityAsync(TEntity entity)
        {
            BeforeExecuteInsertEntity(entity);
            var result = await Context.InsertAsync<TContract, TEntity>(entity).ConfigureAwait(false);
            AfterExecuteInsertEntity(entity);
            return result;
        }
        partial void BeforeExecuteInsertEntity(TEntity entity);
        partial void AfterExecuteInsertEntity(TEntity entity);
        #endregion Insert

        #region Update
        internal override async Task<TEntity> ExecuteUpdateEntityAsync(TEntity entity)
        {
            BeforeExecuteUpdateEntity(entity);
            var result = await Context.UpdateAsync<TContract, TEntity>(entity).ConfigureAwait(false);
            AfterExecuteUpdateEntity(entity);
            return result;
        }
        partial void BeforeExecuteUpdateEntity(TEntity entity);
        partial void AfterExecuteUpdateEntity(TEntity entity);
        #endregion Update

        #region Delete
        internal override async Task ExecuteDeleteEntityAsync(TEntity entity)
        {
            BeforeExecuteDeleteEntity(entity);
            await Context.DeleteAsync<TContract, TEntity>(entity.Id).ConfigureAwait(false);
            AfterExecuteDeleteEntity(entity);
        }
        partial void BeforeExecuteDeleteEntity(TEntity entity);
        partial void AfterExecuteDeleteEntity(TEntity entity);
        #endregion Delete
    }
}
//MdEnd