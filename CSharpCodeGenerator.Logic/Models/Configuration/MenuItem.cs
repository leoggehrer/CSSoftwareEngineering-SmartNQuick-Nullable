//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Models.Configuration
{
    internal record MenuItem
    {
#pragma warning disable CA1822 // Mark members as static
        public string Type => nameof(MenuItem);
#pragma warning restore CA1822 // Mark members as static
        public string Text { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
    }
}
//MdEnd