//@BaseCode
//MdStart
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Business
{
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

        public override Task<int> CountAsync()
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        public override Task<int> CountByAsync(string predicate)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #region Async-Methods
        public override Task<C> GetByIdAsync(int id)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        public override Task<IEnumerable<C>> GetAllAsync()
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        public override Task<IEnumerable<C>> QueryAllAsync(string predicate)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        public override Task<C> CreateAsync()
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        public override Task<C> InsertAsync(C entity)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        public override Task<C> UpdateAsync(C entity)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }

        public override Task DeleteAsync(int id)
        {
            throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetAsyncOriginal()}!");
        }
        #endregion Async-Methods
    }
}
//MdEnd