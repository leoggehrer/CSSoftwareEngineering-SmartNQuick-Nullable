//@BaseCode
//MdStart
using System.Collections.Generic;

namespace CSharpCodeGenerator.Logic.Contracts
{
    public interface IEntityGenerator
    {
        ISolutionProperties Properties { get; }

        IEnumerable<IGeneratedItem> CreateBusinessEntities();
        IEnumerable<IGeneratedItem> CreateModulesEntities();
        IEnumerable<IGeneratedItem> CreatePersistenceEntities();
        IEnumerable<IGeneratedItem> CreateShadowEntities();
    }
}
//MdEnd