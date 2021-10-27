//@BaseCode
//MdStart
#if ACCOUNT_ON
using CommonBase.Extensions;
using Microsoft.EntityFrameworkCore;
using SmartNQuick.Logic.Entities.Persistence.Account;
using SmartNQuick.Logic.Modules.Account;
using SmartNQuick.Logic.Modules.Exception;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Persistence.Account
{
    internal partial class IdentityController
    {
        private static void CheckInsertEntity(Identity entity)
        {
            if (AccountManager.CheckMailAddressSyntax(entity.Email) == false)
            {
                throw new LogicException(ErrorType.InvalidEmail);
            }
            if (AccountManager.CheckPasswordSyntax(entity.Password) == false)
            {
                throw new LogicException(ErrorType.InvalidPassword);
            }
        }
        private static void CheckUpdateEntity(Identity entity)
        {
            if (AccountManager.CheckMailAddressSyntax(entity.Email) == false)
            {
                throw new LogicException(ErrorType.InvalidEmail);
            }
            if (entity.Password.HasContent())
            {
                if (AccountManager.CheckPasswordSyntax(entity.Password) == false)
                {
                    throw new LogicException(ErrorType.InvalidPassword);
                }
            }
        }

        protected override Task<Identity> BeforeInsertAsync(Identity entity)
        {
            CheckInsertEntity(entity);

            var (Hash, Salt) = AccountManager.CreatePasswordHash(entity.Password);

            entity.PasswordHash = Hash;
            entity.PasswordSalt = Salt;
            entity.Guid = System.Guid.NewGuid().ToString();

            return base.BeforeInsertAsync(entity);
        }
        protected override async Task<Identity> BeforeUpdateAsync(Identity entity)
        {
            CheckUpdateEntity(entity);

            var oldEntity = await GetEntityByIdAsync(entity.Id).ConfigureAwait(false);

            if (oldEntity != null)
            {
                entity.Guid = oldEntity.Guid;
            }

            if (entity.Password.HasContent())
            {
                var (Hash, Salt) = AccountManager.CreatePasswordHash(entity.Password);

                entity.PasswordHash = Hash;
                entity.PasswordSalt = Salt;
            }
            return await base.BeforeUpdateAsync(entity).ConfigureAwait(false);
        }

        public Task<Identity> GetValidIdentityByEmail(string email)
		{
            return QueryableSet().Include(e => e.IdentityXRoles)
                                 .ThenInclude(e => e.Role)
                                 .FirstOrDefaultAsync(e => e.State == Contracts.Modules.Common.State.Active
                                                        && e.AccessFailedCount < 4
                                                        && e.Email.ToLower() == email.ToLower());
		}
    }
}
#endif
//MdEnd