//@BaseCode
//MdStart
using CommonBase.Extensions;
using CSharpCodeGenerator.Logic.Contracts;
using CSharpCodeGenerator.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpCodeGenerator.Logic.Generation
{
    internal abstract partial class GeneratorObject
    {
        static GeneratorObject()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public GeneratorObject()
        {
            Constructing();

            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public enum InterfaceType
        {
            Unknown,
            Root,
            Client,
            Business,
            Module,
            Persistence,
            Shadow,
        }
        public ISolutionProperties Properties => SolutionProperties;
        public SolutionProperties SolutionProperties { get; }
        public GeneratorObject(SolutionProperties solutionProperties)
        {
            solutionProperties.CheckArgument(nameof(solutionProperties));

            SolutionProperties = solutionProperties;
        }

        #region Helpers
        #region Namespace-Helpers
        public static IEnumerable<string> EnvelopeWithANamespace(IEnumerable<string> source, string nameSpace, params string[] usings)
        {
            var result = new List<string>();

            if (nameSpace.HasContent())
            {
                result.Add($"namespace {nameSpace}");
                result.Add("{");
                result.AddRange(usings);
            }
            result.AddRange(source);
            if (nameSpace.HasContent())
            {
                result.Add("}");
            }
            return result;
        }
        #endregion Namespace-Helpers

        #region Assemply-Helpers
        public static IEnumerable<Type> GetInterfaceTypes(Assembly assembly)
        {
            assembly.CheckArgument(nameof(assembly));

            return assembly.GetTypes().Where(t => t.IsInterface && t.IsPublic);
        }
        public static IEnumerable<Type> GetModulesTypes(Assembly assembly)
        {
            return GetInterfaceTypes(assembly)
                        .Where(t => t.IsInterface
                                 && t.FullName.Contains(StaticLiterals.ModulesSubName));
        }
        public static IEnumerable<Type> GetPersistenceTypes(Assembly assembly)
        {
            return GetInterfaceTypes(assembly)
                        .Where(t => t.IsInterface
                                 && t.FullName.Contains(StaticLiterals.PersistenceSubName));
        }
        public static IEnumerable<Type> GetBusinessTypes(Assembly assembly)
        {
            return GetInterfaceTypes(assembly)
                        .Where(t => t.IsInterface
                                 && t.FullName.Contains(StaticLiterals.BusinessSubName));
        }
        #endregion Assembly-Helpers

        #region Interface helpers
        public static InterfaceType GetInterfaceType(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = InterfaceType.Unknown;

            if (type.IsInterface)
            {
                if (type.FullName.EndsWith($"{StaticLiterals.RootSubName}{type.Name}"))
                    result = InterfaceType.Client;
                else if (type.FullName.Contains(StaticLiterals.ClientSubName))
                    result = InterfaceType.Client;
                else if (type.FullName.Contains(StaticLiterals.BusinessSubName))
                    result = InterfaceType.Business;
                else if (type.FullName.Contains(StaticLiterals.ModulesSubName))
                    result = InterfaceType.Module;
                else if (type.FullName.Contains(StaticLiterals.PersistenceSubName))
                    result = InterfaceType.Persistence;
                else if (type.FullName.Contains(StaticLiterals.ShadowSubName))
                    result = InterfaceType.Shadow;
            }
            return result;
        }
        public static IEnumerable<Type> GetInterfaces(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new List<Type>();

            static void GetInterfacesRec(Type type, List<Type> interfaces)
            {
                foreach (var item in type.GetInterfaces())
                {
                    if (interfaces.Contains(item) == false)
                    {
                        interfaces.Add(item);
                    }
                    GetInterfacesRec(item, interfaces);
                }
            }
            GetInterfacesRec(type, result);
            return result;
        }
        public static Type GetTypeInterface(Type type)
        {
            type.CheckArgument(nameof(type));
            type.CheckInterface(nameof(type));

            var result = default(Type);
            var interfaceType = GetInterfaceType(type);

            if (interfaceType == InterfaceType.Business)
            {
                result = type.GetInterfaces().FirstOrDefault(i => i.Namespace.Contains(StaticLiterals.BusinessSubName));
            }
            else if (interfaceType == InterfaceType.Module)
            {
                result = type.GetInterfaces().FirstOrDefault(i => i.Namespace.Contains(StaticLiterals.ModulesSubName));
            }
            else if (interfaceType == InterfaceType.Persistence)
            {
                result = type.GetInterfaces().FirstOrDefault(i => i.Namespace.Contains(StaticLiterals.PersistenceSubName));
            }
            else if (interfaceType == InterfaceType.Shadow)
            {
                result = type.GetInterfaces().FirstOrDefault(i => i.Namespace.Contains(StaticLiterals.ShadowSubName));
            }
            return result;
        }
        #endregion Interface helpers

        /// <summary>
        /// Diese Methode ueberprueft, ob der Typ ein Schnittstellen-Typ ist. Wenn nicht,
        /// dann wirft die Methode eine Ausnahme.
        /// </summary>
        /// <param name="type">Der zu ueberpruefende Typ.</param>
        public static void CheckInterfaceType(Type type)
        {
            type.CheckArgument(nameof(type));

            if (type.IsInterface == false)
                throw new ArgumentException($"The parameter '{nameof(type)}' must be an interface.");
        }
        /// <summary>
        /// Diese Methode ermittelt den Solutionname aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Schema der Entitaet.</returns>
        public static string GetSolutionNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;
            var data = type.Namespace.Split('.');

            if (data.Length > 0)
            {
                result = data[0];
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Teilnamensraum aus einem Typ.
        /// </summary>
        /// <param name="type">Typ</param>
        /// <returns>Teil-Namensraum</returns>
        public static string CreateSubNamespaceFromType(Type type)
        {
            var result = string.Empty;
            var data = type.Namespace.Split('.');

            for (var i = 2; i < data.Length; i++)
            {
                if (string.IsNullOrEmpty(result))
                {
                    result = $"{data[i]}";
                }
                else
                {
                    result = $"{result}.{data[i]}";
                }
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Teil-Path aus einem Typ.
        /// </summary>
        /// <param name="type">Typ</param>
        /// <returns>Teil-Path</returns>
        public static string CreateSubPathFromType(Type type)
        {
            return CreateSubNamespaceFromType(type).Replace(".", "/");
        }

        /// <summary>
        /// Diese Methode ermittelt den Entity Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name der Entitaet.</returns>
        public static string CreateEntityNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.IsInterface)
            {
                result = type.Name[1..];
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Model Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name des Models.</returns>
        public static string CreateModelNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.IsInterface)
            {
                result = type.Name[1..];
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Entity-Typ aus seiner Schnittstellen.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Typ der Entitaet.</returns>
        public static string CreateEntityTypeFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.IsInterface)
            {
                var entityName = CreateEntityNameFromInterface(type);

                result = $"{CreateSubNamespaceFromType(type)}.{entityName}";
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Entity Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name der Entitaet.</returns>
        public static string CreateEntityFullNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.IsInterface)
            {
                var entityName = CreateEntityNameFromInterface(type);

                result = type.FullName.Replace(type.Name, entityName);
                result = result.Replace(".Contracts", ".Logic.Entities");
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Kontroller Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name des Kontrollers.</returns>
        public static string CreateLogicControllerFullNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.IsInterface)
            {
                var entityName = type.Name[1..];

                result = type.FullName.Replace(type.Name, entityName);
                result = result.Replace(".Contracts", ".Logic.Controllers");
                result = $"{result}Controller";
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Kontroller Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name des Kontrollers.</returns>
        public static string CreateWebApiControllerFullNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var result = string.Empty;

            if (type.IsInterface)
            {
                var entityName = $"{type.Name[1..]}s";

                result = type.FullName.Replace(type.Name, entityName);
                result = result.Replace(".Contracts", ".WebApi.Controllers");
                result = $"{result}Controller";
            }
            return result;
        }

        #region Property-Helpers
        /// <summary>
        /// Diese Methode konvertiert den Eigenschaftstyp in eine Zeichenfolge.
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <returns>Der Eigenschaftstyp als Zeichenfolge.</returns>
        public static string GetPropertyType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.GetCodeDefinition();
        }
        /// <summary>
        /// Diese Methode ermittelt den Feldnamen der Eigenschaft.
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <param name="prefix">Prefix der dem Namen vorgestellt ist.</param>
        /// <returns>Der Feldname als Zeichenfolge.</returns>
        public static string CreateFieldName(PropertyInfo propertyInfo, string prefix)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            return $"{prefix}{char.ToLower(propertyInfo.Name.First())}{propertyInfo.Name[1..]}";
        }
        #endregion Property-Helpers
        #endregion Helpers
    }
}
//MdEnd