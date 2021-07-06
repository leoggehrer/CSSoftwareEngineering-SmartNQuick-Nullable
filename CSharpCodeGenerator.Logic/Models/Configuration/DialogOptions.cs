//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Models.Configuration
{
    internal record DialogOptions
    {
#pragma warning disable CA1822 // Mark members as static
        public string Type => nameof(DialogOptions);
#pragma warning restore CA1822 // Mark members as static
        public bool ShowTitle { get; set; }
        public bool ShowClose { get; set; }
        public string Left { get; set; }
        public string Top { get; set; }
        public string Bottom { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }
}
//MdEnd