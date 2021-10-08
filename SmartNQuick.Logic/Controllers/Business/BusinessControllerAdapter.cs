//@BaseCode
//MdStart
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

namespace SmartNQuick.Logic.Controllers.Business
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class BusinessControllerAdapter<C, E> : GenericController<C, E>
        where C : Contracts.IIdentifiable
        where E : Entities.IdentityEntity, C, Contracts.ICopyable<C>, new()
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
        public override Task<int> CountAsync()
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        public override Task<int> CountByAsync(string predicate)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Count

        #region Query
        internal override Task<E> ExecuteGetEntityByIdAsync(int id)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<IEnumerable<E>> ExecuteGetEntityAllAsync()
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        internal override Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(string predicate)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Query

        #region Insert
        internal override Task<E> ExecuteInsertEntityAsync(E entity)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Insert

        #region Update
        internal override Task<E> ExecuteUpdateEntityAsync(E entity)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Update

        #region Delete
        internal override Task ExecuteDeleteEntityAsync(E entity)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Delete
    }
}
//MdEnd