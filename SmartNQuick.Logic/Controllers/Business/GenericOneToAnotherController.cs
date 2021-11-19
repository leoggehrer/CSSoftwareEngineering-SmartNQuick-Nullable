//@BaseCode
//MdStart
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
    internal abstract partial class GenericOneToAnotherController<C, E, TOne, TOneEntity, TAnother, TAnotherEntity> : BusinessControllerAdapter<C, E>
        where C : Contracts.IOneToAnother<TOne, TAnother>
        where E : Entities.OneToAnotherEntity<TOne, TOneEntity, TAnother, TAnotherEntity>, C, Contracts.ICopyable<C>, new()
        where TOne : Contracts.IIdentifiable, Contracts.ICopyable<TOne>
        where TOneEntity : Entities.IdentityEntity, TOne, Contracts.ICopyable<TOne>, new()
        where TAnother : Contracts.IIdentifiable, Contracts.ICopyable<TAnother>
        where TAnotherEntity : Entities.IdentityEntity, TAnother, Contracts.ICopyable<TAnother>, new()
    {
        static GenericOneToAnotherController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        protected abstract GenericController<TOne, TOneEntity> OneEntityController { get; set; }
        protected abstract GenericController<TAnother, TAnotherEntity> AnotherEntityController { get; set; }

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
            return typeof(TAnotherEntity).GetProperty(typeof(TOneEntity).Name);
        }
        protected virtual PropertyInfo GetForeignKeyToOne()
        {
            return typeof(TAnother).GetProperty($"{typeof(TOneEntity).Name}Id");
        }
        protected virtual async Task LoadAnotherAsync(E entity, int masterId)
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
            var query = await AnotherEntityController.QueryAllAsync(predicate).ConfigureAwait(false);

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
            return OneEntityController.CountAsync();
        }
        internal override Task<int> ExecuteCountByAsync(string predicate)
        {
            return OneEntityController.CountByAsync(predicate);
        }
        #endregion Count

        #region Query
        internal override async Task<E> ExecuteGetEntityByIdAsync(int id)
        {
            E result;
            var oneEntity = await OneEntityController.GetEntityByIdAsync(id).ConfigureAwait(false);

            if (oneEntity != null)
            {
                result = new E
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
        internal override async Task<IEnumerable<E>> ExecuteGetEntityAllAsync()
        {
            var result = new List<E>();
            var query = await OneEntityController.GetAllAsync().ConfigureAwait(false);

            foreach (var item in query)
            {
                var entity = new E();

                entity.OneItem.CopyProperties(item);
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

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
                await LoadAnotherAsync(entity, item.Id).ConfigureAwait(false);

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
        internal override async Task<E> ExecuteUpdateEntityAsync(E entity)
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
        internal override async Task<E> ExecuteDeleteEntityAsync(E entity)
        {
            if (entity.AnotherEntity.Id > 0)
            {
                await AnotherEntityController.DeleteAsync(entity.AnotherEntity.Id).ConfigureAwait(false);
            }
            await OneEntityController.DeleteAsync(entity.Id).ConfigureAwait(false);
            return entity;
        }
        #endregion Delete
    }
}
//MdEnd