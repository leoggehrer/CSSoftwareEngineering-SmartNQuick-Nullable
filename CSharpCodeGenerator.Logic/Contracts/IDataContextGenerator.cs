//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Contracts
{
    public interface IDataContextGenerator
    {
        ISolutionProperties Properties { get; }

        IGeneratedItem CreateDbContext();
    }
}
//MdEnd