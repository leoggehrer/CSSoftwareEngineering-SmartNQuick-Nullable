//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Models.Configuration
{
    internal record EditFormSetting
    {
#pragma warning disable CA1822 // Mark members as static
        public string Type => nameof(EditFormSetting);
#pragma warning restore CA1822 // Mark members as static
        public bool HasHeader { get; set; } = false;
        public bool HasCancel { get; set; } = true;
        public bool HasSubmit { get; set; } = true;
        public bool HasSubmitClose { get; set; } = true;
        public bool HasFooter { get; set; } = true;
    }
}
//MdEnd