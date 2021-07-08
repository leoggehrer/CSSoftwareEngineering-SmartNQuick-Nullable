//@BaseCode
//MdStart
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharpCodeGenerator.Logic.Generation
{
    internal partial class SolutionProperties : Contracts.ISolutionProperties
    {
        public static string GeneratedCodeFileName => "_GeneratedCode.cs";

        #region Project-postfixes
        public static string ContractsPostfix => ".Contracts";
        public static string LogicPostfix => ".Logic";
        public static string TransferPostfix => ".Transfer";
        public static string WebApiPostfix => ".WebApi";
        public static string AdaptersPostfix => ".Adapters";
        public static string AspMvcPostfix => ".AspMvc";
        public static string BlazorAppPostfix => ".BlazorApp";
        public static string TelerikServerBlazorAppPostfix => ".TelerikServerBlazorApp";
        public static string AngularAppPostfix => ".AngularApp";
        public static string ConnectorPostfix => ".Connector";
        #endregion Project-postfixes

        #region ProjectNames
        public IEnumerable<string> ProjectNames => CommonBase.StaticLiterals.ProjectExtensions.Select(e => $"{SolutionName}{e}");
        public string ContractsProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{ContractsPostfix}"));
        public string LogicProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{LogicPostfix}"));
        public string TransferProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{TransferPostfix}"));
        public string WebApiProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{WebApiPostfix}"));
        public string AdaptersProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{AdaptersPostfix}"));
        public string AspMvcProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{AspMvcPostfix}"));
        public string BlazorAppProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{BlazorAppPostfix}"));
        public string TelerikServerBlazorAppProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{TelerikServerBlazorAppPostfix}"));
        public string AngularAppProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{AngularAppPostfix}"));
        public string ConnectorProjectName => ProjectNames.FirstOrDefault(e => e.EndsWith($"{ConnectorPostfix}"));
        #endregion ProjectNames

        #region ProjectPaths
        public string SolutionName { get; }
        public string ContractsFilePath { get; }
        public string SolutionPath { get; }

        public string ContractsSubPath => ContractsProjectName;
        public string LogicSubPath => LogicProjectName;
        public string TransferSubPath => TransferProjectName;
        public string WebApiSubPath => WebApiProjectName;
        public string AdaptersSubPath => AdaptersProjectName;
        public string AspMvcSubPath => AspMvcProjectName;
        public string ConnectorSubPath => ConnectorProjectName;
        #endregion ProjectPaths


        #region Entities
        public string EntitiesSubPath => StaticLiterals.EntitiesFolder;
        public string EntitiesBusinessSubPath => Path.Combine(LogicSubPath, EntitiesSubPath, StaticLiterals.BusinessFolder);
        public string EntitiesModulesSubPath => Path.Combine(LogicSubPath, EntitiesSubPath, StaticLiterals.ModulesFolder);
        public string EntitiesPersitenceSubPath => Path.Combine(LogicSubPath, EntitiesSubPath, StaticLiterals.PersistenceFolder);
        public string EntitiesShadowSubPath => Path.Combine(LogicSubPath, EntitiesSubPath, StaticLiterals.ShadowFolder);

        public string EntitiesBusinessFileSubPath => Path.Combine(EntitiesBusinessSubPath, GeneratedCodeFileName);
        public string EntitiesModulesFileSubPath => Path.Combine(EntitiesModulesSubPath, GeneratedCodeFileName);
        public string EntitiesPersistenceFileSubPath => Path.Combine(EntitiesPersitenceSubPath, GeneratedCodeFileName);
        public string EntitiesShadowFileSubPath => Path.Combine(EntitiesShadowSubPath, GeneratedCodeFileName);
        #endregion Entities

        #region DataContext
        public static string DataContextFolder => "DataContext";
        public string DataContextSubPath => DataContextFolder;
        public string DataContextDbFolder => "Db";
        public string DataContextPersistenceSubPath => Path.Combine(LogicSubPath, DataContextSubPath);
        public string DataContextPersistenceFileSubPath => Path.Combine(DataContextPersistenceSubPath, GeneratedCodeFileName);
        #endregion DataContext

        #region Controllers
        public string ControllersSubPath => StaticLiterals.ControllersFolder;
        public string ControllersBusinessSubPath => Path.Combine(LogicSubPath, ControllersSubPath, StaticLiterals.BusinessFolder);
        public string ControllersPersistenceSubPath => Path.Combine(LogicSubPath, ControllersSubPath, StaticLiterals.PersistenceFolder);
        public string ControllersShadowSubPath => Path.Combine(LogicSubPath, ControllersSubPath, StaticLiterals.ShadowFolder);

        public string ControllersBusinessFileSubPath => Path.Combine(ControllersBusinessSubPath, GeneratedCodeFileName);
        public string ControllersPersistenceFileSubPath => Path.Combine(ControllersPersistenceSubPath, GeneratedCodeFileName);
        public string ControllersShadowFileSubPath => Path.Combine(ControllersShadowSubPath, GeneratedCodeFileName);
        #endregion Controllers

        #region Logic-Factory
        public string LogicFactorySubPath => Path.Combine(LogicSubPath);
        public string LogicFactoryFileSubPath => Path.Combine(LogicFactorySubPath, GeneratedCodeFileName);
        #endregion Logic-Factory

        #region Transfer
        public string TransferBusinessSubPath => Path.Combine(TransferSubPath, StaticLiterals.BusinessFolder);
        public string TransferModulesSubPath => Path.Combine(TransferSubPath, StaticLiterals.ModulesFolder);
        public string TransferPersistenceSubPath => Path.Combine(TransferSubPath, StaticLiterals.PersistenceFolder);
        public string TransferShadowSubPath => Path.Combine(TransferSubPath, StaticLiterals.ShadowFolder);

        public string TransferBusinessFileSubPath => Path.Combine(TransferBusinessSubPath, GeneratedCodeFileName);
        public string TransferModulesFileSubPath => Path.Combine(TransferModulesSubPath, GeneratedCodeFileName);
        public string TransferPersistenceFileSubPath => Path.Combine(TransferPersistenceSubPath, GeneratedCodeFileName);
        public string TransferShadowFileSubPath => Path.Combine(TransferShadowSubPath, GeneratedCodeFileName);
        #endregion Transfer

        #region WebApi
        public string WebApiControllersSubPath => Path.Combine(WebApiSubPath, StaticLiterals.ControllersFolder);
        public string WebApiControllersFileSubPath => Path.Combine(WebApiControllersSubPath, GeneratedCodeFileName);
        #endregion WebApi

        #region Adapters-Factory
        public string AdaptersFactorySubPath => Path.Combine(AdaptersSubPath);
        public string AdaptersFactoryFileSubPath => Path.Combine(AdaptersFactorySubPath, GeneratedCodeFileName);
        #endregion Adapters-Factory

        #region AspMvc
        public string AspMvcBusinessSubPath => Path.Combine(AspMvcSubPath, StaticLiterals.ModelsFolder, StaticLiterals.BusinessFolder);
        public string AspMvcModulesSubPath => Path.Combine(AspMvcSubPath, StaticLiterals.ModelsFolder, StaticLiterals.ModulesFolder);
        public string AspMvcPersistenceSubPath => Path.Combine(AspMvcSubPath, StaticLiterals.ModelsFolder, StaticLiterals.PersistenceFolder);
        public string AspMvcShadowSubPath => Path.Combine(AspMvcSubPath, StaticLiterals.ModelsFolder, StaticLiterals.ShadowFolder);

        public string AspMvcBusinessFileSubPath => Path.Combine(AspMvcBusinessSubPath, GeneratedCodeFileName);
        public string AspMvcModulesFileSubPath => Path.Combine(AspMvcModulesSubPath, GeneratedCodeFileName);
        public string AspMvcPersistenceFileSubPath => Path.Combine(AspMvcPersistenceSubPath, GeneratedCodeFileName);
        public string AspMvcShadowFileSubPath => Path.Combine(AspMvcShadowSubPath, GeneratedCodeFileName);
        #endregion AspMvc

        protected SolutionProperties(string solutionPath)
        {
            solutionPath.CheckArgument(nameof(solutionPath));

            SolutionPath = solutionPath;
        }
        protected SolutionProperties(string solutionName, string contactsFilePath)
        {
            solutionName.CheckArgument(nameof(solutionName));
            contactsFilePath.CheckArgument(nameof(contactsFilePath));

            SolutionName = solutionName;
            ContractsFilePath = contactsFilePath;
        }

        public static SolutionProperties Create()
        {
            return new SolutionProperties(GetCurrentSolutionPath());
        }
        public static SolutionProperties Create(string solutionPath)
        {
            return new SolutionProperties(solutionPath);
        }
        public static SolutionProperties Create(string solutionName, string contactsFilePath)
        {
            return new SolutionProperties(solutionName, contactsFilePath);
        }

        private static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory.IndexOf($"{nameof(CSharpCodeGenerator)}", StringComparison.CurrentCultureIgnoreCase);

            return AppContext.BaseDirectory.Substring(0, endPos);
        }
        private static string GetSolutionName(string solutionPath)
        {
            var fileInfo = new DirectoryInfo(solutionPath).GetFiles().SingleOrDefault(f => f.Extension.Equals(".sln", StringComparison.CurrentCultureIgnoreCase));

            return fileInfo != null ? Path.GetFileNameWithoutExtension(fileInfo.Name) : string.Empty;
        }
    }
}
//MdEnd