//@BaseCode
//MdStart
using CSharpCodeGenerator.Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpCodeGenerator.Logic.Generation
{
    internal abstract partial class ModelGenerator : ClassGenerator, Contracts.IModelGenerator
    {
        #region Models
        internal static string ModelObject => nameof(ModelObject);
        internal static string ModuleModel => nameof(ModuleModel);
        internal static string IdentityModel => nameof(IdentityModel);
        internal static string VersionModel => nameof(VersionModel);

        internal static string CompositeModel => nameof(CompositeModel);
        internal static string OneToAnotherModel => nameof(OneToAnotherModel);
        internal static string OneToManyModel => nameof(OneToManyModel);
        internal static string ShadowModel => nameof(ShadowModel);
        #endregion Models

        protected ModelGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }

        public abstract Common.UnitType UnitType { get; }
        public abstract string AppPostfix { get; }
        public abstract string AppModelsNameSpace { get; }
        public abstract string ModelsFolder { get; }

        public string CreateModelsNamespace(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"{AppModelsNameSpace}.{GeneratorObject.CreateSubNamespaceFromType(type)}";
        }
        protected virtual bool CanCreate(Type type)
        {
            bool create = true;

            CanCreateModel(type, ref create);
            return create;
        }
        partial void CanCreateModel(Type type, ref bool create);
        partial void CreateModelAttributes(Type type, List<string> codeLines);
        protected virtual void CreateModelPropertyAttributes(ContractPropertyHelper propertyHelper, List<string> codeLines)
        {
            var handled = false;

            BeforeCreateModelPropertyAttributes(propertyHelper, codeLines, ref handled);
            if (handled == false)
            {
            }
            AfterCreateModelPropertyAttributes(propertyHelper, codeLines);
        }
        partial void BeforeCreateModelPropertyAttributes(ContractPropertyHelper propertyHelper, List<string> codeLines, ref bool handled);
        partial void AfterCreateModelPropertyAttributes(ContractPropertyHelper propertyHelper, List<string> codeLines);

        public virtual IEnumerable<Contracts.IGeneratedItem> GenerateAll()
        {
            var result = new List<Contracts.IGeneratedItem>();

            result.AddRange(CreateBusinessModels());
            result.AddRange(CreateModulesModels());
            result.AddRange(CreatePersistenceModels());
            result.AddRange(CreateShadowModels());
            result.AddRange(CreateThirdPartyModels());
            return result;
        }

        public virtual IEnumerable<Contracts.IGeneratedItem> CreateBusinessModels()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.BusinessTypes)
            {
                if (CanCreate(type))
                {
                    result.Add(CreateModelFromContract(type, UnitType, Common.ItemType.BusinessModel));
                    if (ContractHelper.HasOneToMany(type))
                    {
                        var (one, _) = ContractHelper.GetOneToManyTypes(type);

                        result.Add(CreateDelegateProperties(type, one, StaticLiterals.OneItemName, UnitType, Common.ItemType.BusinessModel));
                    }
                    else if (ContractHelper.HasOneToAnother(type))
                    {
                        var (one, another) = ContractHelper.GetOneToAnotherTypes(type);

                        result.Add(CreateDelegateProperties(type, one, StaticLiterals.OneItemName, UnitType, Common.ItemType.BusinessModel));
                        result.Add(CreateDelegateProperties(type, another, StaticLiterals.AnotherItemName, UnitType, Common.ItemType.BusinessModel));
                    }
                    result.Add(CreateBusinessModel(type, UnitType));
                }
            }
            return result;
        }
        protected virtual Contracts.IGeneratedItem CreateBusinessModel(Type type, Common.UnitType unitType)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(unitType, Common.ItemType.BusinessModel)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, ModelsFolder, "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateModelNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        public virtual IEnumerable<Contracts.IGeneratedItem> CreateModulesModels()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.ModuleTypes)
            {
                if (CanCreate(type))
                {
                    result.Add(CreateModelFromContract(type, UnitType, Common.ItemType.ModuleModel));
                    result.Add(CreateModuleModel(type, UnitType));
                }
            }
            return result;
        }
        protected virtual Contracts.IGeneratedItem CreateModuleModel(Type type, Common.UnitType unitType)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(unitType, Common.ItemType.ModuleModel)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, ModelsFolder, "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateModelNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        public virtual IEnumerable<Contracts.IGeneratedItem> CreatePersistenceModels()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.PersistenceTypes)
            {
                if (CanCreate(type))
                {
                    result.Add(CreateModelFromContract(type, UnitType, Common.ItemType.PersistenceModel));
                    result.Add(CreatePersistenceModel(type, UnitType));
                    result.Add(CreateEditModelFromContract(type, UnitType, Common.ItemType.PersistenceModel));
                    //result.Add(CreateOverrideToString(type, UnitType));
                }
            }
            return result;
        }
        protected virtual Contracts.IGeneratedItem CreatePersistenceModel(Type type, Common.UnitType unitType)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(unitType, Common.ItemType.PersistenceModel)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, ModelsFolder, "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateModelNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        public virtual IEnumerable<Contracts.IGeneratedItem> CreateShadowModels()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.ShadowTypes)
            {
                if (CanCreate(type))
                {
                    result.Add(CreateModelFromContract(type, UnitType, Common.ItemType.ShadowModel));
                    result.Add(CreateShadowModel(type, UnitType));
                    result.Add(CreateEditModelFromContract(type, UnitType, Common.ItemType.ShadowModel));
                }
            }
            return result;
        }
        protected virtual Contracts.IGeneratedItem CreateShadowModel(Type type, Common.UnitType unitType)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(unitType, Common.ItemType.ShadowModel)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Models", "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateModelNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        public virtual IEnumerable<Contracts.IGeneratedItem> CreateThirdPartyModels()
        {
            var result = new List<Contracts.IGeneratedItem>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            foreach (var type in contractsProject.ThirdPartyTypes)
            {
                if (CanCreate(type))
                {
                    result.Add(CreateModelFromContract(type, UnitType, Common.ItemType.ThridPartyModel));
                    result.Add(CreateThirdPartyModel(type, UnitType));
                    result.Add(CreateEditModelFromContract(type, UnitType, Common.ItemType.ThridPartyModel));
                }
            }
            return result;
        }
        protected virtual Contracts.IGeneratedItem CreateThirdPartyModel(Type type, Common.UnitType unitType)
        {
            type.CheckArgument(nameof(type));

            var result = new Models.GeneratedItem(unitType, Common.ItemType.ThridPartyModel)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Models", "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateModelNameFromInterface(type)} : {GetBaseClassByContract(type)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        protected virtual Contracts.IGeneratedItem CreateModelFromContract(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            type.CheckArgument(nameof(type));

            var interfaces = GetInterfaces(type);
            var modelName = CreateModelNameFromInterface(type);
            var typeProperties = ContractHelper.GetAllProperties(type);
            var generateProperties = default(IEnumerable<PropertyInfo>);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Models", "", StaticLiterals.CSharpFileExtension),
            };
            CreateModelAttributes(type, result.Source);
            result.Add($"public partial class {modelName} : {type.FullName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(modelName));
            result.AddRange(CreatePartialConstrutor("public", modelName));

            if (itemType == Common.ItemType.ShadowModel)
            {
                generateProperties = ContractHelper.FilterShadowPropertiesForGeneration(type, typeProperties); 
            }
            else
            {
                generateProperties = ContractHelper.FilterPropertiesForGeneration(type, typeProperties);
            }
            foreach (var item in generateProperties)
            {
                var propertyHelper = new ContractPropertyHelper(type, item);

                CreateModelPropertyAttributes(propertyHelper, result.Source);
                result.AddRange(CreateProperty(propertyHelper));
            }
            result.AddRange(CreateCopyProperties(type));
            foreach (var item in interfaces.Where(e => ContractHelper.HasCopyable(e)))
            {
                result.AddRange(CreateCopyProperties(item));
            }
            result.AddRange(CreateFactoryMethods(type, false));
            result.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type), "using System;");
            result.FormatCSharpCode();
            return result;
        }
        protected virtual Contracts.IGeneratedItem CreateEditModelFromContract(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            type.CheckArgument(nameof(type));

            var interfaces = GetInterfaces(type);
            var modelName = CreateEditModelNameFromInterface(type);
            var typeProperties = ContractHelper.GetAllProperties(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, "Models", "", StaticLiterals.CSharpFileExtension),
            };
            IEnumerable<PropertyInfo> generateProperties;

            CreateModelAttributes(type, result.Source);
            result.Add($"public partial class {modelName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(modelName));
            result.AddRange(CreatePartialConstrutor("public", modelName));

            if (itemType == Common.ItemType.ShadowModel)
            {
                generateProperties = ContractHelper.FilterShadowPropertiesForGeneration(type, typeProperties);
            }
            else
            {
                generateProperties = ContractHelper.FilterPropertiesForGeneration(type, typeProperties);
            }
            foreach (var item in generateProperties)
            {
                var propertyHelper = new ContractPropertyHelper(type, item);

                CreateModelPropertyAttributes(propertyHelper, result.Source);
                result.AddRange(CreateProperty(propertyHelper));
            }
            result.AddRange(CreateCopyProperties(type, pi => ContractHelper.VersionProperties.Contains(pi.Name) == false));
            foreach (var item in interfaces.Where(e => ContractHelper.HasCopyable(e)))
            {
                result.AddRange(CreateCopyProperties(item, pi => ContractHelper.VersionProperties.Contains(pi.Name) == false));
            }
            result.AddRange(CreateFactoryMethods(type, false));
            result.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type), "using System;");
            result.FormatCSharpCode();
            return result;
        }

        private Contracts.IGeneratedItem CreateDelegateProperties(Type type, Type delegateType, string delegateObjectName, Common.UnitType unitType, Common.ItemType itemType)
        {
            type.CheckArgument(nameof(type));
            delegateType.CheckArgument(nameof(delegateType));

            var modelName = CreateModelNameFromInterface(type);
            var typeProperties = ContractHelper.GetAllProperties(delegateType);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, ModelsFolder, delegateObjectName, StaticLiterals.CSharpFileExtension),
            };
            CreateModelAttributes(type, result.Source);
            result.Add($"public partial class {modelName}");
            result.Add("{");
            foreach (var item in ContractHelper.FilterPropertiesForGeneration(type, typeProperties))
            {
                var propertyHelper = new ContractPropertyHelper(type, item);

                if (item.CanRead || item.CanWrite)
                {
                    CreateModelPropertyAttributes(propertyHelper, result.Source);
                    result.Add($"public {propertyHelper.PropertyFieldType} {item.Name}");
                    result.Add("{");
                    if (item.CanRead)
                    {
                        result.Add($"get => {delegateObjectName}.{item.Name};");
                    }
                    if (item.CanWrite)
                    {
                        result.Add($"set => {delegateObjectName}.{item.Name} = value;");
                    }
                    result.Add("}");
                }
            }
            result.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type), "using System;");
            result.FormatCSharpCode();
            return result;
        }

        protected string GetBaseClassByContract(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = string.Empty;
            var typeHelper = new ContractHelper(type);

            if (type.FullName.Contains(StaticLiterals.BusinessSubName))
            {
                result = IdentityModel;
                var itfcs = type.GetInterfaces();

                if (itfcs.Length > 0 && itfcs[0].Name.Equals(StaticLiterals.ICompositeName))
                {
                    var genericArgs = itfcs[0].GetGenericArguments();

                    if (genericArgs.Length == 3)
                    {
                        var connectorModel = $"{CreateModelFullNameFromInterface(genericArgs[0])}";
                        var oneModel = $"{CreateModelFullNameFromInterface(genericArgs[1])}";
                        var anotherModel = $"{CreateModelFullNameFromInterface(genericArgs[2])}";

                        result = $"{CompositeModel}<{genericArgs[0].FullName}, {connectorModel}, {genericArgs[1].FullName}, {oneModel}, {genericArgs[2].FullName}, {anotherModel}>";
                    }
                }
                else if (itfcs.Length > 0 && itfcs[0].Name.Equals(StaticLiterals.IOneToAnotherName))
                {
                    var genericArgs = itfcs[0].GetGenericArguments();

                    if (genericArgs.Length == 2)
                    {
                        var oneModel = $"{CreateModelFullNameFromInterface(genericArgs[0])}";
                        var anotherModel = $"{CreateModelFullNameFromInterface(genericArgs[1])}";

                        result = $"{OneToAnotherModel}<{genericArgs[0].FullName}, {oneModel}, {genericArgs[1].FullName}, {anotherModel}>";
                    }
                }
                else if (itfcs.Length > 0 && itfcs[0].Name.Equals(StaticLiterals.IOneToManyName))
                {
                    var genericArgs = itfcs[0].GetGenericArguments();

                    if (genericArgs.Length == 2)
                    {
                        var firstModel = $"{CreateModelFullNameFromInterface(genericArgs[0])}";
                        var secondModel = $"{CreateModelFullNameFromInterface(genericArgs[1])}";

                        result = $"{OneToManyModel}<{genericArgs[0].FullName}, {firstModel}, {genericArgs[1].FullName}, {secondModel}>";
                    }
                }
            }
            else if (type.FullName.Contains(StaticLiterals.PersistenceSubName))
            {
                if (typeHelper.IsVersionable)
                {
                    result = VersionModel;
                }
                else if (typeHelper.IsIdentifiable)
                {
                    result = IdentityModel;
                }
                else
                {
                    result = ModelObject;
                }
            }
            else if (type.FullName.Contains(StaticLiterals.ShadowSubName))
            {
                result = ShadowModel;
            }
            else if (typeHelper.IsVersionable)
            {
                result = VersionModel;
            }
            else if (typeHelper.IsIdentifiable)
            {
                result = IdentityModel;
            }
            else if (type.FullName.Contains(StaticLiterals.ModulesSubName))
            {
                result = ModuleModel;
            }
            return result;
        }
        protected string CreateModelFullNameFromInterface(Type type)
        {
            CheckInterfaceType(type);

            var modelName = CreateModelNameFromInterface(type);
            var subNamespace = $"{AppPostfix}.{(ModelsFolder.HasContent() ? $"{ModelsFolder}." : string.Empty)}";
            var result = type.FullName.Replace(type.Name, modelName);

            return result.Replace(".Contracts.", subNamespace);
        }

        protected virtual Contracts.IGeneratedItem CreateModelReferenceItems(Type type, IEnumerable<Type> types, Common.UnitType unitType, Common.ItemType itemType)
        {
            type.CheckArgument(nameof(type));

            var contractHelper = new ContractHelper(type);
            var masters = contractHelper.GetMasterTypes(types);
            var modelName = CreateModelNameFromInterface(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullNameFromInterface(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = CreateSubFilePathFromInterface(type, ModelsFolder, "PartR", StaticLiterals.CSharpFileExtension),
            };
            CreateModelAttributes(type, result.Source);
            result.Add($"public partial class {modelName}");
            result.Add("{");

			foreach (var item in masters)
			{
                result.Add($"public string {item.Name}String " + "{ get; set; }");
			}
            result.Add("}");
            result.EnvelopeWithANamespace(CreateModelsNamespace(type), "using System;");
            result.FormatCSharpCode();
            result.Source.Insert(0, $"// {nameof(CreateModelReferenceItems)}");
            return result;
        }
        //private Contracts.IGeneratedItem CreateOverrideToString(Type type, Common.UnitType unitType)
        //{
        //    type.CheckArgument(nameof(type));

        //    var result = new Models.GeneratedItem(unitType, Common.ItemType.Model)
        //    {
        //        FullName = CreateModelFullNameFromInterface(type),
        //        FileExtension = StaticLiterals.CSharpFileExtension,
        //    };
        //    result.SubFilePath = $"{result.FullName}PartA{result.FileExtension}";
        //    result.Source.Add($"partial class {CreateModelNameFromInterface(type)}");
        //    result.Source.Add("{");
        //    result.Source.Add("public override string ToString()");
        //    result.Source.Add("{");
        //    result.Source.Add("var result = string.Empty;");
        //    result.Source.Add("var handled = false;");
        //    result.Source.Add("BeforeToString(ref result, ref handled);");
        //    result.Source.Add("if (handled == false)");
        //    result.Source.Add("{");
        //    result.Source.Add("result = base.ToString();");
        //    result.Source.Add("}");
        //    result.Source.Add("AfterToString(ref result);");
        //    result.Source.Add("return result;");
        //    result.Source.Add("}");
        //    result.Source.Add("partial void BeforeToString(ref string result, ref bool handled);");
        //    result.Source.Add("partial void AfterToString(ref string result);");
        //    result.Source.Add("}");
        //    result.EnvelopeWithANamespace(CreateModelsNamespace(type));
        //    result.FormatCSharpCode();
        //    return result;
        //}
    }
}
//MdEnd