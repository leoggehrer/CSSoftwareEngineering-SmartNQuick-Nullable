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
#if ACCOUNT_ON
            ChangedSessionToken += HandleChangedSessionToken;
#endif
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public GenericOneToAnotherController(ControllerObject controller) : base(controller)
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
            AnotherEntityController.SessionToken = SessionToken;
        }
#endif

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
            var qyr = await AnotherEntityController.QueryAllAsync(predicate).ConfigureAwait(false);

            if (qyr.Any())
            {
                if (AnotherEntityController.IsTransient)
                {
                    var another = await AnotherEntityController.GetByIdAsync(qyr.ElementAt(0).Id).ConfigureAwait(false);

                    entity.AnotherEntity.CopyProperties(another);
                }
                else
                {
                    entity.AnotherEntity.CopyProperties(qyr.ElementAt(0));
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
            var oneEntity = await OneEntityController.GetByIdAsync(id).ConfigureAwait(false);

            if (oneEntity != null)
            {
                result = new E();
                result.OneItem.CopyProperties(oneEntity);
                await LoadAnotherAsync(result, oneEntity.Id).ConfigureAwait(false);
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
        public override async Task<IEnumerable<C>> QueryAllAsync(string predicate)
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
            entity.OneItem.CheckArgument(nameof(entity.OneItem));
            entity.AnotherItem.CheckArgument(nameof(entity.AnotherItem));

            var result = new E();

            result.OneEntity.CopyProperties(entity.OneItem);
            await OneEntityController.InsertAsync(result.OneEntity).ConfigureAwait(false);

            result.AnotherEntity.CopyProperties(entity.AnotherItem);
            var pi = GetNavigationToOne();

            if (pi != null)
            {
                pi.SetValue(result.AnotherEntity, result.OneEntity);
            }
            await AnotherEntityController.InsertAsync(result.AnotherEntity).ConfigureAwait(false);
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
            var updOne = await OneEntityController.UpdateAsync(entity.OneItem).ConfigureAwait(false);

            result.OneEntity.CopyProperties(updOne);
            if (entity.AnotherItem.Id == 0)
            {
                var pi = GetForeignKeyToOne();

                if (pi != null)
                {
                    pi.SetValue(entity.AnotherItem, result.OneEntity.Id);
                }
                var insAnother = await AnotherEntityController.InsertAsync(entity.AnotherItem).ConfigureAwait(false);

                result.AnotherEntity.CopyProperties(insAnother);
            }
            else
            {
                var updAnother = await AnotherEntityController.UpdateAsync(entity.AnotherItem).ConfigureAwait(false);

                result.AnotherEntity.CopyProperties(updAnother);
            }
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        #endregion Update

        #region Delete
        internal override async Task<E> ExecuteDeleteEntityAsync(E entity)
        {
            if (entity.AnotherItem.Id > 0)
            {
                await AnotherEntityController.DeleteAsync(entity.AnotherItem.Id).ConfigureAwait(false);
            }
            await OneEntityController.DeleteAsync(entity.Id).ConfigureAwait(false);
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
                AnotherEntityController.Dispose();

                OneEntityController = null;
                AnotherEntityController = null;
            }
        }
    }
}
//MdEnd