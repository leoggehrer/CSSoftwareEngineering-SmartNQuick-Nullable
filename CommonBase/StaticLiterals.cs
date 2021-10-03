//@BaseCode

using System.Collections.Generic;

namespace CommonBase
{
    public static partial class StaticLiterals
    {
        public static string SolutionFileExtension => ".sln";
        public static string ProjectFileExtension => ".csproj";

        public static string[] CommonProjects { get; } = new string[]
        {
            "CommonBase",
            "SolutionPreprocessorHelper.ConApp"
        };
        public static string[] GeneratorProjects { get; } = new string[]
        {
            "CSharpCodeGenerator.Logic",
            "CSharpCodeGenerator.ConApp",
        };
        public static string[] ToolProjects { get; } = new[]
        {
            "SolutionCodeComparsion.ConApp",
            "SolutionCopier.ConApp",
            "SolutionDeveloperHelper.ConApp",
            "SolutionGeneratedCodeDeleter.ConApp",
        };
        public static string[] ProjectExtensions { get; } = new string[]
            {
            ".Contracts",
            ".Logic",
            ".Transfer",
            ".WebApi",
            ".Adapters",
            ".AspMvc",
            ".ConApp"
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
