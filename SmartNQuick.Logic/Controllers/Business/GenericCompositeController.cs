//@BaseCode
//MdStart
using SmartNQuick.Logic.Modules.Exception;
using System;
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
    internal abstract partial class GenericCompositeController<C, E, TConnector, TConnectorEntity, TOne, TOneEntity, TAnother, TAnotherEntity> : BusinessControllerAdapter<C, E>
        where C : Contracts.IComposite<TConnector, TOne, TAnother>
        where E : Entities.CompositeEntity<TConnector, TConnectorEntity, TOne, TOneEntity, TAnother, TAnotherEntity>, C, Contracts.ICopyable<C>, new()
        where TConnector : Contracts.IIdentifiable, Contracts.ICopyable<TConnector>
        where TConnectorEntity : Entities.IdentityEntity, TConnector, Contracts.ICopyable<TConnector>, new()
        where TOne : Contracts.IIdentifiable, Contracts.ICopyable<TOne>
        where TOneEntity : Entities.IdentityEntity, TOne, Contracts.ICopyable<TOne>, new()
        where TAnother : Contracts.IIdentifiable, Contracts.ICopyable<TAnother>
        where TAnotherEntity : Entities.IdentityEntity, TAnother, Contracts.ICopyable<TAnother>, new()
    {
        static GenericCompositeController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        protected abstract GenericController<TConnector, TConnectorEntity> ConnectorEntityController { get; set; }
        protected abstract GenericController<TOne, TOneEntity> OneEntityController { get; set; }
        protected abstract GenericController<TAnother, TAnotherEntity> AnotherEntityController { get; set; }

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
            return typeof(TConnectorEntity).GetProperties().SingleOrDefault(pi => pi.Name.Equals($"{typeof(TOneEntity).Name}Id"));
        }
        protected virtual PropertyInfo GetForeignKeyToAnother()
        {
            return typeof(TConnectorEntity).GetProperties().SingleOrDefault(pi => pi.Name.Equals($"{typeof(TAnotherEntity).Name}Id"));
        }
        protected virtual async Task LoadChildsAsync(E entity)
        {
            var piOneFK = GetForeignKeyToOne();
            var piAnotherFK = GetForeignKeyToAnother();

            if (piOneFK != null)
            {
                var value = piOneFK.GetValue(entity.ConnectorEntity);

                if (value != null)
                {
                    var child = await OneEntityController.GetEntityByIdAsync((int)Convert.ChangeType(value, piOneFK.PropertyType)).ConfigureAwait(false);

                    if (child != null)
                    {
                        entity.OneEntity.CopyProperties(child);
                    }
                }
            }
            if (piAnotherFK != null)
            {
                var value = piAnotherFK.GetValue(entity.ConnectorEntity);

                if (value != null)
                {
                    var child = await AnotherEntityController.GetEntityByIdAsync((int)Convert.ChangeType(value, piAnotherFK.PropertyType)).ConfigureAwait(false);

                    if (child != null)
                    {
                        entity.AnotherEntity.CopyProperties(child);
                    }
                }
            }
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
        internal override async Task<E> ExecuteGetEntityByIdAsync(int id)
        {
            E result;
            var entity = await ConnectorEntityController.GetEntityByIdAsync(id).ConfigureAwait(false);

            if (entity != null)
            {
                result = new E();
                result.ConnectorEntity.CopyProperties(entity);
                await LoadChildsAsync(result).ConfigureAwait(false);
            }
            else
            {
                throw new LogicException(ErrorType.InvalidId);
            }
            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteGetEntityAllAsync()
        {
            var result = new List<E>();
            var query = await ConnectorEntityController.GetEntityAllAsync().ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new E();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        internal override async Task<IEnumerable<E>> ExecuteQueryEntityAllAsync(string predicate)
        {
            var result = new List<E>();
            var query = await ConnectorEntityController.QueryEntityAllAsync(predicate).ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new E();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        #endregion Query

        #region Insert
        internal override async Task<E> ExecuteInsertEntityAsync(E entity)
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
        internal override async Task<E> ExecuteUpdateEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));
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
        internal override async Task ExecuteDeleteEntityAsync(E entity)
        {
            await ConnectorEntityController.DeleteEntityAsync(entity.ConnectorEntity).ConfigureAwait(false);
        }
        #endregion Delete
    }
}
//MdEnd