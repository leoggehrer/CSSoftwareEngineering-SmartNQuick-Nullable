//@BaseCode
//MdStart
using CommonBase.Extensions;
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

		#region SaveChanges
		protected virtual async Task BeforeSaveChangesAsync()
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
		protected virtual async Task AfterSaveChangesAsync()
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

		protected virtual async Task BeforeRejectChangesAsync()
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
		protected virtual async Task AfterRejectChangesAsync()
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
//MdEnd