//@BaseCode
//MdStart
/*
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Model = SmartNQuick.AspMvc.Models.Business.Account.AppAccess;
using Contract = SmartNQuick.Contracts.Business.Account.IAppAccess;
using SmartNQuick.AspMvc.Models.Persistence.Account;
using Microsoft.AspNetCore.Http;

namespace SmartNQuick.AspMvc.Controllers.Business.Account
{
    public partial class AppAccessesController
    {
        public AppAccessesController()
            : base()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync(string error = null)
        {
            using var ctrl = CreateController();
            var entity = await ctrl.CreateAsync().ConfigureAwait(false);
            var model = ToModel(entity);

            model.ActionError = error;
            await LoadRolesAsync(model).ConfigureAwait(false);
            return View("Edit", model);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync(int id, string error = null)
        {
            using var ctrl = CreateController();
            using var ctrlRole = Factory.Create<Contracts.Persistence.Account.IRole>(SessionWrapper.SessionToken);
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
            var model = ConvertTo<Model, Contract>(entity);

            model.ActionError = error;
            await LoadRolesAsync(model).ConfigureAwait(false);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync(int id, Identity identityModel, IFormCollection formCollection)
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            async Task<IActionResult> CreateFailedAsync(Identity identity, string error)
            {
                var entity = await ctrl.CreateAsync().ConfigureAwait(false);

                entity.FirstItem.CopyProperties(identity);

                var model = ConvertTo<Model, Contract>(entity);

                model.ActionError = error;
                await LoadRolesAsync(model).ConfigureAwait(false);
                return View("Edit", model);
            }
            async Task<IActionResult> EditFailedAsync(Identity identity, string error)
            {
                var entity = await ctrl.GetByIdAsync(identity.Id).ConfigureAwait(false);

                entity.FirstItem.CopyProperties(identity);

                var model = ConvertTo<Model, Contract>(entity);

                model.ActionError = error;
                await LoadRolesAsync(model).ConfigureAwait(false);
                return View("Edit", model);
            }
            async Task UpdateRolesAsync(Model model)
            {
                using var ctrlRole = Factory.Create<Contracts.Persistence.Account.IRole>(SessionWrapper.SessionToken);
                var roles = await ctrlRole.GetAllAsync().ConfigureAwait(false);

                model.ClearSecondItems();
                foreach (var item in formCollection.Where(l => l.Key.StartsWith("Assigned")))
                {
                    var roleId = item.Key.ToInt();
                    var role = roles.SingleOrDefault(r => r.Id == roleId);

                    if (role != null)
                    {
                        model.AddSecondItem(role);
                    }
                }
            }

            if (ModelState.IsValid == false)
            {
                if (identityModel.Id == 0)
                {
                    return await CreateFailedAsync(identityModel, GetModelStateError()).ConfigureAwait(false);
                }
                else
                {
                    return await EditFailedAsync(identityModel, GetModelStateError()).ConfigureAwait(false);
                }
            }
            try
            {
                if (identityModel.Id == 0)
                {
                    var entity = await ctrl.CreateAsync().ConfigureAwait(false);

                    entity.FirstItem.CopyProperties(identityModel);
                    var model = ConvertTo<Model, Contract>(entity);

                    await UpdateRolesAsync(model).ConfigureAwait(false);
                    var result = await ctrl.InsertAsync(model).ConfigureAwait(false);

                    id = result.Id;
                }
                else
                {
                    var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

                    var model = ConvertTo<Model, Contract>(entity);
                    model.FirstItem.CopyProperties(identityModel);
                    await UpdateRolesAsync(model).ConfigureAwait(false);
                    await ctrl.UpdateAsync(model).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                if (identityModel.Id == 0)
                {
                    return await CreateFailedAsync(identityModel, GetExceptionError(ex)).ConfigureAwait(false);
                }
                else
                {
                    return await EditFailedAsync(identityModel, GetExceptionError(ex)).ConfigureAwait(false);
                }
            }
            return RedirectToAction("Edit", new { id });
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

            return View(entity != null ? ConvertTo<Model, Contract>(entity) : entity);
        }

        // GET: /Delete/5
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(int id, string error = null)
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            using var ctrlRole = Factory.Create<Contracts.Persistence.Account.IRole>(SessionWrapper.SessionToken);
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
            var model = ConvertTo<Model, Contract>(entity);

            model.ActionError = error;
            return View(model);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            try
            {
                using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);

                await ctrl.DeleteAsync(id).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Delete", new { error = GetExceptionError(ex) });
            }
        }


        #region Export and Import
        protected override string[] CsvHeader => new string[] { "Id", "FirstItem.Name", "FirstItem.Email", "FirstItem.Password", "FirstItem.AccessFailedCount", "FirstItem.EnableJwtAuth", "RoleList" };

        [ActionName("Export")]
        public async Task<FileResult> ExportAsync()
        {
            var fileName = "AppAccess.csv";
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            var entities = (await ctrl.GetAllAsync().ConfigureAwait(false)).Select(e => ConvertTo<Model, Contract>(e));

            return ExportDefault(CsvHeader, entities, fileName);
        }

        [ActionName("Import")]
        public ActionResult ImportAsync(string error = null)
        {
            var model = new Models.Modules.Export.ImportProtocol() { BackController = ControllerName, ActionError = error };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Import")]
        public async Task<IActionResult> ImportAsync()
        {
            var index = 0;
            var model = new Models.Modules.Export.ImportProtocol() { BackController = ControllerName };
            var logInfos = new List<Models.Modules.Export.ImportLog>();
            var importModels = ImportDefault<Model>(CsvHeader);
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);

            foreach (var item in importModels)
            {
                index++;
                try
                {
                    if (item.Action == Models.Modules.Export.ImportAction.Insert)
                    {
                        var entity = await ctrl.CreateAsync();

                        CopyModels(CsvHeader, item.Model, entity);
                        item.Model.SecondEntities.ForEach(e => entity.AddSecondItem(e));
                        await ctrl.InsertAsync(entity);
                    }
                    else if (item.Action == Models.Modules.Export.ImportAction.Update)
                    {
                        var entity = await ctrl.GetByIdAsync(item.Id);

                        CopyModels(CsvHeader, item.Model, entity);
                        entity.ClearSecondItems();
                        item.Model.SecondEntities.ForEach(e => entity.AddSecondItem(e));

                        await ctrl.UpdateAsync(entity);
                    }
                    else if (item.Action == Models.Modules.Export.ImportAction.Delete)
                    {
                        await ctrl.DeleteAsync(item.Id);
                    }
                    logInfos.Add(new Models.Modules.Export.ImportLog
                    {
                        IsError = false,
                        Prefix = $"Line: {index} - {item.Action}",
                        Text = "OK",
                    });
                }
                catch (Exception ex)
                {
                    logInfos.Add(new Models.Modules.Export.ImportLog
                    {
                        IsError = true,
                        Prefix = $"Line: {index} - {item.Action}",
                        Text = ex.Message,
                    });
                }
            }
            model.LogInfos = logInfos;
            return View(model);
        }
        #endregion Export and Import

        #region Helpers
        private async Task LoadRolesAsync(Model model)
        {
            model.CheckArgument(nameof(model));

            using var ctrlRole = Factory.Create<Contracts.Persistence.Account.IRole>(SessionWrapper.SessionToken);
            var roles = await ctrlRole.GetAllAsync().ConfigureAwait(false);

            foreach (var item in roles)
            {
                var assigned = model.SecondEntities.SingleOrDefault(r => r.Id == item.Id);

                if (assigned != null)
                {
                    assigned.Assigned = true;
                }
                else
                {
                    var role = new Models.Persistence.Account.Role();

                    role.CopyProperties(item);
                    model.SecondEntities.Add(role);
                }
            }
        }
        #endregion Helpers
    }
}
//MdEnd
*/