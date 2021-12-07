//@BaseCode
using Microsoft.EntityFrameworkCore;
using SmartNQuick.Contracts;
using SmartNQuick.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.DataContext
{
    internal interface IContext : IDisposable
    {
        DbSet<E> Set<C, E>()
            where C : IIdentifiable
            where E : IdentityEntity, C;
        IQueryable<E> QueryableSet<C, E>()
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<int> CountAsync<C, E>()
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<int> CountByAsync<C, E>(string predicate)
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<E> GetByIdAsync<C, E>(int id)
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<IEnumerable<E>> GetAllAsync<C, E>()
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<IEnumerable<E>> QueryAllAsync<C, E>(string predicate)
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<IEnumerable<E>> QueryAllAsync<C, E>(string predicate, string orderBy)
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<E> InsertAsync<C, E>(E entity)
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<E> UpdateAsync<C, E>(E entity)
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task DeleteAsync<C, E>(int id)
            where C : IIdentifiable
            where E : IdentityEntity, C;

        Task<int> SaveChangesAsync();
        Task<int> RejectChangesAsync();
    }
}
