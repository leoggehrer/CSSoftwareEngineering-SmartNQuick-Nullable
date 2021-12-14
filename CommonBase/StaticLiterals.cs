//@BaseCode
//MdStart
using System.Collections.Generic;

namespace CommonBase
{
	public static partial class StaticLiterals
	{
		public static string SolutionFileExtension => ".sln";
		public static string ProjectFileExtension => ".csproj";

		public static string[] ProjectExtensions { get; } = new string[]
		{
			".Contracts",
			".Logic",
			".Transfer",
			".WebApi",
			".Adapters",
			".AspMvc",
			".Logic.UnitTest",
			".ConApp"
		};
		public static string[] GeneratorProjects { get; } = new string[]
		{
			"CSharpCodeGenerator.Logic",
			"CSharpCodeGenerator.ConApp",
		};
		public static string[] SolutionProjects { get; } = new string[]
		{
			"CommonBase",
			"SolutionDockerBuilder.ConApp",
			"SolutionPreprocessorHelper.ConApp"
		};
		public static string[] SolutionToolProjects { get; } = new[]
		{
			"SolutionCodeComparsion.ConApp",
			"SolutionCopier.ConApp",
			"SolutionGeneratedCodeDeleter.ConApp",
		};
		public static IDictionary<string, string> SourceFileHeaders { get; } = new Dictionary<string, string>()
		{
			{".css", $"/*{GeneratedCodeLabel}*/" },
			{".cs", $"//{GeneratedCodeLabel}" },
			{".ts", $"//{GeneratedCodeLabel}" },
			{".cshtml", $"@*{GeneratedCodeLabel}*@" },
			{".razor", $"@*{GeneratedCodeLabel}*@" },
			{".razor.cs", $"//{GeneratedCodeLabel}" },
		};
		public static string GeneratedCodeLabel => "@GeneratedCode";
		public static string CustomizedAndGeneratedCodeLabel => "@CustomAndGeneratedCode";
		public static string SourceFileExtensions => "*.css|*.cs|*.ts|*.cshtml|*.razor|*.razor.cs|*.template";
		public static string CSharpFileExtension => ".cs";
		public static string IgnoreLabel => "@Ignore";
		public static string BaseCodeLabel => "@BaseCode";
		public static string CodeCopyLabel => "@CodeCopy";
	}
}
//MdEnd