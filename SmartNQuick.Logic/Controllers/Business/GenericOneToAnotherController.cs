//@BaseCode
//MdStart
using SmartNQuick.Logic.Modules.Exception;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Business
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class GenericOneToAnotherController<TContract, TEntity, TOneContract, TOneEntity, TAnotherContract, TAnotherEntity> : BusinessControllerAdapter<TContract, TEntity>
        where TContract : Contracts.IOneToAnother<TOneContract, TAnotherContract>
        where TEntity : Entities.OneToAnotherEntity<TOneContract, TOneEntity, TAnotherContract, TAnotherEntity>, TContract, Contracts.ICopyable<TContract>, new()
        where TOneContract : Contracts.IIdentifiable, Contracts.ICopyable<TOneContract>
        where TOneEntity : Entities.IdentityEntity, TOneContract, Contracts.ICopyable<TOneContract>, new()
        where TAnotherContract : Contracts.IIdentifiable, Contracts.ICopyable<TAnotherContract>
        where TAnotherEntity : Entities.IdentityEntity, TAnotherContract, Contracts.ICopyable<TAnotherContract>, new()
    {
        static GenericOneToAnotherController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        protected abstract GenericController<TOneContract, TOneEntity> OneEntityController { get; set; }
        protected abstract GenericController<TAnotherContract, TAnotherEntity> AnotherEntityController { get; set; }

        public override bool IsTransient => true;

        public GenericOneToAnotherController(DataContext.IContext context) : base(context)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public GenericOneToAnotherController(ControllerObject controller) : base(controller)
        {
            Constructing();
            Constructed();
        }

        protected virtual PropertyInfo GetNavigationToOne()
        {
            return typeof(TAnotherContract).GetInterfaceProperty(typeof(TOneEntity).Name);
        }
        protected virtual PropertyInfo GetForeignKeyToOne()
        {
            return typeof(TAnotherContract).GetInterfaceProperty($"{typeof(TOneEntity).Name}Id");
        }
        protected virtual async Task LoadAnotherAsync(TEntity entity, int masterId)
        {
            var predicate = $"{typeof(TOneEntity).Name}Id == {masterId}";
            var qyr = await AnotherEntityController.QueryEntityAllAsync(predicate).ConfigureAwait(false);

            if (qyr.Any())
            {
                if (AnotherEntityController.IsTransient)
                {
                    var another = await AnotherEntityController.GetEntityByIdAsync(qyr.ElementAt(0).Id).ConfigureAwait(false);

                    entity.AnotherEntity = another;
                }
                else
                {
                    entity.AnotherEntity = qyr.ElementAt(0);
                }
            }
            else
            {
                entity.AnotherEntity.CopyProperties(new TAnotherEntity());
            }
        }
        protected virtual async Task<IEnumerable<TAnotherEntity>> QueryDetailsAsync(int masterId)
        {
            var result = new List<TAnotherEntity>();
            var predicate = $"{typeof(TOneEntity).Name}Id == {masterId}";
            var query = await AnotherEntityController.QueryEntityAllAsync(predicate).ConfigureAwait(false);

            foreach (var item in query)
            {
                var e = new TAnotherEntity();

                e.CopyProperties(item);
                result.Add(e);
            }
            return result;
        }

        #region Count
        internal override Task<int> ExecuteCountAsync()
        {
            return OneEntityController.ExecuteCountAsync();
        }
        internal override Task<int> ExecuteCountByAsync(string predicate)
        {
            return OneEntityController.ExecuteCountByAsync(predicate);
        }
        #endregion Count

        #region Query
        internal override async Task<TEntity> ExecuteGetEntityByIdAsync(int id)
        {
            TEntity result;
            var oneEntity = await OneEntityController.GetEntityByIdAsync(id).ConfigureAwait(false);

            if (oneEntity != null)
            {
                result = new TEntity
                {
                    OneEntity = oneEntity
                };
                await LoadAnotherAsync(result, oneEntity.Id).ConfigureAwait(false);
            }
            else
            {
                throw new LogicException(ErrorType.InvalidId);
            }
            return result;
        }

        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityAllAsync()
        {
            var result = new List<TEntity>();
            var query = await OneEntityController.ExecuteGetEntityAllAsync().ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityAllAsync(string orderBy)
        {
            var result = new List<TEntity>();
            var query = await OneEntityController.ExecuteGetEntityAllAsync(orderBy).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }

        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityPageListAsync(int pageIndex, int pageSize)
        {
            var result = new List<TEntity>();
            var query = await OneEntityController.ExecuteGetEntityPageListAsync(pageIndex, pageSize).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            var result = new List<TEntity>();
            var query = await OneEntityController.ExecuteGetEntityPageListAsync(orderBy, pageIndex, pageSize).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }

        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(string predicate)
        {
            var result = new List<TEntity>();
            var query = await OneEntityController.ExecuteQueryEntityAllAsync(predicate).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(string predicate, string orderBy)
        {
            var result = new List<TEntity>();
            var query = await OneEntityController.ExecuteQueryEntityAllAsync(predicate, orderBy).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }

        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            var result = new List<TEntity>();
            var query = await OneEntityController.ExecuteQueryEntityPageListAsync(predicate, pageIndex, pageSize).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            var result = new List<TEntity>();
            var query = await OneEntityController.ExecuteQueryEntityPageListAsync(predicate, orderBy, pageIndex, pageSize).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        #endregion Query

        #region Insert
        internal override async Task<TEntity> ExecuteInsertEntityAsync(TEntity entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneEntity.CheckArgument(nameof(entity.OneEntity));
            entity.AnotherEntity.CheckArgument(nameof(entity.AnotherEntity));

            entity.OneEntity = await OneEntityController.InsertEntityAsync(entity.OneEntity).ConfigureAwait(false);

            var pi = GetNavigationToOne();

            if (pi != null)
            {
                pi.SetValue(entity.AnotherEntity, entity.OneEntity);
            }
            entity.AnotherEntity = await AnotherEntityController.InsertEntityAsync(entity.AnotherEntity).ConfigureAwait(false);
            return entity;
        }
        #endregion Insert

        #region Update
        internal override async Task<TEntity> ExecuteUpdateEntityAsync(TEntity entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneEntity.CheckArgument(nameof(entity.OneEntity));
            entity.AnotherEntity.CheckArgument(nameof(entity.AnotherEntity));

            entity.OneEntity = await OneEntityController.UpdateEntityAsync(entity.OneEntity).ConfigureAwait(false);

            if (entity.AnotherEntity.Id == 0)
            {
                var pi = GetForeignKeyToOne();

                if (pi != null)
                {
                    pi.SetValue(entity.AnotherEntity, entity.OneEntity.Id);
                }
                entity.AnotherEntity = await AnotherEntityController.InsertEntityAsync(entity.AnotherEntity).ConfigureAwait(false);
            }
            else
            {
                entity.AnotherEntity = await AnotherEntityController.UpdateEntityAsync(entity.AnotherEntity).ConfigureAwait(false);
            }
            return entity;
        }
        #endregion Update

        #region Delete
        internal override async Task<TEntity> ExecuteDeleteEntityAsync(TEntity entity)
        {
            if (entity.AnotherEntity.Id > 0)
            {
                await AnotherEntityController.DeleteEntityAsync(entity.AnotherEntity).ConfigureAwait(false);
            }
            await OneEntityController.DeleteEntityAsync(entity.OneEntity).ConfigureAwait(false);
            return entity;
        }
        #endregion Delete
    }
}
//MdEnd