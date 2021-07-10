//@BaseCode
//MdStart

using System;
using System.Collections.Generic;
using System.Reflection;

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

        protected override void CreateModelPropertyAttributes(Type type, PropertyInfo propertyInfo, List<string> codeLines)
        {
            base.CreateModelPropertyAttributes(type, propertyInfo, codeLines);
            var handled = false;

            BeforeCreateModelPropertyAttributes(type, propertyInfo, codeLines, ref handled);
            if (handled == false)
            {
                if (propertyInfo.PropertyType.IsInterface)
                {
                    codeLines.Add("[System.Text.Json.Serialization.JsonIgnore]");
                }
            }
            AfterCreateModelPropertyAttributes(type, propertyInfo, codeLines);
        }
        partial void BeforeCreateModelPropertyAttributes(Type type, PropertyInfo propertyInfo, List<string> codeLines, ref bool handled);
        partial void AfterCreateModelPropertyAttributes(Type type, PropertyInfo propertyInfo, List<string> codeLines);

    }
}
//MdEnd