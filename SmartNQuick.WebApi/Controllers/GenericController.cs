//@BaseCode
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.WebApi.Controllers
{
    public abstract class GenericController<TContract, TModel> : ApiControllerBase, IDisposable
        where TContract : Contracts.IIdentifiable
        where TModel : Transfer.Models.IdentityModel, TContract, Contracts.ICopyable<TContract>, new()
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
		protected Task<Contracts.Client.IControllerAccess<TContract>> CreateControllerAsync()
		{
			return Task.Run(() => Logic.Factory.Create<TContract>());
		}
#endif
		protected TModel ToModel(TContract entity)
		{
			var result = new TModel();

			result.CopyProperties(entity);
			return result;
		}

		[HttpGet("/api/[controller]/MaxPageSize")]
		public async Task<int> GetMaxPageSizeAsync()
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);

			return ctrl.MaxPageSize;
		}

        #region Get actions
        [HttpGet("/api/[controller]/Count")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<int>> GetCountAsync()
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.CountAsync().ConfigureAwait(false);

			return Ok(result);
		}
		[HttpGet("/api/[controller]/Count/{predicate}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<int>> GetCountByAsync(string predicate)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var result = await ctrl.CountByAsync(predicate).ConfigureAwait(false);

			return Ok(result);
		}
		[HttpGet("/api/[controller]/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<TModel>> GetByIdAsync(int id)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			return entity == null ? NotFound() : Ok(ToModel(entity));
		}
		/// <summary>
		/// Gets a list of models
		/// </summary>
		/// <returns>List of models</returns>
		[HttpGet("/api/[controller]")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<TModel>>> GetAllAsync()
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

			return Ok(entities.Select(e => ToModel(e)));
		}
		[HttpGet("/api/[controller]/Sorted/{orderBy}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<TModel>>> GetAllAsync(string orderBy)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.GetAllAsync(orderBy).ConfigureAwait(false);

			return Ok(entities.Select(e => ToModel(e)));
		}

		[HttpGet("/api/[controller]/{index}/{size}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<TModel>>> GetPageListAsync(int index, int size)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.GetPageListAsync(index, size).ConfigureAwait(false);

			return Ok(entities.Select(e => ToModel(e)));
		}
		[HttpGet("/api/[controller]/Sorted/{orderBy}/{index}/{size}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<TModel>>> GetPageListAsync(int index, int size, string orderBy)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.GetPageListAsync(orderBy, index, size).ConfigureAwait(false);

			return Ok(entities.Select(e => ToModel(e)));
		}
        #endregion Get actions

        #region Query actions
        [HttpGet("/api/[controller]/Query/{predicate}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<TModel>>> QueryAllAsync(string predicate)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.QueryAllAsync(predicate).ConfigureAwait(false);

			return Ok(entities.Select(e => ToModel(e)));
		}
		[HttpGet("/api/[controller]/Sorted/Query/{predicate}/{orderBy}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<TModel>>> QueryAllAync(string predicate, string orderBy)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.QueryAllAsync(predicate, orderBy).ConfigureAwait(false);

			return Ok(entities.Select(e => ToModel(e)));
		}

		[HttpGet("/api/[controller]/Query/{predicate}/{index}/{size}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<TModel>>> QueryPageListAsync(string predicate, int index, int size)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.QueryPageListAsync(predicate, index, size).ConfigureAwait(false);

			return Ok(entities.Select(e => ToModel(e)));
		}
		[HttpGet("/api/[controller]/Sorted/Query/{predicate}/{orderBy}/{index}/{size}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<TModel>>> QueryPageListAsync(string predicate, int index, int size, string orderBy)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.QueryPageListAsync(predicate, orderBy, index, size).ConfigureAwait(false);

			return Ok(entities.Select(e => ToModel(e)));
		}
		#endregion Query actions

		[HttpGet("/api/[controller]/Create")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<TModel>> CreateAsync()
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entity = await ctrl.CreateAsync().ConfigureAwait(false);

			return Ok(ToModel(entity));
		}

		/// <summary>
		/// Adds a model.
		/// </summary>
		/// <param name="model">Model to add</param>
		/// <returns>Data about the created model (including primary key)</returns>
		/// <response code="201">Model created</response>
		[HttpPost("/api/[controller]")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<TModel>> PostAsync([FromBody] TModel model)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entity = await ctrl.InsertAsync(model).ConfigureAwait(false);

			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			return CreatedAtAction("GetById", new { id = entity.Id }, ToModel(entity));
		}
		[HttpPost("/api/[controller]/Array")]
		public async Task<IQueryable<TModel>> PostArrayAsync(IEnumerable<TModel> models)
		{
			var result = new List<TModel>();
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.InsertAsync(models).ConfigureAwait(false);

			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			result.AddRange(entities.Select(e => ToModel(e)));
			return result.AsQueryable();
		}

		/// <summary>
		/// Updates a model
		/// </summary>
		/// <param name="id">Id of the model to update</param>
		/// <param name="model">Data to update</param>
		/// <returns>Data about the updated model</returns>
		/// <response code="200">Model updated</response>
		/// <response code="404">Model not found</response>
		[HttpPut("/api/[controller]/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<TModel>> PutAsync(int id, [FromBody]TModel model)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			if (entity != null && model != null)
            {
				entity.CopyFrom(model);
				await ctrl.UpdateAsync(entity).ConfigureAwait(false);
				await ctrl.SaveChangesAsync().ConfigureAwait(false);
			}
			return entity == null ? NotFound() : Ok(ToModel(entity));
		}
		[HttpPut("/api/[controller]/Array")]
		public async Task<IQueryable<TModel>> PutArrayAsync(IEnumerable<TModel> models)
		{
			var result = new List<TModel>();
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entities = await ctrl.UpdateAsync(models).ConfigureAwait(false);

			await ctrl.SaveChangesAsync().ConfigureAwait(false);
			result.AddRange(entities.Select(e => ToModel(e)));
			return result.AsQueryable();
		}

		/// <summary>
		/// Delete a single model by Id
		/// </summary>
		/// <param name="id">Id of the model to delete</param>
		/// <response code="204">Model deleted</response>
		/// <response code="404">Model not found</response>
		[HttpDelete("/api/[controller]/{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteAsync(int id)
		{
			using var ctrl = await CreateControllerAsync().ConfigureAwait(false);
			var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

			if (entity != null)
            {
				await ctrl.DeleteAsync(entity.Id).ConfigureAwait(false);
				await ctrl.SaveChangesAsync().ConfigureAwait(false);
			}
			return entity == null ? NotFound() : NoContent();
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
