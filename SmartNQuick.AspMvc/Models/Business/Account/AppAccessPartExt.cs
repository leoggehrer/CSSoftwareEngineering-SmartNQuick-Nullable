//@BaseCode
//MdStart
#if ACCOUNT_ON
using System;

namespace SmartNQuick.AspMvc.Models.Business.Account
{
    public partial class AppAccess
    {
        private static char RoleSeparator => ',';
        public string RoleList
        {
            get
            {
                string result = string.Empty;

                foreach (var item in ManyItems)
                {
                    if (result.Length > 0)
                        result += RoleSeparator;

                    result += item.Designation;
                }
                return result;
            }
            set
            {
                var values = value != null ? value.Split(RoleSeparator) : Array.Empty<string>();

                ClearManyItems();
                foreach (var item in values)
                {
                    var role = CreateManyItem();

                    role.Designation = item;
                    AddManyItem(role);
                }
            }
        }
    }
}
#endif