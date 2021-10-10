//@BaseCode
//MdStart
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public override bool IsTransient => false;
        public DbSet<E> Set() => Context.Set<C, E>();
        public IQueryable<E> QueryableSet() => Context.Set<C, E>();
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
        internal override Task<IEnumerable<E>> ExecuteGetEntityAllAsync()
        {
            return Context.GetAllAsync<C, E>();
        }
        internal override Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(string predicate)
        {
            return Context.QueryAllAsync<C, E>(predicate);
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