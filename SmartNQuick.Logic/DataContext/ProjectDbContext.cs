//@BaseCode
using Microsoft.EntityFrameworkCore;
using SmartNQuick.Contracts;
using SmartNQuick.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.DataContext
{
	internal partial class ProjectDbContext : DbContext, Contracts.IContext
	{
		public ProjectDbContext()
		{
		}

		public DbSet<E> Set<C, E>()
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			throw new NotImplementedException();
		}

		public Task<int> CountAsync<C, E>()
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			return Set<E>().CountAsync();
		}
		public Task<int> CountByAsync<C, E>(string predicate)
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			return Set<E>().Where(predicate).CountAsync();
		}

		public Task<E> GetByIdAsync<C, E>(int id)
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			return Set<C, E>().FindAsync(id).AsTask();
		}
		public async Task<IEnumerable<E>> GetAllAsync<C, E>()
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			return await Set<C, E>().ToArrayAsync().ConfigureAwait(false);
		}

		public async Task<IEnumerable<E>> QueryAllAsync<C, E>(string predicate)
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			return await Set<C, E>().Where(predicate).ToArrayAsync();
		}

		public async Task<E> InsertAsync<C, E>(E entity)
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			await Set<C, E>().AddAsync(entity).ConfigureAwait(false);

			return entity;
		}

		public Task DeleteAsync<C, E>(int id)
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			return Task.Run(() =>
			{
				E result = Set<E>().SingleOrDefault(i => i.Id == id);

				if (result != null)
				{
					Set<C, E>().Remove(result);
				}
			});
		}

		public Task<E> UpdateAsync<C, E>(E entity)
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			return Task.Run(() =>
			{
				Set<C, E>().Update(entity);
				return entity;
			});
		}

		public Task<int> SaveChangesAsync()
		{
			return SaveChangesAsync();
		}

	}
}
