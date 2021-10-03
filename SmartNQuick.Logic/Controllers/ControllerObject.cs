//@BaseCode
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartNQuick.Logic.Controllers
{
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

        private void HandleChangeSessionToken(object source, EventArgs e)
        {
            var handled = false;

            BeforeHandleManagedMembers(ref handled);

            if (handled == false)
            {
                ControllerManagedProperties.ForeachAction(item =>
                {
                    if (item.GetValue(this) is ControllerObject controllerObject)
                    {
                        controllerObject.SessionToken = SessionToken;
                    }
                });
                ControllerManagedFields.ForeachAction(item =>
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
            ChangedSessionToken += HandleChangeSessionToken;
#endif
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
            ChangedSessionToken += HandleChangeSessionToken;
#endif
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        #endregion Instance-Constructors

        private IEnumerable<FieldInfo> ControllerManagedFields => GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                                                           .Where(p => p.GetCustomAttributes<Attributes.ControllerManagedFieldAttribute>()
                                                                           .Any());
        private IEnumerable<PropertyInfo> ControllerManagedProperties => GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                                                                  .Where(p => p.GetCustomAttributes<Attributes.ControllerManagedPropertyAttribute>()
                                                                                  .Any());

        #region Dispose pattern
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if (contextOwner)
                    {
                        Context.Dispose();
                    }
                    Context = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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
