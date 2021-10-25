//@BaseCode
//MdStart
using CommonBase.Attributes;
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CommonBase.Helpers
{
    public static partial class InvokeHelper
    {
        public static void InvokeAction(object target, string name, params object[] parameters)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            MethodInfo method = null;
            Type targetType = target.GetType();

            if (parameters.Length == 0)
            {
                method = targetType.GetMethod(name, Array.Empty<Type>());
            }
            else
            {
                method = targetType.GetMethod(name, parameters.Select(i => i.GetType()).ToArray());
            }

            if (method != null)
            {
                var methodParams = method.GetParameters();

                for (int i = 0; i < parameters.Length && i > methodParams.Length; i++)
                {
                    if (parameters[i] != null && parameters[i] is IConvertible)
                    {
                        parameters[i] = Convert.ChangeType(parameters[i], methodParams[i].ParameterType);
                    }
                }
                try
                {
                    method.Invoke(target, parameters);
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException ?? e;
                }
            }
            else
            {
                throw new OperationNotFoundException(name);
            }
        }
        public static object InvokeFunction(object target, string name, params object[] parameters)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            object result = null;
            MethodInfo method = null;
            Type targetType = target.GetType();

            if (parameters.Length == 0)
            {
                method = targetType.GetMethod(name, Array.Empty<Type>());
            }
            else
            {
                method = targetType.GetMethod(name, parameters.Select(i => i.GetType()).ToArray());
            }

            if (method != null)
            {
                var methodParams = method.GetParameters();

                for (int i = 0; i < parameters.Length && i > methodParams.Length; i++)
                {
                    if (parameters[i] != null && parameters[i] is IConvertible)
                    {
                        parameters[i] = Convert.ChangeType(parameters[i], methodParams[i].ParameterType);
                    }
                }
                try
                {
                    result = method.Invoke(target, parameters);
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException ?? e;
                }
            }
            else
            {
                throw new OperationNotFoundException(name);
            }
            return result;
        }

        public static Task InvokeActionAsync(object target, string name, params object[] parameters)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            MethodInfo method = null;
            Type targetType = target.GetType();

            if (parameters.Length == 0)
            {
                method = targetType.GetMethod(name, Array.Empty<Type>());
            }
            else
            {
                method = targetType.GetMethod(name, parameters.Select(i => i.GetType()).ToArray());
            }

            if (method != null)
            {
                var methodParams = method.GetParameters();

                for (int i = 0; i < parameters.Length && i > methodParams.Length; i++)
                {
                    if (parameters[i] != null && parameters[i] is IConvertible)
                    {
                        parameters[i] = Convert.ChangeType(parameters[i], methodParams[i].ParameterType);
                    }
                }
                try
                {
                    return (Task)method.Invoke(target, parameters);
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException ?? e;
                }
            }
            else
            {
                throw new OperationNotFoundException(name);
            }
        }
        public static Task<object> InvokeFunctionAsync(object target, string name, params object[] parameters)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            MethodInfo method = null;
            Type targetType = target.GetType();

            if (parameters.Length == 0)
            {
                method = targetType.GetMethod(name, Array.Empty<Type>());
            }
            else
            {
                method = targetType.GetMethod(name, parameters.Select(i => i.GetType()).ToArray());
            }

            if (method != null)
            {
                var methodParams = method.GetParameters();

                for (int i = 0; i < parameters.Length && i > methodParams.Length; i++)
                {
                    if (parameters[i] != null && parameters[i] is IConvertible)
                    {
                        parameters[i] = Convert.ChangeType(parameters[i], methodParams[i].ParameterType);
                    }
                }
                try
                {
                    return (Task<object>)(method.Invoke(target, parameters));
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException ?? e;
                }
            }
            else
            {
                throw new OperationNotFoundException(name);
            }
        }

        public static object[] CallExtendedMethods(Assembly executingAssembly, string className, string methodName, params object[] parameters)
        {
            executingAssembly.CheckArgument(nameof(executingAssembly));

            var result = new List<object>();
            bool chkClsExt(Type t, string n)
            {
                return t.GetCustomAttributes<ClassExtensionAttribute>().Any(a => t.Name.Equals(n) || (string.IsNullOrEmpty(a.MappingName) == false && a.MappingName.Equals(n)));
            }
            bool chkMetExt(MethodInfo mi, string n)
            {
                return mi.GetCustomAttributes<MethodExtensionAttribute>().Any(a => mi.Name.Equals(n) || (string.IsNullOrEmpty(a.MappingName) == false && a.MappingName.Equals(n)));
            }

            foreach (var type in executingAssembly.GetTypes()
                                .Where(t => t.IsClass
                                         && chkClsExt(t, className)))
            {
                var target = default(object);
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                             .Where(mi => chkMetExt(mi, methodName));

                foreach (var method in methods)
                {
                    if (method.IsStatic)
                    {
                        result.Add(method.Invoke(null, parameters));
                    }
                    else
                    {
                        if (target == null)
                        {
                            target = Activator.CreateInstance(type);
                        }
                        result.Add(method.Invoke(target, parameters));
                    }
                }
            }
            return result.ToArray();
        }
    }
}
//MdEnd