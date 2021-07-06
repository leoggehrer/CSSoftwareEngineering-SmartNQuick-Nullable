//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Contracts
{
	public interface IConfigurationGenerator
    {
        ISolutionProperties Properties { get; }

        string Separator { get; set; }
        IGeneratedItem CreateTranslations();
        IGeneratedItem CreateProperties();
    }
}
//MdEnd