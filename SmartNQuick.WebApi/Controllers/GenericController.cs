//@BaseCode
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.WebApi.Controllers
{
	public abstract class GenericController<I, M> : ApiControllerBase, IDisposable
        where I : Contracts.IIdentifiable
        where M : Transfer.Models.IdentityModel, I, Contracts.ICopyable<I>, new()
    {
		private bool disposedValue;

		protected GenericController()
		{
		}

#if ACCOUNT_ON
		protected async Task<Contracts.Client.IControllerAccess<I>> CreateControllerAsync()
		{
			var result = Logic.Factory.Create<I>();
			var sessionToken = await GetSessionTokenAsync().ConfigureAwait(false);

			if (sessionToken.HasContent())
			{
				result.SessionToken = sessionToken;
			}
			return result;
		}
#else
		protected Task<Contracts.Client.IControllerAccess<I>> CreateControllerAsync()
		{
			return Task.Run(() => Logic.Factory.Create<I>());
		}
#endif
		protected M ToModel(I entity)
		{
			var result = new M();

			result.CopyProperties(entity);
			return result;
		}

		[HttpGet("/api/[controller]/MaxPageSize")]
		public async Task<int> GetMaxPageSizeAsync()
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);

			return ctrl.MaxPageSize;
		}

		[HttpGet("/api/[controller]/Count")]
		public async Task<int> GetCountAsync()
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);

			return await ctrl.CountAsync().ConfigureAwait(false);
		}
		[HttpGet("/api/[controller]/Count/{predicate}")]
		public async Task<int> GetCountByAsync(string predicate)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);

			return await ctrl.CountByAsync(predicate).ConfigureAwait(false);
		}

		[HttpGet("/api/[controller]/{id}")]
		public async Task<M> GetByIdAsync(int id)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return ToModel(result);
		}
		[HttpGet("/api/[controller]")]
		public async Task<IEnumerable<M>> GetAllAsync()
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.GetAllAsync().ConfigureAwait(false);

			return result.Select(e => ToModel(e));
		}
		[HttpGet("/api/[controller]/{index}/{size}")]
		public async Task<IEnumerable<M>> GetPageListAsync(int index, int size)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.GetPageListAsync(index, size).ConfigureAwait(false);

			return result.Select(e => ToModel(e));
		}

		[HttpGet("/api/[controller]/Query/{predicate}")]
		public async Task<IEnumerable<M>> QueryAllBy(string predicate)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.QueryAllAsync(predicate).ConfigureAwait(false);

			return result.Select(e => ToModel(e));
		}
		[HttpGet("/api/[controller]/{predicate}/{index}/{size}")]
		public async Task<IEnumerable<M>> QueryPageListAsync(string predicate, int index, int size)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.QueryPageListAsync(predicate, index, size).ConfigureAwait(false);

			return result.Select(e => ToModel(e));
		}

		[HttpGet("/api/[controller]/Create")]
		public async Task<M> CreateAsync()
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.CreateAsync().ConfigureAwait(false);

			return ToModel(result);
		}

		[HttpPost("/api/[controller]")]
		public async Task<M> PostAsync([FromBody] M model)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.InsertAsync(model).ConfigureAwait(false);

			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			return ToModel(result);
		}
		[HttpPost("/api/[controller]/Array")]
		public async Task<IQueryable<M>> PostArrayAsync(IEnumerable<M> models)
		{
			var result = new List<M>();
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.InsertAsync(models).ConfigureAwait(false);

			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			result.AddRange(entities.Select(e => ToModel(e)));
			return result.AsQueryable();
		}

		[HttpPut("/api/[controller]")]
		public async Task<M> PutAsync([FromBody]M model)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.UpdateAsync(model).ConfigureAwait(false);

			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			return ToModel(result);
		}
		[HttpPut("/api/[controller]/Array")]
		public async Task<IQueryable<M>> PutArrayAsync(IEnumerable<M> models)
		{
			var result = new List<M>();
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.UpdateAsync(models).ConfigureAwait(false);

			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			result.AddRange(entities.Select(e => ToModel(e)));
			return result.AsQueryable();
		}

		[HttpDelete("/api/[controller]/{id}")]
		public async Task DeleteAsync(int id)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);

			await ctrl.DeleteAsync(id).ConfigureAwait(false);
			await ctrl.SaveChangesAsync().ConfigureAwait(false);
		}

		#region Disposable pattern
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~GenericController()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion Disposable pattern
	}
}
