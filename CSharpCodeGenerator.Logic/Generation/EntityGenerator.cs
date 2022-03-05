//@BaseCode
//MdStart
using CSharpCodeGenerator.Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpCodeGenerator.Logic.Generation
{
    internal partial class EntityGenerator : ClassGenerator, Contracts.IEntityGenerator
    {
        #region Models
        internal static string ModuleObject => nameof(ModuleObject);
        internal static string EntityObject => nameof(EntityObject);
        internal static string IdentityEntity => nameof(IdentityEntity);
        internal static string VersionEntity => nameof(VersionEntity);

        internal static string CompositeEntity => nameof(CompositeEntity);
        internal static string OneToAnotherEntity => nameof(OneToAnotherEntity);
        internal static string OneToManyEntity => nameof(OneToManyEntity);
        internal static string ShadowEntity => nameof(ShadowEntity);
        #endregion Models

        protected EntityGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }
        public new static Contracts.IEntityGenerator Create(SolutionProperties solutionProperties)
        {
            return new EntityGenerator(solutionProperties);
        }

        public string EntityNameSpace => $"{SolutionProperties.LogicProjectName}.{StaticLiterals.EntitiesFolder}";

        public string CreateNameSpace(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"{EntityNameSpace}.{GeneratorObject.CreateSubNamespaceFromType(type)}";
        }
        private bool CanCreateEntity(Type type)
        {
            bool create = true;

            CanCreateEntity(type, ref create);
            return create;
        }
        partial void CanCreateEntity(Type type, ref bool create);
        private bool CanCreateProperty(Type type, string propertyName)
        {
            bool create = true;

            CanCreateProperty(type, propertyName, ref create);
            return create;
        }
        partial void CanCreateProperty(Type type, string propertyName, ref bool create);
        partial void CreateEntityAttributes(Type type, List<string> codeLines);

        public virtual IEnumerable<Contracts.IGeneratedItem> GenerateAll()
        {
            var result = new List<Contracts.IGeneratedItem>();

            result.AddRange(CreateBusinessEntities());
            result.AddRange(CreateModulesEntities());
            result.AddRange(CreatePersistenceEntities());
            result.AddRange(CreateShadowEntities());
            return result;
        }

        public IEnumerable<Contracts.IGeneratedItem> CreateBusinessEntities()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.BusinessTypes)
            {
                if (CanCreateEntity(type))
                {
                    result.Add(CreateEntityFromContract(type, Common.ItemType.BusinessEntity));
                    result.Add(CreateBusinessEntity(type));
                }
            }
            return result;
        }
        private Contracts.IGeneratedItem CreateBusinessEntity(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.BusinessEntity)
            {
                FullName = CreateEntityFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Entities", "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateEntityNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateNameSpace(type));
            result.FormatCSharpCode();
            return result;
        }
        public IEnumerable<Contracts.IGeneratedItem> CreateModulesEntities()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.ModuleTypes)
            {
                if (CanCreateEntity(type))
                {
                    result.Add(CreateEntityFromContract(type, Common.ItemType.ModuleEntity));
                    result.Add(CreateModuleEntity(type));
                }
            }
            return result;
        }
        private Contracts.IGeneratedItem CreateModuleEntity(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.ModuleEntity)
            {
                FullName = CreateEntityFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Entities", "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateEntityNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateNameSpace(type));
            result.FormatCSharpCode();
            return result;
        }
        public IEnumerable<Contracts.IGeneratedItem> CreatePersistenceEntities()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);
            var persistenceTypes = contractsProject.PersistenceTypes;

            foreach (var type in persistenceTypes)
            {
                if (CanCreateEntity(type))
                {
                    result.Add(CreateEntityFromContract(type, Common.ItemType.PersistenceEntity));
                    result.Add(CreatePersistenceEntity(type));
                    result.Add(CreateEntityToEntityFromContracts(type, persistenceTypes));
                }
            }
            return result;
        }
        private Contracts.IGeneratedItem CreatePersistenceEntity(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.PersistenceEntity)
            {
                FullName = CreateEntityFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Entities", "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateEntityNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateNameSpace(type));
            result.FormatCSharpCode();
            return result;
        }
        public IEnumerable<Contracts.IGeneratedItem> CreateShadowEntities()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.ShadowTypes)
            {
                if (CanCreateEntity(type))
                {
                    result.Add(CreateEntityFromContract(type, Common.ItemType.ShadowEntity));
                    result.Add(CreateShadowEntity(type));
                }
            }
            return result;
        }
        private Contracts.IGeneratedItem CreateShadowEntity(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.ShadowEntity)
            {
                FullName = CreateEntityFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Entities", "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateEntityNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateNameSpace(type));
            result.FormatCSharpCode();
            return result;
        }

        /// <summary>
        /// Diese Methode erstellt den Programmcode der Beziehungen zwischen den Entitaeten aus den Schnittstellen-Typen.
        /// </summary>
        /// <param name="type">Der Schnittstellen-Typ.</param>
        /// <param name="types">Die Schnittstellen-Typen.</param>
        /// <param name="mapPropertyName">Ein Lambda-Ausdruck zum konvertieren des Eigenschaftsnamen.</param>
        /// <returns>Die Entitaet als Text.</returns>
        private Contracts.IGeneratedItem CreateEntityToEntityFromContracts(Type type, IEnumerable<Type> types)
        {
            type.CheckArgument(nameof(type));
            types.CheckArgument(nameof(types));

            var typeName = CreateEntityNameFromInterface(type);
            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.PersistenceEntity)
            {
                FullName = CreateEntityFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Entities", "Navigation", StaticLiterals.CSharpFileExtension),
            };
            result.Add($"partial class {typeName}");
            result.Add("{");

            foreach (var other in types)
            {
                var otherHelper = new ContractHelper(other);

                if (otherHelper.ContextType != CommonBase.Attributes.ContextType.View)
                {
                    var otherName = GeneratorObject.CreateEntityNameFromInterface(other);

                    foreach (var pi in other.GetProperties())
                    {
                        if (pi.Name.Equals($"{typeName}Id"))
                        {
                            var otherFullName = GeneratorObject.CreateEntityFullNameFromInterface(other);
                            var navigationName = $"{otherName}s";

                            result.Add(($"public System.Collections.Generic.ICollection<{otherFullName}> {navigationName} " + "{ get; set; }"));
                        }
                    }
                }
            }

            var interfaces = new List<Type>();
            var typeInterface = GetTypeInterface(type);

            interfaces.Add(type);
            foreach (var item in GeneratorObject.GetInterfaces(type)
                                          .Where(t => t != typeInterface))
            {
                interfaces.Add(item);
            }

            foreach (var item in interfaces)
            {
                foreach (var pi in item.GetProperties())
                {
                    foreach (var other in types)
                    {
                        var otherHelper = new ContractHelper(other);

                        if (otherHelper.ContextType != CommonBase.Attributes.ContextType.View)
                        {
                            var otherName = GeneratorObject.CreateEntityNameFromInterface(other);

                            if (pi.Name.Equals($"{otherName}Id"))
                            {
                                var propHelper = new ContractPropertyHelper(item, pi);
                                var otherFullName = GeneratorObject.CreateEntityFullNameFromInterface(other);
                                var navigationName = propHelper.NavigationName.GetValueOrDefault(otherName);

                                result.Add(($"[System.ComponentModel.DataAnnotations.Schema.ForeignKey(\"{pi.Name}\")]"));
                                result.Add(($"public {otherFullName} {navigationName} " + "{ get; set; }"));
                            }
                            else if (pi.Name.StartsWith($"{otherName}Id_"))
                            {
                                var data = pi.Name.Split("_");

                                if (data.Length == 2)
                                {
                                    var propHelper = new ContractPropertyHelper(item, pi);
                                    var otherFullName = CreateEntityFullNameFromInterface(other);
                                    var navigationName = propHelper.NavigationName.GetValueOrDefault(data[1]);

                                    result.Add(($"[System.ComponentModel.DataAnnotations.Schema.ForeignKey(\"{pi.Name}\")]"));
                                    result.Add(($"public {otherFullName} {navigationName} " + "{ get; set; }"));
                                }
                            }
                        }
                    }
                }
            }
            result.Add("}");
            result.EnvelopeWithANamespace(CreateNameSpace(type));
            result.FormatCSharpCode();
            return result;
        }
        private Contracts.IGeneratedItem CreateEntityFromContract(Type type, Common.ItemType itemType)
        {
            type.CheckArgument(nameof(type));

            var contractHelper = new ContractHelper(type);
            var baseInterfaces = GetPersistenceBaseContract(type);
            var typeProperties = ContractHelper.GetAllProperties(type);
            var delegateType = default(Type);
            var delegateEntityType = default(string);
            var delegateSourceName = "Source";
            var delegateProperties = default(IEnumerable<PropertyInfo>);
            var generateProperties = default(IEnumerable<PropertyInfo>);
            var result = new Models.GeneratedItem(Common.UnitType.Logic, itemType)
            {
                FullName = CreateEntityFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Entities", null, StaticLiterals.CSharpFileExtension),
            };
            CreateEntityAttributes(type, result.Source);
            result.Add($"partial class {contractHelper.EntityName} : {type.FullName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(contractHelper.EntityName));
            result.AddRange(CreatePartialConstrutor("public", contractHelper.EntityName));

            if (itemType == Common.ItemType.BusinessEntity && contractHelper.DelegateType != null)
            {
                delegateType = contractHelper.DelegateType;
                delegateEntityType = $"{CreateEntityFullNameFromInterface(delegateType)}";

                result.Add($"public {delegateEntityType} {delegateSourceName}" + " { get; set; }" + $" = new {delegateEntityType}();");
                result.Add($"public virtual void SetSource({delegateEntityType} source) => {delegateSourceName} = source;");

                delegateProperties = ContractHelper.GetAllProperties(delegateType)
                                                   .Where(p => CanCreateProperty(delegateType, p.Name));
                generateProperties = ContractHelper.GetAllProperties(type)
                                                   .Where(p => CanCreateProperty(type, p.Name));
            }
            else if (itemType == Common.ItemType.ShadowEntity)
            {
                var interfaceTypes = type.GetInterfaces();

                delegateType = interfaceTypes.Single(e => e.IsGenericType && e.Name.Equals(StaticLiterals.IShadowName)).GetGenericArguments()[0];
                delegateEntityType = $"{CreateEntityFullNameFromInterface(delegateType)}";

                result.Add($"public {delegateEntityType} {delegateSourceName}" + " { get; set; }");
                result.Add($"public override void SetSource(object source) => {delegateSourceName} = source as {delegateEntityType};");

                result.Add($"public override int Id ");
                result.Add("{");
                result.Add($"get => {delegateSourceName}.Id;");
                result.Add($"set => {delegateSourceName}.Id = value;");
                result.Add("}");

                delegateProperties = ContractHelper.GetEntityProperties(delegateType)
                                                   .Where(p => CanCreateProperty(delegateType, p.Name));
                generateProperties = ContractHelper.FilterShadowPropertiesForGeneration(type, typeProperties);
            }
            else
            {
                delegateProperties = Array.Empty<PropertyInfo>();
                generateProperties = ContractHelper.GetEntityProperties(type)
                                                   .Where(p => CanCreateProperty(type, p.Name));
            }

            foreach (var generateItem in generateProperties)
            {
                var propertyHelper = new ContractPropertyHelper(type, generateItem);
                var delegateItem = delegateProperties.FirstOrDefault(p => p.Name.Equals(generateItem.Name) && p.PropertyType.Equals(generateItem.PropertyType));
                var codeLines = new List<string>();

                if (delegateItem != null)
                {
                    var delegateHelper = new ContractPropertyHelper(delegateType, delegateItem);

                    codeLines.AddRange(CreateDelegateProperty(propertyHelper, delegateSourceName, delegateHelper));
                }
                else
                {
                    codeLines.AddRange(CreateProperty(propertyHelper));
                }

                AfterCreateProperty(propertyHelper, codeLines);
                result.AddRange(codeLines);
            }
            result.AddRange(CreateCopyProperties(type));
            result.AddRange(CreateEquals(type));
            result.AddRange(CreateGetHashCode(type));
            result.AddRange(CreateFactoryMethods(type, baseInterfaces != null));
            result.Add("}");
            result.EnvelopeWithANamespace(CreateNameSpace(type), "using System;");
            result.FormatCSharpCode();
            return result;
        }
        static partial void AfterCreateProperty(ContractPropertyHelper propertyHelper, List<string> codeLines);

        private static string GetBaseClassByContract(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = string.Empty;
            var typeHelper = new ContractHelper(type);

            if (type.FullName.Contains(StaticLiterals.BusinessSubName))
            {
                result = IdentityEntity;
                var itfcs = type.GetInterfaces();

                if (itfcs.Length > 0 && itfcs[0].Name.Equals(StaticLiterals.ICompositeName))
                {
                    var genericArgs = itfcs[0].GetGenericArguments();

                    if (genericArgs.Length == 3)
                    {
                        var connectorEntity = $"{CreateEntityFullNameFromInterface(genericArgs[0])}";
                        var oneEntity = $"{CreateEntityFullNameFromInterface(genericArgs[1])}";
                        var anotherEntity = $"{CreateEntityFullNameFromInterface(genericArgs[2])}";

                        result = $"{CompositeEntity}<{genericArgs[0].FullName}, {connectorEntity}, {genericArgs[1].FullName}, {oneEntity}, {genericArgs[2].FullName}, {anotherEntity}>";
                    }
                }
                else if (itfcs.Length > 0 && itfcs[0].Name.Equals(StaticLiterals.IOneToAnotherName))
                {
                    var genericArgs = itfcs[0].GetGenericArguments();

                    if (genericArgs.Length == 2)
                    {
                        var oneEntity = $"{CreateEntityFullNameFromInterface(genericArgs[0])}";
                        var anotherEntity = $"{CreateEntityFullNameFromInterface(genericArgs[1])}";

                        result = $"{OneToAnotherEntity}<{genericArgs[0].FullName}, {oneEntity}, {genericArgs[1].FullName}, {anotherEntity}>";
                    }
                }
                else if (itfcs.Length > 0 && itfcs[0].Name.Equals(StaticLiterals.IOneToManyName))
                {
                    var genericArgs = itfcs[0].GetGenericArguments();

                    if (genericArgs.Length == 2)
                    {
                        var firstEntity = $"{CreateEntityFullNameFromInterface(genericArgs[0])}";
                        var secondEntity = $"{CreateEntityFullNameFromInterface(genericArgs[1])}";

                        result = $"{OneToManyEntity}<{genericArgs[0].FullName}, {firstEntity}, {genericArgs[1].FullName}, {secondEntity}>";
                    }
                }
                else
                {
                    if (typeHelper.IsVersionable)
                    {
                        result = VersionEntity;
                    }
                    else if (typeHelper.IsIdentifiable)
                    {
                        result = IdentityEntity;
                    }
                    else
                    {
                        result = EntityObject;
                    }
                }
            }
            else if (type.FullName.Contains(StaticLiterals.PersistenceSubName))
            {
                var baseItf = GetPersistenceBaseContract(type);

                if (baseItf != null)
                {
                    result = CreateEntityFullNameFromInterface(baseItf);
                }
                else if (typeHelper.IsVersionable)
                {
                    result = VersionEntity;
                }
                else if (typeHelper.IsIdentifiable)
                {
                    result = IdentityEntity;
                }
                else
                {
                    result = EntityObject;
                }
            }
            else if (type.FullName.Contains(StaticLiterals.ShadowSubName))
            {
                result = ShadowEntity;
            }
            else if (typeHelper.IsVersionable)
            {
                result = VersionEntity;
            }
            else if (typeHelper.IsIdentifiable)
            {
                result = IdentityEntity;
            }
            else if (type.FullName.Contains(StaticLiterals.ModulesSubName))
            {
                result = ModuleObject;
            }

            return result;
        }
        private static Type GetPersistenceBaseContract(Type type)
        {
            type.CheckArgument(nameof(type));


            return type.GetInterfaces().FirstOrDefault(e => e.IsGenericType == false
                                                         && e.FullName.Contains(StaticLiterals.PersistenceSubName));
        }
    }
}
//MdEnd