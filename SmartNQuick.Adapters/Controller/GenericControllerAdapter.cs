//@BaseCode
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.Adapters.Controller
{
    internal partial class GenericControllerAdapter<TContract> : Contracts.Client.IAdapterAccess<TContract>
        where TContract : Contracts.IIdentifiable
    {
        static GenericControllerAdapter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public Contracts.Client.IControllerAccess<TContract> controller;

        public GenericControllerAdapter()
        {
            Constructing();
            controller = Logic.Factory.Create<TContract>();
            Constructed();
        }
#if ACCOUNT_ON
        public GenericControllerAdapter(string sessionToken)
        {
            Constructing();
            controller = Logic.Factory.Create<TContract>(sessionToken);
            Constructed();
        }
#endif
        partial void Constructing();
        partial void Constructed();

#if ACCOUNT_ON
        public string SessionToken { set => controller.SessionToken = value; }
#endif
        public int MaxPageSize => 500;

        #region Async-Methods
        public Task<int> CountAsync()
        {
            return controller.CountAsync();
        }
        public Task<int> CountByAsync(string predicate)
        {
            return controller.CountByAsync(predicate);
        }

        public Task<TContract> GetByIdAsync(int id)
        {
            return controller.GetByIdAsync(id);
        }
        public Task<IEnumerable<TContract>> GetAllAsync()
        {
            return controller.GetAllAsync();
        }

        public Task<IEnumerable<TContract>> QueryAllAsync(string predicate)
        {
            return controller.QueryAllAsync(predicate);
        }

        public Task<TContract> CreateAsync()
        {
            return controller.CreateAsync();
        }

        public async Task<TContract> InsertAsync(TContract entity)
        {
            var result = await controller.InsertAsync(entity).ConfigureAwait(false);

            await SaveChangesAsync().ConfigureAwait(false);
            return result;
        }
        public async Task<IQueryable<TContract>> InsertAsync(IEnumerable<TContract> entities)
        {
            var result = await controller.InsertAsync(entities).ConfigureAwait(false);

            await SaveChangesAsync().ConfigureAwait(false);
            return result.AsQueryable();
        }
        public async Task<TContract> UpdateAsync(TContract entity)
        {
            var result = await controller.UpdateAsync(entity).ConfigureAwait(false);

            await SaveChangesAsync().ConfigureAwait(false);
            return result;
        }
        public async Task<IQueryable<TContract>> UpdateAsync(IEnumerable<TContract> entities)
        {
            var result = await controller.UpdateAsync(entities).ConfigureAwait(false);

            await SaveChangesAsync().ConfigureAwait(false);
            return result.AsQueryable();
        }

        public async Task DeleteAsync(int id)
        {
            await controller.DeleteAsync(id).ConfigureAwait(false);
            await SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                await controller.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                await controller.RejectChangesAsync().ConfigureAwait(false);
                throw;
            }
        }

        #endregion Async-Methods

        public void Dispose()
        {
            controller?.Dispose();
            controller = null;
        }
    }
}
//MdEnd