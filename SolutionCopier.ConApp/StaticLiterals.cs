using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionCopier.ConApp
{
	static class StaticLiterals
	{
		public static string SolutionFileExtension => ".sln";
		public static string ProjectFileExtension => ".csproj";

		public static string[] SnQCommonProjects => new string[]
		{
			"CommonBase",
		};
		public static string[] SnQProjectExtensions => new string[]
		{
				".Contracts",
				".Logic",
				".Transfer",
				".WebApi",
				".AspMvc",
				".ConApp"
		};
		public static string SnQIgnoreLabel => "@Ignore";
		public static string SnQGeneratedCodeLabel => "@GeneratedCode";
		public static string SnQBaseCodeLabel => "@BaseCode";
		public static string SnQCodeCopyLabel => "@CodeCopy";
	}
}
