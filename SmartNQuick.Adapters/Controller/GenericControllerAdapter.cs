﻿//@BaseCode
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
        partial void Constructing();
        partial void Constructed();

#if ACCOUNT_ON
        public GenericControllerAdapter(string sessionToken)
        {
            Constructing();
            controller = Logic.Factory.Create<TContract>(sessionToken);
            Constructed();
        }

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
        public Task<IEnumerable<TContract>> GetAllAsync(string orderBy)
        {
            return controller.GetAllAsync(orderBy);
        }

        public async Task<IEnumerable<TContract>> GetPageListAsync(int pageIndex, int pageSize)
        {
            return await controller.GetPageListAsync(pageIndex, pageSize).ConfigureAwait(false);
        }
        public async Task<IEnumerable<TContract>> GetPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            return await controller.GetPageListAsync(orderBy, pageIndex, pageSize).ConfigureAwait(false);
        }

        public Task<IEnumerable<TContract>> QueryAllAsync(string predicate)
        {
            return controller.QueryAllAsync(predicate);
        }
        public Task<IEnumerable<TContract>> QueryAllAsync(string predicate, string orderBy)
        {
            return controller.QueryAllAsync(predicate, orderBy);
        }

        public async Task<IEnumerable<TContract>> QueryPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            return (await controller.QueryPageListAsync(predicate, pageIndex, pageSize).ConfigureAwait(false)).ToArray();
        }
        public async Task<IEnumerable<TContract>> QueryPageListAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            return (await controller.QueryPageListAsync(predicate, orderBy, pageIndex, pageSize).ConfigureAwait(false)).ToArray();
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