//@BaseCode
//MdStart
using CSharpCodeGenerator.Logic.Common;
using System.Collections.Generic;

namespace CSharpCodeGenerator.Logic.Contracts
{
    public interface IModelGenerator
    {
        ISolutionProperties Properties { get; }


        UnitType UnitType { get; }
        string AppPostfix { get; }
        string ModelsFolder { get; }
        string AppModelsNameSpace { get; }

        IEnumerable<IGeneratedItem> GenerateAll();
        IEnumerable<IGeneratedItem> CreateBusinessModels();
        IEnumerable<IGeneratedItem> CreateModulesModels();
        IEnumerable<IGeneratedItem> CreatePersistenceModels();
        IEnumerable<IGeneratedItem> CreateShadowModels();
    }
}
//MdEnd