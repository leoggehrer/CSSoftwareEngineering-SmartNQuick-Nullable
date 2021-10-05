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

        #region Async-Methods
        public override Task<int> CountAsync()
        {
            return OneEntityController.CountAsync();
        }
        public override Task<int> CountByAsync(string predicate)
        {
            return OneEntityController.CountByAsync(predicate);
        }

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

        public override async Task<C> GetByIdAsync(int id)
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
        public override async Task<IEnumerable<C>> GetAllAsync()
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
        public override async Task<IEnumerable<C>> QueryAllAsync(string predicate)
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

        public override Task<C> CreateAsync()
        {
            return Task.Run<C>(() =>
            {
                var entity = new E();

                AfterCreate(entity);
                return entity;
            });
        }

        public override async Task<C> InsertAsync(C entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneItem.CheckArgument(nameof(entity.OneItem));
            entity.ManyItems.CheckArgument(nameof(entity.ManyItems));

            var result = new E();

            result.OneEntity.CopyProperties(entity.OneItem);
            await OneEntityController.InsertAsync(result.OneEntity).ConfigureAwait(false);

            foreach (var item in entity.ManyItems)
            {
                var manyEntity = new TManyEntity();

                manyEntity.CopyProperties(item);
                SetNavigationToParent(manyEntity, result.OneEntity);
                await ManyEntityController.InsertAsync(manyEntity).ConfigureAwait(false);
                result.AddManyItem(manyEntity);
            }
            return result;
        }
        internal override async Task<E> InsertEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneItem.CheckArgument(nameof(entity.OneItem));
            entity.ManyItems.CheckArgument(nameof(entity.ManyItems));

            await OneEntityController.InsertEntityAsync(entity.OneEntity).ConfigureAwait(false);

            foreach (var item in entity.ManyEntities)
            {
                SetNavigationToParent(item, entity.OneEntity);
                await ManyEntityController.InsertEntityAsync(item).ConfigureAwait(false);
            }
            return entity;
        }
        public override async Task<C> UpdateAsync(C entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneItem.CheckArgument(nameof(entity.OneItem));
            entity.ManyItems.CheckArgument(nameof(entity.ManyItems));

            var query = (await QueryDetailsAsync(entity.Id).ConfigureAwait(false)).ToList();

            //Delete all costs that are no longer included in the list.
            foreach (var item in query)
            {
                var exitsItem = entity.ManyItems.SingleOrDefault(i => i.Id == item.Id);

                if (exitsItem == null)
                {
                    await ManyEntityController.DeleteAsync(item.Id).ConfigureAwait(false);
                }
            }

            var result = new E();
            var oneEntity = await OneEntityController.UpdateAsync(entity.OneItem).ConfigureAwait(false);

            result.OneItem.CopyProperties(oneEntity);
            foreach (var item in entity.ManyItems)
            {
                if (item.Id == 0)
                {
                    var pi = GetForeignKeyToOne();

                    if (pi != null)
                    {
                        pi.SetValue(item, oneEntity.Id);
                    }
                    var insDetail = await ManyEntityController.InsertAsync(item).ConfigureAwait(false);

                    item.CopyProperties(insDetail);
                }
                else
                {
                    var updDetail = await ManyEntityController.UpdateAsync(item).ConfigureAwait(false);

                    item.CopyProperties(updDetail);
                }
            }
            return result;
        }
        public override async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id).ConfigureAwait(false);

            if (entity != null)
            {
                foreach (var item in entity.ManyItems)
                {
                    await ManyEntityController.DeleteAsync(item.Id).ConfigureAwait(false);
                }
                await OneEntityController.DeleteAsync(entity.Id).ConfigureAwait(false);
            }
            else
            {
                throw new LogicException(ErrorType.InvalidId);
            }
        }
        #endregion Async-Methods

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