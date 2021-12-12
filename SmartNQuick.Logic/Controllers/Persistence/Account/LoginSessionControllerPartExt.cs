//@BaseCode
//MdStart
#if ACCOUNT_ON
using Microsoft.EntityFrameworkCore;
using SmartNQuick.Logic.Entities.Persistence.Account;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Persistence.Account
{
    partial class LoginSessionController
	{
		protected override Task<LoginSession> BeforeInsertAsync(LoginSession entity)
		{
			entity.SessionToken = Guid.NewGuid().ToString();
			entity.LoginTime = entity.LastAccess = DateTime.UtcNow;

			return base.BeforeInsertAsync(entity);
		}

		public Task<LoginSession[]> QueryOpenLoginSessionsAsync()
		{
			return QueryableSet().Where(e => e.LogoutTime.HasValue == false)
								 .Include(e => e.Identity)
								 .ToArrayAsync();
		}
	}
}
#endif
//MdEnd