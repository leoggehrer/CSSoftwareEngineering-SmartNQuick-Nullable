//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Generation
{
    internal partial class BlazorServerAppGenerator : ModelGenerator
    {
        internal BlazorServerAppGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }
        public new static BlazorServerAppGenerator Create(SolutionProperties solutionProperties)
        {
            return new BlazorServerAppGenerator(solutionProperties);
        }

        public override Common.UnitType UnitType => Common.UnitType.BlazorServerApp;
        public override string AppPostfix => SolutionProperties.BlazorServerAppPostfix;
        public override string AppModelsNameSpace => $"{SolutionProperties.BlazorServerAppProjectName}.{StaticLiterals.ModelsFolder}";
        public override string ModelsFolder => StaticLiterals.ModelsFolder;
    }
}
//MdEnd