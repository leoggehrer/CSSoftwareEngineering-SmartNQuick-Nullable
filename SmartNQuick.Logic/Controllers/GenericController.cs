//@BaseCode
using CommonBase.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers
{
    internal abstract partial class GenericController<C, E> : ControllerObject, SmartNQuick.Contracts.Client.IControllerAccess<C>
		where C : SmartNQuick.Contracts.IIdentifiable
		where E : Entities.IdentityEntity, C, new()
	{
		protected GenericController(DataContext.IContext context) : base(context)
		{
		}

		protected GenericController(ControllerObject other) : base(other)
		{
		}

		public abstract Task<int> CountAsync();

		public abstract Task<int> CountByAsync(string predicate);

		public virtual Task<C> CreateAsync()
		{
			return Task.Factory.StartNew<C>(() => new E());
		}

		public abstract Task DeleteAsync(int id);

		public abstract Task<IEnumerable<C>> GetAllAsync(); 

		public abstract Task<C> GetByIdAsync(int id); 
		public abstract Task<IEnumerable<C>> QueryAllAsync(string predicate); 

		public abstract Task<C> InsertAsync(C entity);
		public virtual async Task<IEnumerable<C>> InsertAsync(IEnumerable<C> entities)
		{
			entities.CheckArgument(nameof(entities));

			var result = new List<C>();

			foreach (var entity in entities)
			{
				result.Add(await InsertAsync(entity).ConfigureAwait(false));
			}
			return result.AsQueryable();
		}

		public abstract Task<C> UpdateAsync(C entity);
		public virtual async Task<IEnumerable<C>> UpdateAsync(IEnumerable<C> entities)
		{
			entities.CheckArgument(nameof(entities));

			var result = new List<C>();

			foreach (var entity in entities)
			{
				result.Add(await UpdateAsync(entity).ConfigureAwait(false));
			}
			return result.AsQueryable();
		}

		public virtual Task<int> SaveChangesAsync()
		{
			return Context.SaveChangesAsync();
		}
		public virtual Task<int> RejectChangesAsync()
		{
			return Context.RejectChangesAsync();
		}
	}
}