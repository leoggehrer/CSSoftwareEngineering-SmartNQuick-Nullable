//@BaseCode
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;
#endif
    internal abstract partial class ControllerObject : IDisposable
    {
        #region Class-Constructors
        static ControllerObject()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class-Contructors

        #region Context
        private readonly bool contextOwner;
        internal DataContext.IContext Context { get; private set; }
        #endregion Context

#if ACCOUNT_ON
        #region SessionToken
        protected event EventHandler ChangedSessionToken;

        private string sessionToken;

        /// <summary>
        /// Sets the session token.
        /// </summary>
        public string SessionToken
        {
            internal get => sessionToken;
            set
            {
                sessionToken = value;
                ChangedSessionToken?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HandleChangedSessionToken(object source, EventArgs e)
        {
            var handled = false;

            BeforeHandleManagedMembers(ref handled);

            if (handled == false)
            {
                ControllerManagedProperties.ForEach(item =>
                {
                    if (item.GetValue(this) is ControllerObject controllerObject)
                    {
                        controllerObject.SessionToken = SessionToken;
                    }
                });
                ControllerManagedFields.ForEach(item =>
                {
                    if (item.GetValue(this) is ControllerObject controllerObject)
                    {
                        controllerObject.SessionToken = SessionToken;
                    }
                });
            }
            AfterHandleManagedMembers();
        }
        partial void BeforeHandleManagedMembers(ref bool handled);
        partial void AfterHandleManagedMembers();
        #endregion SessionToken
#endif
        #region Instance-Constructors
        public ControllerObject(DataContext.IContext context)
        {
            context.CheckArgument(nameof(context));

            Constructing();
            contextOwner = true;
            Context = context;
#if ACCOUNT_ON
            ChangedSessionToken += HandleChangedSessionToken;
#endif
            InitManagedMembers();
            Constructed();
        }
        public ControllerObject(ControllerObject other)
        {
            other.CheckArgument(nameof(other));

            Constructing();
            contextOwner = false;
            Context = other.Context;
#if ACCOUNT_ON
            SessionToken = other.SessionToken;
            ChangedSessionToken += HandleChangedSessionToken;
#endif
            InitManagedMembers();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        #endregion Instance-Constructors

        #region Handle managed members
        private IEnumerable<FieldInfo> ControllerManagedFields => GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                                                           .Where(p => p.GetCustomAttributes<Attributes.ControllerManagedFieldAttribute>()
                                                                           .Any());
        private IEnumerable<PropertyInfo> ControllerManagedProperties => GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                                                                  .Where(p => p.GetCustomAttributes<Attributes.ControllerManagedPropertyAttribute>()
                                                                                  .Any());
        private static ConstructorInfo GetControllerConstructor(Type type) => type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                                                                  .Single(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(ControllerObject));

        protected virtual void InitManagedMembers()
        {
            var handled = false;

            BeforeInitManagedMembers(ref handled);

            if (handled == false)
            {
                ControllerManagedProperties.ForEach(item =>
                {
                    var constructor = GetControllerConstructor(item.PropertyType);

                    if (constructor?.Invoke(new ControllerObject[] { this }) is ControllerObject controllerObject)
                    {
                        item.SetValue(this, controllerObject);
                    }
                });
                ControllerManagedFields.ForEach(item =>
                {
                    var constructor = GetControllerConstructor(item.FieldType);

                    if (constructor?.Invoke(new ControllerObject[] { this }) is ControllerObject controllerObject)
                    {
                        item.SetValue(this, controllerObject);
                    }
                });
            }
            AfterInitManagedMembers();
        }
        partial void BeforeInitManagedMembers(ref bool handled);
        partial void AfterInitManagedMembers();

        protected virtual void DisposeManagedMembers()
        {
            var handled = false;

            BeforeDisposeManagedMembers(ref handled);

            if (handled == false)
            {
                ControllerManagedProperties.ForEach(item =>
                {
                    if (item.GetValue(this) is ControllerObject controllerObject)
                    {
                        System.Diagnostics.Debug.WriteLine($"Dispose object {controllerObject.GetType().Name}");
                        controllerObject.Dispose();
                    }
                    item.SetValue(this, null);
                });
                ControllerManagedFields.ForEach(item =>
                {
                    if (item.GetValue(this) is ControllerObject controllerObject)
                    {
                        System.Diagnostics.Debug.WriteLine($"Dispose object {controllerObject.GetType().Name}");
                        controllerObject.Dispose();
                    }
                    item.SetValue(this, null);
                });
            }
            AfterDisposeManagedMembers();
        }
        partial void BeforeDisposeManagedMembers(ref bool handled);
        partial void AfterDisposeManagedMembers();
        #endregion Handle managed members

        #region SaveChanges
        internal virtual async Task BeforeSaveChangesAsync()
        {
            foreach (var item in ControllerManagedProperties)
            {
                if (item.GetValue(this) is ControllerObject controllerObject)
                {
                    await controllerObject.BeforeSaveChangesAsync().ConfigureAwait(false);
                }
            }
            foreach (var item in ControllerManagedFields)
            {
                if (item.GetValue(this) is ControllerObject controllerObject)
                {
                    await controllerObject.BeforeSaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
        public virtual async Task<int> SaveChangesAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Save).ConfigureAwait(false);
#endif
            return await ExecuteSaveChangesAsync().ConfigureAwait(false);
        }
        internal async Task<int> ExecuteSaveChangesAsync()
        {
            await BeforeSaveChangesAsync().ConfigureAwait(false);
            var result = await Context.SaveChangesAsync().ConfigureAwait(false);
            await AfterSaveChangesAsync().ConfigureAwait(false);
            return result;
        }
        internal virtual async Task AfterSaveChangesAsync()
        {
            foreach (var item in ControllerManagedProperties)
            {
                if (item.GetValue(this) is ControllerObject controllerObject)
                {
                    await controllerObject.AfterSaveChangesAsync().ConfigureAwait(false);
                }
            }
            foreach (var item in ControllerManagedFields)
            {
                if (item.GetValue(this) is ControllerObject controllerObject)
                {
                    await controllerObject.AfterSaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
        internal virtual async Task SaveChangesDirectAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Save).ConfigureAwait(false);
#endif
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        internal virtual async Task BeforeRejectChangesAsync()
        {
            foreach (var item in ControllerManagedProperties)
            {
                if (item.GetValue(this) is ControllerObject controllerObject)
                {
                    await controllerObject.BeforeRejectChangesAsync().ConfigureAwait(false);
                }
            }
            foreach (var item in ControllerManagedFields)
            {
                if (item.GetValue(this) is ControllerObject controllerObject)
                {
                    await controllerObject.BeforeRejectChangesAsync().ConfigureAwait(false);
                }
            }
        }
        public virtual async Task<int> RejectChangesAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Reject).ConfigureAwait(false);
#endif
            return await ExecuteRejectChangesAsync().ConfigureAwait(false);
        }
        internal async Task<int> ExecuteRejectChangesAsync()
        {
            await BeforeSaveChangesAsync().ConfigureAwait(false);
            var result = await Context.RejectChangesAsync().ConfigureAwait(false);
            await AfterSaveChangesAsync().ConfigureAwait(false);
            return result;
        }
        internal virtual async Task AfterRejectChangesAsync()
        {
            foreach (var item in ControllerManagedProperties)
            {
                if (item.GetValue(this) is ControllerObject controllerObject)
                {
                    await controllerObject.AfterRejectChangesAsync().ConfigureAwait(false);
                }
            }
            foreach (var item in ControllerManagedFields)
            {
                if (item.GetValue(this) is ControllerObject controllerObject)
                {
                    await controllerObject.AfterRejectChangesAsync().ConfigureAwait(false);
                }
            }
        }
        internal virtual async Task RejectChangesDirectAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Reject).ConfigureAwait(false);
#endif
            await Context.RejectChangesAsync().ConfigureAwait(false);
        }
        #endregion SaveChanges

#if ACCOUNT_ON
        #region Authorization
        protected virtual Task CheckAuthorizationAsync(Type subjectType, MethodBase methodBase, AccessType accessType)
        {
            return CheckAuthorizationAsync(subjectType, methodBase, accessType, null);
        }
        protected virtual async Task CheckAuthorizationAsync(Type subjectType, MethodBase methodBase, AccessType accessType, string infoData)
        {
            subjectType.CheckArgument(nameof(subjectType));
            methodBase.CheckArgument(nameof(methodBase));

            bool handled = false;

            BeforeCheckAuthorization(subjectType, methodBase, accessType, ref handled);
            if (handled == false)
            {
                await Authorization.CheckAuthorizationAsync(SessionToken, subjectType, methodBase, accessType, infoData).ConfigureAwait(false);
            }
            AfterCheckAuthorization(subjectType, methodBase, accessType);
        }
        partial void BeforeCheckAuthorization(Type subjectType, MethodBase methodBase, AccessType accessType, ref bool handled);
        partial void AfterCheckAuthorization(Type subjectType, MethodBase methodBase, AccessType accessType);
        #endregion Authorization
#endif

        #region Dispose pattern
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
#if ACCOUNT_ON
                    ChangedSessionToken -= HandleChangedSessionToken;
#endif
                    // TODO: dispose managed state (managed objects).
                    if (contextOwner && Context != null)
                    {
                        Context.Dispose();
                    }
                    DisposeManagedMembers();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                Context = null;
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ControllerObject()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion Dispose pattern
    }
}
//MdEnd