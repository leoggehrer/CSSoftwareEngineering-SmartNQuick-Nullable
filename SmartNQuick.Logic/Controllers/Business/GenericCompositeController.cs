//@BaseCode
//MdStart
using CommonBase.Extensions;
using SmartNQuick.Logic.Modules.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Business
{
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
#if ACCOUNT_ON
            ChangedSessionToken += HandleChangedSessionToken;
#endif
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public GenericCompositeController(ControllerObject controller) : base(controller)
        {
            Constructing();
#if ACCOUNT_ON
            ChangedSessionToken += HandleChangedSessionToken;
#endif
            Constructed();
        }

#if ACCOUNT_ON
        private void HandleChangedSessionToken(object sender, EventArgs e)
        {
            ConnectorEntityController.SessionToken = SessionToken;
            OneEntityController.SessionToken = SessionToken;
            AnotherEntityController.SessionToken = SessionToken;
        }
#endif

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
                    var child = await OneEntityController.GetByIdAsync((int)Convert.ChangeType(value, piOneFK.PropertyType)).ConfigureAwait(false);

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
                    var child = await AnotherEntityController.GetByIdAsync((int)Convert.ChangeType(value, piAnotherFK.PropertyType)).ConfigureAwait(false);

                    if (child != null)
                    {
                        entity.AnotherEntity.CopyProperties(child);
                    }
                }
            }
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
        public override async Task<C> GetByIdAsync(int id)
        {
            E result;
            var entity = await ConnectorEntityController.GetByIdAsync(id).ConfigureAwait(false);

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
        public override async Task<IEnumerable<C>> GetAllAsync()
        {
            var result = new List<E>();
            var query = await ConnectorEntityController.GetAllAsync().ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new E();

                entity.ConnectorEntity.CopyProperties(item);
                await LoadChildsAsync(entity).ConfigureAwait(false);

                result.Add(entity);
            }
            return result;
        }
        public override async Task<IEnumerable<C>> QueryAllAsync(string predicate)
        {
            var result = new List<E>();
            var query = await ConnectorEntityController.QueryAllAsync(predicate).ConfigureAwait(false);

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
            entity.ConnectorItem.CheckArgument(nameof(entity.ConnectorItem));
            entity.OneItem.CheckArgument(nameof(entity.OneItem));
            entity.AnotherItem.CheckArgument(nameof(entity.AnotherItem));

            var result = new E();

            result.OneEntity.CopyProperties(entity.OneItem);
            if (entity.OneItemIncludeSave)
            {
                if (result.OneEntity.Id == 0)
                {
                    await OneEntityController.InsertAsync(result.OneEntity).ConfigureAwait(false);

                    var piNav = GetNavigationToOne();

                    if (piNav != null)
                    {
                        piNav.SetValue(result.ConnectorEntity, result.OneEntity);
                    }
                }
                else
                {
                    await OneEntityController.UpdateAsync(result.OneEntity).ConfigureAwait(false);
                }
            }

            result.AnotherEntity.CopyProperties(entity.AnotherItem);
            if (entity.AnotherItemIncludeSave)
            {
                if (result.AnotherItem.Id == 0)
                {
                    await AnotherEntityController.InsertAsync(result.AnotherEntity).ConfigureAwait(false);

                    var piNav = GetNavigationToAnother();

                    if (piNav != null)
                    {
                        piNav.SetValue(result.ConnectorEntity, result.AnotherEntity);
                    }
                }
                else
                {
                    await AnotherEntityController.UpdateAsync(result.AnotherEntity).ConfigureAwait(false);
                }
            }
            result.ConnectorEntity.CopyProperties(entity.ConnectorItem);
            await ConnectorEntityController.InsertAsync(result.ConnectorEntity).ConfigureAwait(false);
            return result;
        }
        #endregion Insert

        #region Update
        internal override async Task<E> ExecuteUpdateEntityAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.OneItem.CheckArgument(nameof(entity.OneItem));
            entity.AnotherItem.CheckArgument(nameof(entity.AnotherItem));

            var result = new E();

            result.OneEntity.CopyProperties(entity.OneItem);
            if (entity.OneItemIncludeSave)
            {
                if (result.OneEntity.Id == 0)
                {
                    await OneEntityController.InsertAsync(result.OneEntity).ConfigureAwait(false);

                    var piNav = GetNavigationToOne();

                    if (piNav != null)
                    {
                        piNav.SetValue(result.ConnectorEntity, result.OneEntity);
                    }
                }
                else
                {
                    await OneEntityController.UpdateAsync(result.OneEntity).ConfigureAwait(false);
                }
            }

            result.AnotherEntity.CopyProperties(entity.AnotherItem);
            if (entity.AnotherItemIncludeSave)
            {
                if (result.AnotherItem.Id == 0)
                {
                    await AnotherEntityController.InsertAsync(result.AnotherItem).ConfigureAwait(false);

                    var piNav = GetNavigationToAnother();

                    if (piNav != null)
                    {
                        piNav.SetValue(result.ConnectorEntity, result.AnotherItem);
                    }
                }
                else
                {
                    await AnotherEntityController.UpdateAsync(result.AnotherItem).ConfigureAwait(false);
                }
            }
            result.ConnectorEntity.CopyProperties(entity.ConnectorItem);
            await ConnectorEntityController.UpdateAsync(result.ConnectorEntity).ConfigureAwait(false);
            return result;
        }
        #endregion Update

        #region Delete
        internal override async Task ExecuteDeleteEntityAsync(E entity)
        {
            await ConnectorEntityController.DeleteAsync(entity.Id).ConfigureAwait(false);
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
                ConnectorEntityController.Dispose();
                OneEntityController.Dispose();
                AnotherEntityController.Dispose();

                ConnectorEntityController = null;
                OneEntityController = null;
                AnotherEntityController = null;
            }
        }
    }
}
//MdEnd