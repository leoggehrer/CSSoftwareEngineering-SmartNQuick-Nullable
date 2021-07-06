//@BaseCode
using System.Collections.Generic;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace CSharpCodeGenerator.ConApp
{
	public static class StaticLiterals
	{
        public static string SolutionFileExtension => ".sln";
        public static string ProjectFileExtension => ".csproj";
        public static string SourceFileExtensions => CommonStaticLiterals.SourceFileExtensions;
        public static string CSharpFileExtension => CommonStaticLiterals.CSharpFileExtension;
        public static string GeneratedCodeLabel => CommonStaticLiterals.GeneratedCodeLabel;
        public static string CustomizedAndGeneratedCodeLabel => CommonStaticLiterals.CustomizedAndGeneratedCodeLabel;
        public static IDictionary<string, string> SourceFileHeaders => CommonStaticLiterals.SourceFileHeaders;

        public static string ContractsExtension => ".Contracts";
        public static string RootSubName => ".Contracts.";
        public static string ClientSubName => ".Client.";
        public static string BusinessSubName => ".Business.";
        public static string ModulesSubName => ".Modules.";
        public static string PersistenceSubName => ".Persistence.";
        public static string ShadowSubName => ".Shadow.";

        #region Folders
        public static string ContractsFolder => "Contracts";
        public static string EntitiesFolder => "Entities";
        public static string ControllersFolder => "Controllers";
        public static string ModelsFolder => "Models";
        public static string ModulesFolder => "Modules";
        public static string BusinessFolder => "Business";
        public static string PersistenceFolder => "Persistence";
        public static string ShadowFolder => "Shadow";
        #endregion Folders

        public static string EntitiesLabel => "Entities";
        public static string ModulesLabel => "Modules";
        public static string BusinessLabel => "Business";
        public static string PersistenceLabel => "Persistence";
        public static string ShadowLabel => "Shadow";

        public static string DelegatePropertyName => "DelegateObject";
        public static string IIdentifiableName => "IIdentifiable";
        public static string IVersionableName => "IVersionable";
        public static string ICopyableName => "ICopyable`1";

        public static string ICompositeName => "IComposite`3";
        public static string IOneToAnotherName => "IOneToAnother`2";
        public static string IOneToManyName => "IOneToMany`2";
        public static string IShadowName => "IShadow`1";

        public static string ConnectorItemName => "ConnectorItem";
        public static string OneItemName => "OneItem";
        public static string OneModelName => "OneModel";
        public static string AnotherItemName => "AnotherItem";
        public static string FirstItemName => "FirstItem";
        public static string SecondItemName => "SecondItem";
        public static string ManyItemName => "ManyItem";
        public static string ManyItemsName => "ManyItems";

    }
}
