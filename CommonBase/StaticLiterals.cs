//@BaseCode

namespace CommonBase
{
	public static partial class StaticLiterals
	{
		public static string SolutionFileExtension => ".sln";
		public static string ProjectFileExtension => ".csproj";

		public static string[] CommonProjects => new string[]
		{
			"CommonBase",
		};
		public static string[] GenerationProjects => new string[]
		{
			"CSharpCodeGenerator.ConApp",
		};
		public static string[] ProjectExtensions => new string[]
		{
				".Contracts",
				".Logic",
				".Transfer",
				".WebApi",
				".AspMvc",
				".ConApp"
		};
		public static string GeneratedCodeLabel => "@GeneratedCode";
		public static string IgnoreLabel => "@Ignore";
		public static string BaseCodeLabel => "@BaseCode";
		public static string CodeCopyLabel => "@CodeCopy";
	}
}
