//@BaseCode
//MdStart

using CSharpCodeGenerator.Logic.Helpers;
using System.Collections.Generic;

namespace CSharpCodeGenerator.Logic.Generation
{
    internal partial class TransferGenerator : ModelGenerator
    {
        protected TransferGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }
        public new static TransferGenerator Create(SolutionProperties solutionProperties)
        {
            return new TransferGenerator(solutionProperties);
        }

        public override Common.UnitType UnitType => Common.UnitType.Transfer;
        public override string AppPostfix => SolutionProperties.TransferPostfix;
        public override string AppModelsNameSpace => $"{SolutionProperties.TransferProjectName}.{StaticLiterals.ModelsFolder}";
        public override string ModelsFolder => StaticLiterals.ModelsFolder;

        protected override void CreateModelPropertyAttributes(ContractPropertyHelper propertyHelper, List<string> codeLines)
        {
            base.CreateModelPropertyAttributes(propertyHelper, codeLines);
            var handled = false;

            BeforeCreateModelPropertyAttributes(propertyHelper, codeLines, ref handled);
            if (handled == false)
            {
                if (propertyHelper.PropertyType.IsInterface)
                {
                    codeLines.Add("[System.Text.Json.Serialization.JsonIgnore]");
                }
            }
            AfterCreateModelPropertyAttributes(propertyHelper, codeLines);
        }
        partial void BeforeCreateModelPropertyAttributes(ContractPropertyHelper propertyHelper, List<string> codeLines, ref bool handled);
        partial void AfterCreateModelPropertyAttributes(ContractPropertyHelper propertyHelper, List<string> codeLines);
    }
}
//MdEnd