//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Generation
{
    internal partial class AspMvcAppGenerator : ModelGenerator
    {
        internal AspMvcAppGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }
        public new static AspMvcAppGenerator Create(SolutionProperties solutionProperties)
        {
            return new AspMvcAppGenerator(solutionProperties);
        }

        public override Common.UnitType UnitType => Common.UnitType.AspMvcApp;
        public override string AppPostfix => SolutionProperties.AspMvcPostfix;
        public override string AppModelsNameSpace => $"{SolutionProperties.AspMvcAppProjectName}.{StaticLiterals.ModelsFolder}";
        public override string ModelsFolder => StaticLiterals.ModelsFolder;
    }
}
//MdEnd