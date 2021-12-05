//@BaseCode
//MdStart
#if ACCOUNT_ON
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartNQuick.AspMvc.Models;
using SmartNQuick.AspMvc.Models.Business.Account;
using SmartNQuick.AspMvc.Models.Modules.Common;
using SmartNQuick.AspMvc.Models.Persistence.Account;
using SmartNQuick.AspMvc.Modules.View;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Controllers.Business.Account
{
    partial class AppAccessesController
    {
        private IEnumerable<Role> roles = null;
        protected override async Task<AppAccess> BeforeViewAsync(AppAccess model, ActionMode action)
        {
            var viewBagInfo = new ViewBagWrapper(ViewBag);

            viewBagInfo.CommandMode ^= CommandMode.EditDetail;
            if (action == ActionMode.CreateDetail)
            {
                roles = await LoadRolesAsync(model).ConfigureAwait(false);
            }
            else if (action == ActionMode.Details)
            {
                roles = await LoadRolesAsync(model).ConfigureAwait(false);

                if (roles.All(r => r.Assigned))
                {
                    viewBagInfo.CommandMode ^= CommandMode.CreateDetail;
                }
                viewBagInfo.IgnoreNames.Add(nameof(Role.Description));
            }
            return await base.BeforeViewAsync(model, action).ConfigureAwait(false);
        }

        protected override MasterDetailModel BeforeViewMasterDetail(MasterDetailModel model, ActionMode action)
        {
            if (action == ActionMode.CreateDetail && roles != null && model.Detail is Role role)
            {
                role.AssignedRoles = roles.Where(r => r.Assigned == false).OrderBy(e => e.Designation).ToArray();
            }
            return base.BeforeViewMasterDetail(model, action);
        }

        #region Helpers
        private async Task<IEnumerable<Role>> LoadRolesAsync(AppAccess model)
        {
            model.CheckArgument(nameof(model));

            using var ctrlRole = Adapters.Factory.Create<Contracts.Persistence.Account.IRole>(SessionWrapper.SessionToken);
            var roles = await ctrlRole.GetAllAsync().ConfigureAwait(false);
            var result = new List<Role>();

            foreach (var item in roles)
            {
                var role = new Role();

                role.CopyProperties(item);
                role.Assigned = model.ManyModels.Any(r => r.Id == item.Id);
                result.Add(role);
            }
            return result;
        }
        #endregion Helpers
    }
}
#endif
//MdEnd
