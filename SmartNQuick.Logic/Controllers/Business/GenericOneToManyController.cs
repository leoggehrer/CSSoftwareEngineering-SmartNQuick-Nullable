//@BaseCode
//MdStart
using CommonBase.Extensions;
using SmartNQuick.Logic.Modules.Exception;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Business
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class GenericOneToManyController<C, E, TOne, TOneEntity, TMany, TManyEntity> : BusinessControllerAdapter<C, E>
        where C : Contracts.IOneToMany<TOne, TMany>
        where E : Entities.OneToManyEntity<TOne, TOneEntity, TMany, TManyEntity>, C, Contracts.ICopyable<C>, new()
        where TOne : Contracts.IIdentifiable, Contracts.ICopyable<TOne>
        where TOneEntity : Entities.IdentityEntity, TOne, Contracts.ICopyable<TOne>, new()
        where TMany : Contracts.IIdentifiable, Contracts.ICopyable<TMany>
        where TManyEntity : Entities.IdentityEntity, TMany, Contracts.ICopyable<TMany>, new()
    {
        static GenericOneToManyController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        protected abstract GenericController<TOne, TOneEntity> OneEntityController { get; set; }
        protected abstract GenericController<TMany, TManyEntity> ManyEntityController { get; set; }

        public override bool IsTransient => true;

        public GenericOneToManyController(DataContext.IContext context) : base(context)
        {
            Constructing();
#if ACCOUNT_ON
            ChangedSessionToken += HandleChangedSessionToken;
#endif
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public GenericOneToManyController(ControllerObject controller) : base(controller)
        {
            Constructing();
#if ACCOUNT_ON
            ChangedSessionToken += HandleChangedSessionToken;
#endif
            Constructed();
        }

#if ACCOUNT_ON
        private void HandleChangedSessionToken(object sender, System.EventArgs e)
        {
            OneEntityController.SessionToken = SessionToken;
            ManyEntityController.SessionToken = SessionToken;
        }
#endif

        protected virtual bool SetNavigationToParent(object obj, object value)
        {
            var result = false;
            var type = obj.GetType();
            var pi = type.GetProperty("OneEntity");

            if (pi != null)
            {
                var onePi = pi.PropertyType.GetProperty(typeof(TOneEntity).Name);

                if (onePi != null)
                {
                    result = true;
                    onePi.SetValue(pi.GetValue(obj), value);
                }
            }
            else
            {
                pi = typeof(TManyEntity).GetProperty(typeof(TOneEntity).Name);

                if (pi != null)
                {
                    result = true;
                    pi.SetValue(obj, value);
                }
            }
            return result;
        }
        protected virtual PropertyInfo GetForeignKeyToOne()
        {
            return typeof(TMany).GetProperty($"{typeof(TOneEntity).Name}Id");
        }
        protected virtual async Task LoadDetailsAsync(E entity, int masterId)
        {
            var predicate = $"{typeof(TOneEntity).Name}Id == {masterId}";
            var query = await ManyEntityController.QueryAllAsync(predicate).ConfigureAwait(false);

            entity.ClearManyItems();
            foreach (var item in query)
            {
                if (ManyEntityController.IsTransient)
                {
                    var manyEntity = await ManyEntityController.GetByIdAsync(item.Id).ConfigureAwait(false);

                    entity.AddManyItem(manyEntity);
                }
                else
                {
                    entity.AddManyItem(item);
                }
            }
        }
        protected virtual async Task<IEnumerable<TManyEntity>> QueryDetailsAsync(int masterId)
        {
            var result = new List<TManyEntity>();
            var predicate = $"{typeof(TOneEntity).Name}Id == {masterId}";
            var query = await ManyEntityController.QueryAllAsync(predicate).ConfigureAwait(false);

            foreach (var item in query)
            {
                var e = new TManyEntity();

                e.CopyProperties(item);
                result.Add(e);
            }
            return result;
        }

        #region Count
        public override Task<int> CountAsync()
        {
            return OneEntityController.CountAsync();
        }
        public override Task<int> CountByAsync(string predicate)
        {
            return OneEntityController.CountByAsync(predicate);
        }
        #endregion Count

        #region Query
        internal override async Task<E> ExecuteGetEntityByIdAsync(int id)
        {
            E result;
            var oneEntity = await OneEntityController.GetByIdAsync(id).ConfigureAwait(false);

            if (oneEntity != null)
            {
                result = new E();
                result.OneItem.CopyProperties(oneEntity);
                await LoadDetailsAsync(result, oneEntity.Id).ConfigureAwait(false);
            }
            else
                throw new LogicException(ErrorType.InvalidId);

            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteGetEntityAllAsync()
        {
            var result = new List<E>();
            var query = await OneEntityController.GetAllAsync().ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new E();

                entity.OneItem.CopyProperties(item);
                await LoadDetailsAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(string predicate)
        {
            var result = new List<E>();
            var query = await OneEntityController.QueryAllAsync(predicate).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new E();

                entity.OneItem.CopyProperties(item);
                await LoadDetailsAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        #endregion Query

        #region Insert
        internal override async Task<E> ExecuteInsertEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneEntity.CheckArgument(nameof(entity.OneEntity));
            entity.ManyEntities.CheckArgument(nameof(entity.ManyEntities));

            entity.OneEntity = await OneEntityController.InsertEntityAsync(entity.OneEntity).ConfigureAwait(false);

            foreach (var item in entity.ManyEntities)
            {
                SetNavigationToParent(item, entity.OneEntity);
                await ManyEntityController.InsertEntityAsync(item).ConfigureAwait(false);
            }
            return entity;
        }
        #endregion Insert

        #region Update

        internal override async Task<E> ExecuteUpdateEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneEntity.CheckArgument(nameof(entity.OneEntity));
            entity.ManyEntities.CheckArgument(nameof(entity.ManyEntities));

            var query = (await QueryDetailsAsync(entity.Id).ConfigureAwait(false)).ToList();

            //Delete all costs that are no longer included in the list.
            foreach (var item in query)
            {
                var exitsItem = entity.ManyEntities.SingleOrDefault(i => i.Id == item.Id);

                if (exitsItem == null)
                {
                    await ManyEntityController.DeleteAsync(item.Id).ConfigureAwait(false);
                }
            }

            entity.OneEntity = await OneEntityController.UpdateEntityAsync(entity.OneEntity).ConfigureAwait(false);
            foreach (var item in entity.ManyEntities)
            {
                if (item.Id == 0)
                {
                    var pi = GetForeignKeyToOne();

                    if (pi != null)
                    {
                        pi.SetValue(item, entity.OneEntity.Id);
                    }
                    var insDetail = await ManyEntityController.InsertEntityAsync(item).ConfigureAwait(false);

                    item.CopyProperties(insDetail);
                }
                else
                {
                    var updDetail = await ManyEntityController.UpdateEntityAsync(item).ConfigureAwait(false);

                    item.CopyProperties(updDetail);
                }
            }
            return entity;
        }
        #endregion Update

        #region Delete
        internal override async Task<E> ExecuteDeleteEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneEntity.CheckArgument(nameof(entity.OneEntity));
            entity.ManyEntities.CheckArgument(nameof(entity.ManyEntities));

            foreach (var item in entity.ManyEntities)
            {
                await ManyEntityController.DeleteEntityAsync(item).ConfigureAwait(false);
            }
            await OneEntityController.DeleteEntityAsync(entity.OneEntity).ConfigureAwait(false);
            return entity;
        }
        #endregion Delete

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
#if ACCOUNT_ON
                ChangedSessionToken -= HandleChangedSessionToken;
#endif
                OneEntityController.Dispose();
                ManyEntityController.Dispose();

                OneEntityController = null;
                ManyEntityController = null;
            }
        }
    }
}
//MdEnd