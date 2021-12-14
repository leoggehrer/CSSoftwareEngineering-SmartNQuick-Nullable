//@BaseCode
//MdStart
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    internal abstract partial class GenericPersistenceController<C, E> : GenericController<C, E>
        where C : Contracts.IIdentifiable
        where E : Entities.IdentityEntity, Contracts.ICopyable<C>, C, new()
    {
        static GenericPersistenceController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public override bool IsTransient => false;

        public DbSet<E> Set() => Context.Set<C, E>();
        internal IQueryable<E> QueryableSet() => Context.QueryableSet<C, E>();

        protected GenericPersistenceController(DataContext.IContext context) : base(context)
        {
        }
        protected GenericPersistenceController(ControllerObject other) : base(other)
        {
        }

        #region Count
        internal override Task<int> ExecuteCountAsync()
        {
            return Context.CountAsync<C, E>();
        }
        internal override Task<int> ExecuteCountByAsync(string predicate)
        {
            return Context.CountByAsync<C, E>(predicate);
        }
        #endregion Count

        #region Query
        internal override Task<E> ExecuteGetEntityByIdAsync(int id)
        {
            return Context.GetByIdAsync<C, E>(id);
        }
        internal override async Task<IEnumerable<E>> ExecuteGetEntityAllAsync()
        {
            int idx = 0, qryCount;
            var result = new List<E>();
            do
            {
                var qry = await QueryableSet().Skip(idx++ * MaxPageSize)
                                              .Take(MaxPageSize)
                                              .ToArrayAsync()
                                              .ConfigureAwait(false);

                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteGetEntityAllAsync(string orderBy)
        {
            int idx = 0, qryCount;
            var result = new List<E>();
            do
            {
                var qry = await QueryableSet().OrderBy(orderBy)
                                              .Skip(idx++ * MaxPageSize)
                                              .Take(MaxPageSize)
                                              .ToArrayAsync()
                                              .ConfigureAwait(false);

                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return result;
        }

        internal override Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(string predicate)
        {
            return Context.QueryAllAsync<C, E>(predicate);
        }
        internal override Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(string predicate, string orderBy)
        {
            return Context.QueryAllAsync<C, E>(predicate, orderBy);
        }
        internal override async Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(Expression<Func<E, bool>> predicate)
        {
            int idx = 0, qryCount;
            var result = new List<E>();
            do
            {
                var qry = await QueryableSet().Where(predicate)
                                              .Skip(idx++ * MaxPageSize)
                                              .Take(MaxPageSize)
                                              .ToArrayAsync()
                                              .ConfigureAwait(false);

                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return result;
        }

        internal override async Task<IEnumerable<E>> ExecuteGetEntityPageListAsync(int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);

            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteGetEntityPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().OrderBy(orderBy)
                                             .Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);

            return result;
        }

        internal override async Task<IEnumerable<E>> ExecuteQueryEntityPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().Where(predicate)
                                             .Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);
            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteQueryEntityPageListAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().Where(predicate)
                                             .OrderBy(orderBy)
                                             .Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);
            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteQueryEntityPageListAsync(Expression<Func<E, bool>> predicate, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            var result = await QueryableSet().Where(predicate)
                                             .Skip(pageIndex * pageSize)
                                             .Take(pageSize)
                                             .ToArrayAsync()
                                             .ConfigureAwait(false);
            return result;
        }
        #endregion Query

        #region Insert
        internal override async Task<E> ExecuteInsertEntityAsync(E entity)
        {
            BeforeExecuteInsertEntity(entity);
            var result = await Context.InsertAsync<C, E>(entity).ConfigureAwait(false);
            AfterExecuteInsertEntity(entity);
            return result;
        }
        partial void BeforeExecuteInsertEntity(E entity);
        partial void AfterExecuteInsertEntity(E entity);
        #endregion Insert

        #region Update
        internal override async Task<E> ExecuteUpdateEntityAsync(E entity)
        {
            BeforeExecuteUpdateEntity(entity);
            var result = await Context.UpdateAsync<C, E>(entity).ConfigureAwait(false);
            AfterExecuteUpdateEntity(entity);
            return result;
        }
        partial void BeforeExecuteUpdateEntity(E entity);
        partial void AfterExecuteUpdateEntity(E entity);
        #endregion Update

        #region Delete
        internal override async Task ExecuteDeleteEntityAsync(E entity)
        {
            BeforeExecuteDeleteEntity(entity);
            await Context.DeleteAsync<C, E>(entity.Id).ConfigureAwait(false);
            AfterExecuteDeleteEntity(entity);
        }
        partial void BeforeExecuteDeleteEntity(E entity);
        partial void AfterExecuteDeleteEntity(E entity);
        #endregion Delete
    }
}
//MdEnd