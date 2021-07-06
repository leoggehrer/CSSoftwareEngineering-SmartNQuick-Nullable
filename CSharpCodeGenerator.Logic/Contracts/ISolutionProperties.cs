//@BaseCode
//MdStart
using System.Collections.Generic;

namespace CSharpCodeGenerator.Logic.Contracts
{
    public interface ISolutionProperties
    {
        string SolutionName { get; }
        IEnumerable<string> ProjectNames { get; }

        string AspMvcBusinessFileSubPath { get; }
        string AspMvcBusinessSubPath { get; }
        string AspMvcModulesFileSubPath { get; }
        string AspMvcModulesSubPath { get; }
        string AspMvcSubPath { get; }
        string AspMvcPersistenceFileSubPath { get; }
        string AspMvcPersistenceSubPath { get; }
        string AspMvcProjectName { get; }
        string AspMvcShadowFileSubPath { get; }
        string AspMvcShadowSubPath { get; }

        string ConnectorSubPath { get; }
        string ConnectorProjectName { get; }
        string ContractsSubPath { get; }
        string ContractsProjectName { get; }
        string ControllersBusinessFileSubPath { get; }
        string ControllersBusinessSubPath { get; }
        string ControllersPersistenceFileSubPath { get; }
        string ControllersPersistenceSubPath { get; }
        string ControllersSubPath { get; }
        string ControllersShadowFileSubPath { get; }
        string ControllersShadowSubPath { get; }
        string DataContextPersistenceFileSubPath { get; }
        string DataContextPersistenceSubPath { get; }
        string DataContextSubPath { get; }
        string DataContextDbFolder { get; }
        string EntitiesBusinessFileSubPath { get; }
        string EntitiesBusinessSubPath { get; }
        string EntitiesModulesFileSubPath { get; }
        string EntitiesModulesSubPath { get; }
        string EntitiesPersistenceFileSubPath { get; }
        string EntitiesPersitenceSubPath { get; }
        string EntitiesShadowFileSubPath { get; }
        string EntitiesShadowSubPath { get; }
        string EntitiesSubPath { get; }
        string LogicFactoryFileSubPath { get; }
        string LogicFactorySubPath { get; }
        string LogicSubPath { get; }
        string LogicProjectName { get; }

        string TransferBusinessFileSubPath { get; }
        string TransferBusinessSubPath { get; }
        string TransferModulesFileSubPath { get; }
        string TransferModulesSubPath { get; }
        string TransferSubPath { get; }
        string TransferPersistenceFileSubPath { get; }
        string TransferPersistenceSubPath { get; }
        string TransferProjectName { get; }
        string TransferShadowFileSubPath { get; }
        string TransferShadowSubPath { get; }

        string WebApiControllersSubPath { get; }
        string WebApiControllersFileSubPath { get; }
        string WebApiSubPath { get; }
        string WebApiProjectName { get; }
    }
}
//MdEnd