//@BaseCode
//MdStart
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;

namespace SmartNQuick.Logic.Controllers.Business
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class BusinessControllerAdapter<TContract, TEntity> : GenericController<TContract, TEntity>
        where TContract : Contracts.IIdentifiable
        where TEntity : Entities.IdentityEntity, TContract, Contracts.ICopyable<TContract>, new()
    {
        static BusinessControllerAdapter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public override bool IsTransient => true;

        public BusinessControllerAdapter(DataContext.IContext context) : base(context)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public BusinessControllerAdapter(ControllerObject controller) : base(controller)
        {
            Constructing();
            Constructed();
        }

        #region Count
        internal override Task<int> ExecuteCountAsync()
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<int> ExecuteCountByAsync(string predicate)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Count

        #region Query
        internal override Task<TEntity> ExecuteGetEntityByIdAsync(int id)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        internal override Task<IEnumerable<TEntity>> ExecuteGetEntityAllAsync()
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<IEnumerable<TEntity>> ExecuteGetEntityAllAsync(string orderBy)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        internal override Task<IEnumerable<TEntity>> ExecuteGetEntityPageListAsync(int pageIndex, int pageSize)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<IEnumerable<TEntity>> ExecuteGetEntityPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        internal override Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(string predicate)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(string predicate, string orderBy)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        internal override Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Query

        #region Insert
        internal override Task<TEntity> ExecuteInsertEntityAsync(TEntity entity)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Insert

        #region Update
        internal override Task<TEntity> ExecuteUpdateEntityAsync(TEntity entity)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Update

        #region Delete
        internal override Task ExecuteDeleteEntityAsync(TEntity entity)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Delete
    }
}
//MdEnd