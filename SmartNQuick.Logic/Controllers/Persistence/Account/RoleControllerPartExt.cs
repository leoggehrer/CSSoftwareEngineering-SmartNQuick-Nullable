//@BaseCode
//MdStart
#if ACCOUNT_ON
using SmartNQuick.Logic.Entities.Persistence.Account;
using System.Text;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Persistence.Account
{
    partial class RoleController
    {
        protected override Task<Role> BeforeInsertUpdateAsync(Role entity)
        {
            entity.Designation = ClearRoleDesignation(entity.Designation);

            return base.BeforeInsertUpdateAsync(entity);
        }
        public static string ClearRoleDesignation(string name)
        {
            StringBuilder result = new StringBuilder();

            if (name.HasContent())
            {
                foreach (var item in name)
                {
                    if (char.IsLetter(item) || char.IsDigit(item))
                    {
                        result.Append(result.Length == 0 ? char.ToUpper(item) : item);
                    }
                }
            }
            return result.Length > 0 ? result.ToString() : null;
        }
    }
}
#endif
//MdEnd