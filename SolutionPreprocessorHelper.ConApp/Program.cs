//@BaseCode
//MdStart
using CommonBase.Extensions;
using CSharpCodeGenerator.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SolutionPreprocessorHelper.ConApp
{
    internal class Program
    {
        private static string[] Directives { get; } = new string[] 
        { 
            "ACCOUNT_OFF", 
            "LOGGING_OFF", 
            "REVISION_OFF" 
        };

        private static void Main(/*string[] args*/)
        {
            var fileCount = 0;
            var stopwatch = new Stopwatch();
            Console.WriteLine(nameof(SolutionPreprocessorHelper));

            stopwatch.Start();
            PrintSolutionDirectives("DEBUG");
            SetPreprocessorDirectivesInProjectFiles(Directives);
            stopwatch.Stop();
            Console.WriteLine($"Set directives in {fileCount} file(s) in {stopwatch.ElapsedMilliseconds / 1000:f}");
        }

        private static void PrintSolutionDirectives(params string[] excludeDirectives)
        {
            excludeDirectives.CheckArgument(nameof(excludeDirectives));

            var path = SolutionAccessor.GetCurrentSolutionPath(nameof(SolutionPreprocessorHelper));
            var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var idx = 0;
                var lines = File.ReadAllLines(file, Encoding.Default);

                foreach (var line in lines)
                {
                    if (line.Trim().StartsWith("#if ") && excludeDirectives.Any(e => line.Contains(e)) == false)
                    {
                        var message = $"{line} in line {idx} of the {file} file";

                        Console.WriteLine(message);
                        Debug.WriteLine(message);
                    }
                    idx++;
                }
            }
        }

        private static int SetPreprocessorDirectivesInProjectFiles(params string[] directivItems)
        {
            directivItems.CheckArgument(nameof(directivItems));

            var path = SolutionAccessor.GetCurrentSolutionPath(nameof(SolutionPreprocessorHelper));
            var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
            var directives = string.Join(";", directivItems);

            foreach (var file in files)
            {
                var hasChanged = false;
                var result = new List<string>();
                var lines = File.ReadAllLines(file, Encoding.Default);

                foreach (var line in lines)
                {
                    if (line.Contains("<DefineConstants>", "</DefineConstants>"))
                    {
                        hasChanged = true;
                        result.Add(line.ReplaceBetween("<DefineConstants>", "</DefineConstants>", directives));
                    }
                    else
                    {
                        result.Add(line);
                    }
                }
                if (hasChanged == false && directives.Length > 0)
                {
                    var insertIdx = result.FindIndex(e => e.Contains("</PropertyGroup>"));

                    insertIdx = insertIdx < 0 ? result.Count - 2 : insertIdx;
                    hasChanged = true;

                    result.InsertRange(insertIdx + 1, new string[]
                        {
                            string.Empty,
                            "  <PropertyGroup>",
                            $"    <DefineConstants>{directives}</DefineConstants>",
                            "  </PropertyGroup>",
                        });
                }
                if (hasChanged)
                {
                    File.WriteAllLines(file, result.ToArray(), Encoding.Default);
                }
            }
            return files.Length;
        }
    }
}
//MdEnd