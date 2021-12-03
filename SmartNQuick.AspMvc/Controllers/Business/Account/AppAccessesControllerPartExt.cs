//@BaseCode
//MdStart
#if ACCOUNT_ON
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SmartNQuick.AspMvc.Models.Persistence.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Model = SmartNQuick.AspMvc.Models.Business.Account.AppAccess;
using CommonBase.Extensions;

namespace SmartNQuick.AspMvc.Controllers.Business.Account
{
    //public partial class AppAccessesController
    //{
    //    public AppAccessesController()
    //        : base()
    //    {
    //        Constructing();
    //        Constructed();
    //    }
    //    partial void Constructing();
    //    partial void Constructed();

    //    protected override IActionResult ReturnCreateView(Model model)
    //    {
    //        SessionWrapper.SetStringValue(StaticLiterals.RedirectControllerKey, ControllerName);

    //        return RedirectToAction("Create", "Identities");
    //    }

    //    [ActionName("Edit")]
    //    public override async Task<IActionResult> EditAsync(int id)
    //    {
    //        using var ctrl = CreateController();
    //        var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
    //        var model = ToModel(entity);

    //        await LoadRolesAsync(model).ConfigureAwait(false);
    //        return View(model);
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    [ActionName("EditRoles")]
    //    public async Task<IActionResult> EditAsync(int id, Identity identityModel, IFormCollection formCollection)
    //    {
    //        using var ctrl = CreateController();
    //        async Task<IActionResult> CreateFailedAsync(Identity identity, string error)
    //        {
    //            var entity = await ctrl.CreateAsync().ConfigureAwait(false);

    //            entity.OneItem.CopyProperties(identity);

    //            var model = ToModel(entity);

    //            LastViewError = error;
    //            await LoadRolesAsync(model).ConfigureAwait(false);
    //            return View("EditRoles", model);
    //        }
    //        async Task<IActionResult> EditFailedAsync(Identity identity, string error)
    //        {
    //            var entity = await ctrl.GetByIdAsync(identity.Id).ConfigureAwait(false);

    //            entity.OneItem.CopyProperties(identity);

    //            var model = ToModel(entity);

    //            LastViewError = error;
    //            await LoadRolesAsync(model).ConfigureAwait(false);
    //            return View("EditRoles", model);
    //        }
    //        async Task UpdateRolesAsync(Model model)
    //        {
    //            using var ctrlRole = Adapters.Factory.Create<Contracts.Persistence.Account.IRole>(SessionWrapper.SessionToken);
    //            var roles = await ctrlRole.GetAllAsync().ConfigureAwait(false);

    //            model.ClearManyItems();
    //            foreach (var item in formCollection.Where(l => l.Key.StartsWith("Assigned")))
    //            {
    //                var roleId = item.Key.ToInt();
    //                var role = roles.SingleOrDefault(r => r.Id == roleId);

    //                if (role != null)
    //                {
    //                    model.AddManyItem(role);
    //                }
    //            }
    //        }

    //        if (ModelState.IsValid == false)
    //        {
    //            if (identityModel.Id == 0)
    //            {
    //                return await CreateFailedAsync(identityModel, GetModelStateError()).ConfigureAwait(false);
    //            }
    //            else
    //            {
    //                return await EditFailedAsync(identityModel, GetModelStateError()).ConfigureAwait(false);
    //            }
    //        }
    //        try
    //        {
    //            if (identityModel.Id == 0)
    //            {
    //                var entity = await ctrl.CreateAsync().ConfigureAwait(false);

    //                entity.OneItem.CopyProperties(identityModel);
    //                var model = ToModel(entity);

    //                await UpdateRolesAsync(model).ConfigureAwait(false);
    //                var result = await ctrl.InsertAsync(model).ConfigureAwait(false);

    //                id = result.Id;
    //            }
    //            else
    //            {
    //                var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

    //                var model = ToModel(entity);

    //                await UpdateRolesAsync(model).ConfigureAwait(false);
    //                await ctrl.UpdateAsync(model).ConfigureAwait(false);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            if (identityModel.Id == 0)
    //            {
    //                return await CreateFailedAsync(identityModel, ex.GetError()).ConfigureAwait(false);
    //            }
    //            else
    //            {
    //                return await EditFailedAsync(identityModel, ex.GetError()).ConfigureAwait(false);
    //            }
    //        }
    //        return RedirectToAction("Index");
    //    }

    //    //[ActionName("Details")]
    //    //public async Task<IActionResult> DetailsAsync(int id)
    //    //{
    //    //    using var ctrl = CreateController();
    //    //    var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

    //    //    return View(entity != null ? ToModel(entity) : entity);
    //    //}

