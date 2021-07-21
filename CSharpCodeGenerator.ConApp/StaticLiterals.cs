//@BaseCode
using System.Collections.Generic;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace CSharpCodeGenerator.ConApp
{
	public static class StaticLiterals
	{
        public static string SolutionFileExtension => ".sln";
        public static string ProjectFileExtension => ".csproj";
        public static string SourceFileExtensions => CommonStaticLiterals.SourceFileExtensions;
        public static string CSharpFileExtension => CommonStaticLiterals.CSharpFileExtension;
        public static string GeneratedCodeLabel => CommonStaticLiterals.GeneratedCodeLabel;
        public static string CustomizedAndGeneratedCodeLabel => CommonStaticLiterals.CustomizedAndGeneratedCodeLabel;
        public static IDictionary<string, string> SourceFileHeaders => CommonStaticLiterals.SourceFileHeaders;

        public static string ContractsExtension => ".Contracts";
    }
}
