//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Contracts
{
    public interface IFactoryGenerator
    {
        ISolutionProperties Properties { get; }

        IGeneratedItem CreateLogicFactory();
        IGeneratedItem CreateAdapterFactory();
        IGeneratedItem CreateThirdPartyFactory();
    }
}
//MdEnd