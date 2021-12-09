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

        string AspMvcBusinessSubPath { get; }
        string AspMvcModulesSubPath { get; }
        string AspMvcSubPath { get; }
        string AspMvcPersistenceSubPath { get; }
        string AspMvcProjectName { get; }
        string AspMvcShadowSubPath { get; }

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