    //    // GET: /Delete/5
    //    [ActionName("Delete")]
    //    public async Task<ActionResult> DeleteAsync(int id, string error = null)
    //    {
    //        using var ctrl = CreateController();
    //        using var ctrlRole = Adapters.Factory.Create<Contracts.Persistence.Account.IRole>(SessionWrapper.SessionToken);
    //        var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
    //        var model = ToModel(entity);

    //        if (error.HasContent())
    //            LastViewError = error;

    //        return View(model);
    //    }

    //    // POST: /Delete/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    [ActionName("Delete")]
    //    public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
    //    {
    //        try
    //        {
    //            using var ctrl = CreateController();

    //            await ctrl.DeleteAsync(id).ConfigureAwait(false);
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch (Exception ex)
    //        {
    //            return RedirectToAction("Delete", new { error = GetExceptionError(ex) });
    //        }
    //    }

    //    #region Export and Import
    //    protected override string[] CsvHeader => new string[] 
    //    { 
    //        "Id",
    //        $"{nameof(Model.OneItem)}.Name",
    //        $"{nameof(Model.OneItem)}.Email",
    //        $"{nameof(Model.OneItem)}.Password",
    //        $"{nameof(Model.OneItem)}.AccessFailedCount",
    //        $"{nameof(Model.OneItem)}.EnableJwtAuth", 
    //        "RoleList"
    //    };

    //    [ActionName("Export")]
    //    public async Task<FileResult> ExportAsync()
    //    {
    //        var fileName = "AppAccesses.csv";
    //        using var ctrl = CreateController();
    //        var entities = (await ctrl.GetAllAsync().ConfigureAwait(false)).Select(e => ToModel(e));

    //        return ExportDefault(CsvHeader, entities, fileName);
    //    }

    //    [ActionName("Import")]
    //    public ActionResult ImportAsync(string error = null)
    //    {
    //        var model = new Models.Modules.Csv.ImportProtocol() { BackController = ControllerName };

    //        if (error.HasContent())
    //            LastViewError = error;

    //        return View(model);
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    [ActionName("Import")]
    //    public async Task<IActionResult> ImportAsync()
    //    {
    //        var index = 0;
    //        var model = new Models.Modules.Csv.ImportProtocol() { BackController = ControllerName };
    //        var logInfos = new List<Models.Modules.Csv.ImportLog>();
    //        var importModels = ImportDefault<Model>(CsvHeader);
    //        using var ctrl = CreateController();

    //        foreach (var item in importModels)
    //        {
    //            index++;
    //            try
    //            {
    //                if (item.Action == Models.Modules.Csv.ImportAction.Insert)
    //                {
    //                    var entity = await ctrl.CreateAsync();

    //                    CopyModels(CsvHeader, item.Model, entity);
    //                    item.Model.ManyModels.ForEach(e => entity.AddManyItem(e));
    //                    await ctrl.InsertAsync(entity);
    //                }
    //                else if (item.Action == Models.Modules.Csv.ImportAction.Update)
    //                {
    //                    var entity = await ctrl.GetByIdAsync(item.Id);

    //                    CopyModels(CsvHeader, item.Model, entity);
    //                    entity.ClearManyItems();
    //                    item.Model.ManyModels.ForEach(e => entity.AddManyItem(e));

    //                    await ctrl.UpdateAsync(entity);
    //                }
    //                else if (item.Action == Models.Modules.Csv.ImportAction.Delete)
    //                {
    //                    await ctrl.DeleteAsync(item.Id);
    //                }
    //                logInfos.Add(new Models.Modules.Csv.ImportLog
    //                {
    //                    IsError = false,
    //                    Prefix = $"Line: {index} - {item.Action}",
    //                    Text = "OK",
    //                });
    //            }
    //            catch (Exception ex)
    //            {
    //                logInfos.Add(new Models.Modules.Csv.ImportLog
    //                {
    //                    IsError = true,
    //                    Prefix = $"Line: {index} - {item.Action}",
    //                    Text = ex.Message,
    //                });
    //            }
    //        }
    //        model.LogInfos = logInfos;
    //        return View(model);
    //    }
    //    #endregion Export and Import

    //    #region Helpers
    //    private async Task LoadRolesAsync(Model model)
    //    {
    //        model.CheckArgument(nameof(model));

    //        using var ctrlRole = Adapters.Factory.Create<Contracts.Persistence.Account.IRole>(SessionWrapper.SessionToken);
    //        var roles = await ctrlRole.GetAllAsync().ConfigureAwait(false);

    //        foreach (var item in roles)
    //        {
    //            var assigned = model.ManyModels.SingleOrDefault(r => r.Id == item.Id);

    //            if (assigned != null)
    //            {
    //                assigned.Assigned = true;
    //            }
    //            else
    //            {
    //                var role = new Models.Persistence.Account.Role();

    //                role.CopyProperties(item);
    //                model.ManyModels.Add(role);
    //            }
    //        }
    //    }
    //    #endregion Helpers
    //}
}
#endif
//MdEnd
