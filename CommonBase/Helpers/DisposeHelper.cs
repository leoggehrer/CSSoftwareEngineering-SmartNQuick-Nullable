//@BaseCode
//MdStart

using CommonBase.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace CommonBase.Helpers
{
    public static partial class DisposeHelper
    {
        public static void DisposeMembers(object owner)
        {
            DisposeFields(owner);
            DisposeProperties(owner);
        }
        public static void DisposeFields(object owner)
        {
            owner.CheckArgument(nameof(owner));

            var disposeFields = owner.GetType()
                                     .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                     .Where(p => p.GetCustomAttributes<Attributes.DisposeFieldAttribute>()
                                     .Any());

            disposeFields.ForEach(item =>
            {
                if (item.GetValue(owner) is IDisposable disposeObject)
                {
                    disposeObject.Dispose();
                }
                item.SetValue(owner, null);
            });
        }
        public static void DisposeProperties(object owner)
        {
            owner.CheckArgument(nameof(owner));

            var disposeFields = owner.GetType()
                                     .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                     .Where(p => p.GetCustomAttributes<Attributes.DisposePropertyAttribute>()
                                     .Any());

            disposeFields.ForEach(item =>
            {
                if (item.GetValue(owner) is IDisposable disposeObject)
                {
                    disposeObject.Dispose();
                }
                item.SetValue(owner, null);
            });
        }
    }
}
//MdEnd