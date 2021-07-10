//@BaseCode
//MdStart
using CommonBase.Extensions;
using System;
using System.Linq;

namespace CSharpCodeGenerator.Logic.Generation
{
    internal partial class FactoryGenerator : ClassGenerator, Contracts.IFactoryGenerator
    {
        protected FactoryGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }
        public new static FactoryGenerator Create(SolutionProperties solutionProperties)
        {
            return new FactoryGenerator(solutionProperties);
        }

        #region General
        private static bool CanCreateLogicAccess(Type type)
        {
            bool create = true;

            CanCreateLogicAccess(type, ref create);
            return create;
        }
        static partial void CanCreateLogicAccess(Type type, ref bool create);
        private static bool CanCreateAdapterAccess(Type type)
        {
            bool create = true;

            CanCreateAdapterAccess(type, ref create);
            return create;
        }
        static partial void CanCreateAdapterAccess(Type type, ref bool create);
        #endregion General

        #region Logic
        public string LogicNameSpace => $"{SolutionProperties.LogicProjectName}";
        public Contracts.IGeneratedItem CreateLogicFactory()
        {
            var first = true;
            var contractsProject = ContractsProject.Create(SolutionProperties);
            var types = contractsProject.PersistenceTypes
                                        .Union(contractsProject.ShadowTypes)
                                        .Union(contractsProject.BusinessTypes);
            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.Factory)
            {
                FullName = $"{LogicNameSpace}.Factory",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = System.IO.Path.Combine($"FactoryPartA{StaticLiterals.CSharpFileExtension}"),
            };
            result.Add("public static partial class Factory");
            result.Add("{");
            result.Add("static partial void CreateController<I>(ref Contracts.Client.IControllerAccess<I> controller) where I : Contracts.IIdentifiable");
            result.Add("{");
            foreach (var type in types.Where(t => CanCreateLogicAccess(t)))
            {
                var entityName = CreateEntityNameFromInterface(type);
                var controllerNameSpace = $"Controllers.{CreateSubNamespaceFromType(type)}";

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"controller = new {controllerNameSpace}.{entityName}Controller(CreateContext()) as Contracts.Client.IControllerAccess<I>;");
                result.Add("}");
                first = false;
            }
            if (first == false)
            {
                result.Add("else");
                result.Add("{");
                result.Add("throw new Logic.Modules.Exception.LogicException(Modules.Exception.ErrorType.InvalidControllerType);");
                result.Add("}");
            }
            result.Add("}");

            result.Add("static partial void CreateController<I>(object sharedController, ref Contracts.Client.IControllerAccess<I> controller) where I : Contracts.IIdentifiable");
            result.Add("{");
            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t)))
            {
                var entityName = CreateEntityNameFromInterface(type);
                var controllerNameSpace = $"Controllers.{CreateSubNamespaceFromType(type)}";

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"controller = new {controllerNameSpace}.{entityName}Controller(sharedController as Controllers.ControllerObject) as Contracts.Client.IControllerAccess<I>;");
                result.Add("}");
                first = false;
            }
            if (first == false)
            {
                result.Add("else");
                result.Add("{");
                result.Add("throw new Logic.Modules.Exception.LogicException(Modules.Exception.ErrorType.InvalidControllerType);");
                result.Add("}");
            }
            result.Add("}");

            //result.Add("public static Contracts.Client.IControllerAccess<I> Create<I>(string sessionToken) where I : Contracts.IIdentifiable");
            //result.Add("{");
            //result.Add("Contracts.Client.IControllerAccess<I> result;");
            //first = true;
            //foreach (var type in types.Where(t => CanCreateLogicAccess(t)))
            //{
            //    var entityName = CreateEntityNameFromInterface(type);
            //    var controllerNameSpace = $"Controllers.{CreateSubNamespaceFromType(type)}";

            //    if (first)
            //    {
            //        result.Add($"if (typeof(I) == typeof({type.FullName}))");
            //    }
            //    else
            //    {
            //        result.Add($"else if (typeof(I) == typeof({type.FullName}))");
            //    }
            //    result.Add("{");
            //    result.Add($"result = new {controllerNameSpace}.{entityName}Controller(CreateContext()) " + "{ SessionToken = sessionToken } as Contracts.Client.IControllerAccess<I>;");
            //    result.Add("}");
            //    first = false;
            //}
            //result.Add("else");
            //result.Add("{");
            //result.Add("throw new Logic.Modules.Exception.LogicException(Modules.Exception.ErrorType.InvalidControllerType);");
            //result.Add("}");

            //result.Add("return result;");
            //result.Add("}");
            result.Add("}");

            result.AddRange(EnvelopeWithANamespace(result.Source.Eject(), LogicNameSpace));
            result.AddRange(result.Source.Eject().FormatCSharpCode());
            return result;
        }
        #endregion Logic

        #region Adapter
        public string AdapterNameSpace => $"{SolutionProperties.AdaptersProjectName}";
        public static string CreateControllerNameSpace(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"Controllers.{CreateSubNamespaceFromType(type)}";
        }
        public Contracts.IGeneratedItem CreateAdapterFactory()
        {
            var first = true;
            var contractsProject = ContractsProject.Create(SolutionProperties);
            var types = contractsProject.PersistenceTypes
                                        .Union(contractsProject.ShadowTypes)
                                        .Union(contractsProject.BusinessTypes);
            var result = new Models.GeneratedItem(Common.UnitType.Adapters, Common.ItemType.Factory)
            {
                FullName = $"{AdapterNameSpace}.Factory",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = System.IO.Path.Combine($"FactoryPartA{StaticLiterals.CSharpFileExtension}"),
            };
            result.Add("public static partial class Factory");
            result.Add("{");
            result.Add("public static Contracts.Client.IAdapterAccess<I> Create<I>()");
            result.Add("{");
            result.Add("Contracts.Client.IAdapterAccess<I> result = null;");
            result.Add("if (Adapter == AdapterType.Controller)");
            result.Add("{");
            foreach (var type in types.Where(t => CanCreateLogicAccess(t) && CanCreateAdapterAccess(t)))
            {
                var entityName = CreateEntityNameFromInterface(type);
                var controllerNameSpace = CreateControllerNameSpace(type);

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new Controller.GenericControllerAdapter<{type.FullName}>() as Contracts.Client.IAdapterAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            result.Add("else if (Adapter == AdapterType.Service)");
            result.Add("{");

            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t) && CanCreateAdapterAccess(t)))
            {
                var modelName = CreateEntityNameFromInterface(type);
                var modelNameSpace = CreateTransferModelNameSpace(type);
                var extUri = modelName.EndsWith("s") ? $"{modelName}" : $"{modelName}s";

                ConvertExtUri(type, ref extUri);
                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new Service.GenericServiceAdapter<{type.FullName}, {modelNameSpace}.{modelName}>(BaseUri, \"{extUri}\")");
                result.Add(" as Contracts.Client.IAdapterAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            result.Add("return result;");
            result.Add("}");

            result.Add("public static Contracts.Client.IAdapterAccess<I> Create<I>(string sessionToken)");
            result.Add("{");
            result.Add("Contracts.Client.IAdapterAccess<I> result = null;");
            result.Add("if (Adapter == AdapterType.Controller)");
            result.Add("{");

            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t) && CanCreateAdapterAccess(t)))
            {
                var entityName = CreateEntityNameFromInterface(type);
                var controllerNameSpace = CreateControllerNameSpace(type);

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new Controller.GenericControllerAdapter<{type.FullName}>(sessionToken) as Contracts.Client.IAdapterAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            result.Add("else if (Adapter == AdapterType.Service)");
            result.Add("{");
            first = true;
            foreach (var type in types.Where(t => CanCreateLogicAccess(t) && CanCreateAdapterAccess(t)))
            {
                var modelName = CreateEntityNameFromInterface(type);
                var modelNameSpace = CreateTransferModelNameSpace(type);
                var extUri = modelName.EndsWith("s") ? $"{modelName}" : $"{modelName}s";

                ConvertExtUri(type, ref extUri);
                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"result = new Service.GenericServiceAdapter<{type.FullName}, {modelNameSpace}.{modelName}>(sessionToken, BaseUri, \"{extUri}\") as Contracts.Client.IAdapterAccess<I>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            result.Add("return result;");
            result.Add("}");

            result.Add("}");
            result.AddRange(EnvelopeWithANamespace(result.Source.Eject(), AdapterNameSpace));
            result.AddRange(result.Source.Eject().FormatCSharpCode());
            return result;
        }
        static partial void ConvertExtUri(Type type, ref string extUri);
        #endregion
    }
}
//MdEnd