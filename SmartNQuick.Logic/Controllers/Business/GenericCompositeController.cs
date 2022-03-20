//@BaseCode
//MdStart
using SmartNQuick.Logic.Modules.Exception;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Business
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class GenericCompositeController<TContract, TEntity, TConnectorContract, TConnectorEntity, TOneContract, TOneEntity, TAnotherContract, TAnotherEntity> : BusinessControllerAdapter<TContract, TEntity>
        where TContract : Contracts.IComposite<TConnectorContract, TOneContract, TAnotherContract>
        where TEntity : Entities.CompositeEntity<TConnectorContract, TConnectorEntity, TOneContract, TOneEntity, TAnotherContract, TAnotherEntity>, TContract, Contracts.ICopyable<TContract>, new()
        where TConnectorContract : Contracts.IIdentifiable, Contracts.ICopyable<TConnectorContract>
        where TConnectorEntity : Entities.IdentityEntity, TConnectorContract, Contracts.ICopyable<TConnectorContract>, new()
        where TOneContract : Contracts.IIdentifiable, Contracts.ICopyable<TOneContract>
        where TOneEntity : Entities.IdentityEntity, TOneContract, Contracts.ICopyable<TOneContract>, new()
        where TAnotherContract : Contracts.IIdentifiable, Contracts.ICopyable<TAnotherContract>
        where TAnotherEntity : Entities.IdentityEntity, TAnotherContract, Contracts.ICopyable<TAnotherContract>, new()
    {
        static GenericCompositeController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        protected abstract GenericController<TConnectorContract, TConnectorEntity> ConnectorEntityController { get; set; }
        protected abstract GenericController<TOneContract, TOneEntity> OneEntityController { get; set; }
        protected abstract GenericController<TAnotherContract, TAnotherEntity> AnotherEntityController { get; set; }

        public override bool IsTransient => true;

        public GenericCompositeController(DataContext.IContext context) : base(context)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public GenericCompositeController(ControllerObject controller) : base(controller)
        {
            Constructing();
            Constructed();
        }

        protected virtual PropertyInfo GetNavigationToOne()
        {
            return typeof(TConnectorEntity).GetProperty(typeof(TOneEntity).Name);
        }
        protected virtual PropertyInfo GetNavigationToAnother()
        {
            return typeof(TConnectorEntity).GetProperty(typeof(TAnotherEntity).Name);
        }
        protected virtual PropertyInfo GetForeignKeyToOne()
        {
            return typeof(TConnectorContract).GetInterfaceProperty($"{typeof(TOneEntity).Name}Id");
        }
        protected virtual PropertyInfo GetForeignKeyToAnother()
        {
            return typeof(TConnectorContract).GetInterfaceProperty($"{typeof(TAnotherEntity).Name}Id");
        }
        protected virtual async Task LoadChildsAsync(TEntity entity)
        {
            var piOneFK = GetForeignKeyToOne();
            var piAnotherFK = GetForeignKeyToAnother();

            if (piOneFK != null)
            {
                var value = piOneFK.GetValue(entity.ConnectorEntity);

                if (value != null)
                {
                    entity.OneEntity = await OneEntityController.GetEntityByIdAsync((int)Convert.ChangeType(value, piOneFK.PropertyType)).ConfigureAwait(false);
                }
            }
            if (piAnotherFK != null)
            {
                var value = piAnotherFK.GetValue(entity.ConnectorEntity);

                if (value != null)
                {
                    entity.AnotherEntity = await AnotherEntityController.GetEntityByIdAsync((int)Convert.ChangeType(value, piAnotherFK.PropertyType)).ConfigureAwait(false);
                }
            }
        }

        #region Count
        internal override Task<int> ExecuteCountAsync()
        {
            return ConnectorEntityController.ExecuteCountAsync();
        }
        internal override Task<int> ExecuteCountByAsync(string predicate)
        {
            return ConnectorEntityController.ExecuteCountByAsync(predicate);
        }
        #endregion Count

        #region Query
        internal override async Task<TEntity> ExecuteGetEntityByIdAsync(int id)
        {
            TEntity result;
            var entity = await ConnectorEntityController.ExecuteGetEntityByIdAsync(id).ConfigureAwait(false);

            if (entity != null)
            {
                result = new TEntity();
                result.ConnectorEntity = entity;
                await LoadChildsAsync(result).ConfigureAwait(false);
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
            var query = await ConnectorEntityController.ExecuteGetEntityAllAsync().ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityAllAsync(string orderBy)
        {
            var result = new List<TEntity>();
            var query = await ConnectorEntityController.ExecuteGetEntityAllAsync(orderBy).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }

        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityPageListAsync(int pageIndex, int pageSize)
        {
            var result = new List<TEntity>();
            var query = await ConnectorEntityController.ExecuteGetEntityPageListAsync(pageIndex, pageSize).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteGetEntityPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            var result = new List<TEntity>();
            var query = await ConnectorEntityController.ExecuteGetEntityPageListAsync(orderBy, pageIndex, pageSize).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }

        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(string predicate)
        {
            var result = new List<TEntity>();
            var query = await ConnectorEntityController.ExecuteQueryEntityAllAsync(predicate).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityAllAsync(string predicate, string orderBy)
        {
            var result = new List<TEntity>();
            var query = await ConnectorEntityController.ExecuteQueryEntityAllAsync(predicate, orderBy).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            var result = new List<TEntity>();
            var query = await ConnectorEntityController.ExecuteQueryEntityPageListAsync(predicate, pageIndex, pageSize).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<TEntity>> ExecuteQueryEntityPageListAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            var result = new List<TEntity>();
            var query = await ConnectorEntityController.ExecuteQueryEntityPageListAsync(predicate, orderBy, pageIndex, pageSize).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new TEntity();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        #endregion Query

        #region Insert
        internal override async Task<TEntity> ExecuteInsertEntityAsync(TEntity entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.ConnectorEntity.CheckArgument(nameof(entity.ConnectorEntity));
            entity.OneEntity.CheckArgument(nameof(entity.OneEntity));
            entity.AnotherEntity.CheckArgument(nameof(entity.AnotherEntity));

            if (entity.OneItemIncludeSave)
            {
                if (entity.OneEntity.Id == 0)
                {
                    entity.OneEntity = await OneEntityController.InsertEntityAsync(entity.OneEntity).ConfigureAwait(false);

                    var piNav = GetNavigationToOne();

                    if (piNav != null)
                    {
                        piNav.SetValue(entity.ConnectorEntity, entity.OneEntity);
                    }
                }
                else
                {
                    entity.OneEntity = await OneEntityController.UpdateEntityAsync(entity.OneEntity).ConfigureAwait(false);
                }
            }

            if (entity.AnotherItemIncludeSave)
            {
                if (entity.AnotherItem.Id == 0)
                {
                    entity.AnotherEntity = await AnotherEntityController.InsertEntityAsync(entity.AnotherEntity).ConfigureAwait(false);

                    var piNav = GetNavigationToAnother();

                    if (piNav != null)
                    {
                        piNav.SetValue(entity.ConnectorEntity, entity.AnotherEntity);
                    }
                }
                else
                {
                    entity.AnotherEntity = await AnotherEntityController.UpdateEntityAsync(entity.AnotherEntity).ConfigureAwait(false);
                }
            }
            entity.ConnectorEntity = await ConnectorEntityController.InsertEntityAsync(entity.ConnectorEntity).ConfigureAwait(false);
            return entity;
        }
        #endregion Insert

        #region Update
        internal override async Task<TEntity> ExecuteUpdateEntityAsync(TEntity entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.ConnectorEntity.CheckArgument(nameof(entity.ConnectorEntity));
            entity.OneEntity.CheckArgument(nameof(entity.OneEntity));
            entity.AnotherEntity.CheckArgument(nameof(entity.AnotherEntity));

            if (entity.OneItemIncludeSave)
            {
                if (entity.OneEntity.Id == 0)
                {
                    entity.OneEntity = await OneEntityController.InsertEntityAsync(entity.OneEntity).ConfigureAwait(false);

                    var piNav = GetNavigationToOne();

                    if (piNav != null)
                    {
                        piNav.SetValue(entity.ConnectorEntity, entity.OneEntity);
                    }
                }
                else
                {
                    entity.OneEntity = await OneEntityController.UpdateEntityAsync(entity.OneEntity).ConfigureAwait(false);
                }
            }

            if (entity.AnotherItemIncludeSave)
            {
                if (entity.AnotherEntity.Id == 0)
                {
                    entity.AnotherEntity = await AnotherEntityController.InsertEntityAsync(entity.AnotherEntity).ConfigureAwait(false);

                    var piNav = GetNavigationToAnother();

                    if (piNav != null)
                    {
                        piNav.SetValue(entity.ConnectorEntity, entity.AnotherEntity);
                    }
                }
                else
                {
                    entity.AnotherEntity = await AnotherEntityController.UpdateEntityAsync(entity.AnotherEntity).ConfigureAwait(false);
                }
            }
            entity.ConnectorEntity = await ConnectorEntityController.UpdateEntityAsync(entity.ConnectorEntity).ConfigureAwait(false);
            return entity;
        }
        #endregion Update

        #region Delete
        internal override async Task ExecuteDeleteEntityAsync(TEntity entity)
        {
            await ConnectorEntityController.DeleteEntityAsync(entity.ConnectorEntity).ConfigureAwait(false);
        }
        #endregion Delete
    }
}
//MdEnd