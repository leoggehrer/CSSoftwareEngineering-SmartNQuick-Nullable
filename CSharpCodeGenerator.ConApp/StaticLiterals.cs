//@BaseCode
//MdStart
using System.Collections.Generic;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace CSharpCodeGenerator.ConApp
{
	public static partial class StaticLiterals
	{
        public static string GeneratedCodeFileName => "_GeneratedCode.cs";
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
//MdEnd