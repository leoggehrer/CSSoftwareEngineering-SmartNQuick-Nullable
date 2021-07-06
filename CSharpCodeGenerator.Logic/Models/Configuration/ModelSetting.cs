//@BaseCode
//MdStart

namespace CSharpCodeGenerator.Logic.Models.Configuration
{
    internal record ModelSetting
	{
#pragma warning disable CA1822 // Mark members as static
		public string Type => nameof(ModelSetting);
#pragma warning restore CA1822 // Mark members as static
		public bool Editable { get; set; } = true;
		public bool AllowAdd { get; set; } = true;
		public bool AllowEdit { get; set; } = true;
		public bool AllowDelete { get; set; } = true;
		public bool AllowInlineEdit { get; set; } = true;
	}
}
//MdEnd