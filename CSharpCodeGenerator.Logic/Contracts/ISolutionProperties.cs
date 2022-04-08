//@BaseCode
//MdStart
using System.Collections.Generic;

namespace CSharpCodeGenerator.Logic.Contracts
{
    public interface ISolutionProperties
    {
        string SolutionName { get; }
        IEnumerable<string> ProjectNames { get; }

        string AdaptersFactorySubPath { get; }
        string AdaptersSubPath { get; }
        string AdaptersProjectName { get; }

        string AspMvcAppBusinessSubPath { get; }
        string AspMvcAppModulesSubPath { get; }
        string AspMvcAppSubPath { get; }
        string AspMvcAppPersistenceSubPath { get; }
        string AspMvcAppProjectName { get; }
        string AspMvcAppShadowSubPath { get; }

        string BlazorServerAppBusinessSubPath { get; }
        string BlazorServerAppModulesSubPath { get; }
        string BlazorServerAppSubPath { get; }
        string BlazorServerAppPersistenceSubPath { get; }
        string BlazorServerAppProjectName { get; }
        string BlazorServerAppShadowSubPath { get; }

        string ConnectorSubPath { get; }
        string ConnectorProjectName { get; }
        string ContractsSubPath { get; }
        string ContractsProjectName { get; }
        string ControllersBusinessSubPath { get; }
        string ControllersPersistenceSubPath { get; }
        string ControllersSubPath { get; }
        string ControllersShadowSubPath { get; }
        string DataContextPersistenceSubPath { get; }
        string DataContextSubPath { get; }
        string DataContextDbFolder { get; }
        string EntitiesBusinessSubPath { get; }
        string EntitiesModulesSubPath { get; }
        string EntitiesPersitenceSubPath { get; }
        string EntitiesShadowSubPath { get; }
        string EntitiesSubPath { get; }
        string LogicFactorySubPath { get; }
        string LogicSubPath { get; }
        string LogicProjectName { get; }

        string TransferBusinessSubPath { get; }
        string TransferModulesSubPath { get; }
        string TransferSubPath { get; }
        string TransferPersistenceSubPath { get; }
        string TransferProjectName { get; }
        string TransferShadowSubPath { get; }

        string WebApiControllersSubPath { get; }
        string WebApiSubPath { get; }
        string WebApiProjectName { get; }
    }
}
//MdEnd