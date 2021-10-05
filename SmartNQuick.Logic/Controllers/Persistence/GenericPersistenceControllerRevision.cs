//@BaseCode
//MdStart
#if ACCOUNT_ON && REVISION_ON
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Persistence
{
    internal partial class GenericPersistenceController<C, E>
    {
        private enum ActionType
        {
            Insert,
            Update,
            Delete,
        }
        private record HistoryItem(int IdentityId, ActionType ActionType, DateTime ActionTime, Entities.IdentityEntity Entity, string JsonData);
        private readonly List<HistoryItem> historyItems = new();

        protected override async Task<E> AfterInsertAsync(E entity)
        {
            var loginSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);

            if (loginSession != null)
            {
                historyItems.Add(new HistoryItem(loginSession.IdentityId, ActionType.Insert, DateTime.Now, entity, null));
            }
            return await base.AfterInsertAsync(entity).ConfigureAwait(false);
        }
        protected override async Task<E> BeforeUpdateAsync(E entity)
        {
            var historyItem = historyItems.FirstOrDefault(e => e.Entity.Id == entity.Id);

            if (historyItem == null)
            {
                var oriEntity = Context.QueryableSet<C, E>().AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);

                if (oriEntity != null)
                {
                    var loginSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);

                    if (loginSession != null)
                    {
                        historyItems.Add(new HistoryItem(loginSession.IdentityId, ActionType.Update, DateTime.Now, entity, JsonSerializer.Serialize(oriEntity, new JsonSerializerOptions() { MaxDepth = 0, })));
                    }
                }
            }
            return await base.BeforeUpdateAsync(entity).ConfigureAwait(false);
        }
        protected override async Task BeforeDeleteAsync(E entity)
        {
            var loginSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);

            if (loginSession != null)
            {
                historyItems.Add(new HistoryItem(loginSession.IdentityId, ActionType.Delete, DateTime.Now, entity, JsonSerializer.Serialize(entity, new JsonSerializerOptions() { MaxDepth = 0 })));
            }
            await base.BeforeDeleteAsync(entity).ConfigureAwait(false);
        }

        protected override async Task AfterSaveChangesAsync()
        {
            using var ctrl = new Revision.HistoryController(Factory.CreateContext());

            ctrl.SessionToken = Modules.Security.Authorization.SystemAuthorizationToken;
            foreach (var item in historyItems)
            {
                var history = new Entities.Persistence.Revision.History()
                {
                    IdentityId = item.IdentityId,
                    ActionType = item.ActionType.ToString(),
                    ActionTime = item.ActionTime,
                    SubjectName = item.Entity.GetType().Name,
                    SubjectId = item.Entity.Id,
                    JsonData = item.JsonData,
                };
                _ = await ctrl.Context.InsertAsync<Contracts.Persistence.Revision.IHistory, Entities.Persistence.Revision.History>(history).ConfigureAwait(false);
            }
            await ctrl.Context.SaveChangesAsync().ConfigureAwait(false);
            historyItems.Clear();
        }
        protected override Task AfterRejectChangesAsync()
        {
            historyItems.Clear();
            return base.AfterRejectChangesAsync();
        }
    }
}
#endif
//MdEnd