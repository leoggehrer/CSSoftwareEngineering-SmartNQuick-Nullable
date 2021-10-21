//@BaseCode
using CommonBase.Extensions;
using Microsoft.EntityFrameworkCore;
using SmartNQuick.Contracts;
using SmartNQuick.Logic.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.DataContext
{
    internal partial class SmartNQuickDbContext : DbContext, IContext
	{
		static SmartNQuickDbContext()
		{
			ClassConstructing();
			ConnectionString = CommonBase.Modules.Configuration.AppSettings.Configuration[StaticLiterals.EnvironmentConnectionStringKey];
			if (ConnectionString.IsNullOrEmpty())
			{
				ConnectionString = CommonBase.Modules.Configuration.AppSettings.Configuration[StaticLiterals.AppSettingsConnectionStringKey];
			}
			if (ConnectionString.IsNullOrEmpty())
			{
				ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=SmartNQuickDb;Integrated Security=True";
			}
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();

		public SmartNQuickDbContext()
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();

		public static string ConnectionString { get; protected set; }

		public DbSet<E> Set<C, E>()
			where C : IIdentifiable
			where E : IdentityEntity, C
		{
			DbSet<E> result = null;

			GetDbSet<C, E>(ref result);	
			
			return result;
		}
		IQueryable<E> IContext.QueryableSet<C, E>()
		{
			DbSet<E> result = null;

			GetDbSet<C, E>(ref result);

			return result;
		}

		partial void GetDbSet<C, E>(ref DbSet<E> dbset) where E : class;

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

		public Task<int> SaveChangesAsync()
		{
			return base.SaveChangesAsync();
		}
		public Task<int> RejectChangesAsync()
		{
			return Task.Run(() =>
			{
				int count = 0;

				foreach (var entry in ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList())
				{
					switch (entry.State)
					{
						case EntityState.Modified:
							count++;
							entry.CurrentValues.SetValues(entry.OriginalValues);
							entry.State = EntityState.Unchanged;
							break;
						case EntityState.Added:
							count++;
							entry.State = EntityState.Detached;
							break;
						case EntityState.Deleted:
							count++;
							entry.State = EntityState.Unchanged;
							break;
					}
				}
				return count;
			});
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			bool handled = false;

			BeforeOnConfiguring(optionsBuilder, ref handled);
			if (handled == false)
			{
				optionsBuilder.UseSqlServer(ConnectionString);
			}
			AfterOnConfiguring(optionsBuilder);

			base.OnConfiguring(optionsBuilder);
		}

		partial void BeforeOnConfiguring(DbContextOptionsBuilder optionsBuilder, ref bool handled);
		partial void AfterOnConfiguring(DbContextOptionsBuilder optionsBuilder);

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			bool handled = false;

			BeforeOnModelCreating(modelBuilder, ref handled);
			if (handled == false)
			{
				DoModelCreating(modelBuilder);
			}
			AfterOnModelCreating(modelBuilder);
			var cascadeFKs = modelBuilder.Model.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

			foreach (var fk in cascadeFKs)
				fk.DeleteBehavior = DeleteBehavior.Restrict;

			base.OnModelCreating(modelBuilder);
		}
		static partial void BeforeOnModelCreating(ModelBuilder modelBuilder, ref bool handled);
		static partial void DoModelCreating(ModelBuilder modelBuilder);
		static partial void AfterOnModelCreating(ModelBuilder modelBuilder);
	}
}
