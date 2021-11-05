//MdStart
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace SolutionCodeComparsion.ConApp
{
    internal partial class Program
    {
        static Program()
        {
            ClassConstructing();
            HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                        Environment.OSVersion.Platform == PlatformID.MacOSX)
                       ? Environment.GetEnvironmentVariable("HOME")
                       : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            SourcePath = GetCurrentSolutionPath();
            Paths = new Dictionary<string, string[]>();
			SourceLabels = new Dictionary<string, string[]>
			{
				{ SourcePath, new string[] { CommonStaticLiterals.BaseCodeLabel } }
			};

			// Project: SmartNQuick-Projects
			var targetPaths = new string[]
            {
                Path.Combine(UserPath, @"source\repos\HtlLeo\CSSoftwareEngineering\SnQTranslator"),
                Path.Combine(UserPath, @"source\repos\HtlLeo\CSSoftwareEngineering\SnQConfigurator"),
                Path.Combine(UserPath, @"source\repos\HtlLeo\CSSoftwareEngineering\SnQContact"),
                Path.Combine(UserPath, @"source\repos\HtlLeo\CSSoftwareEngineering\SnQTradingCompany"),
                Path.Combine(UserPath, @"source\repos\HtlLeo\CSSoftwareEngineering\SnQMusicStore"),
            };
            Paths.Add(SourcePath, targetPaths);
            // End: SmartNQuick-Projects
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        private static string HomePath { get; set; }
        private static string UserPath { get; set; }
        private static string SourcePath { get; set; }
        private static Dictionary<string, string[]> Paths { get; set; }
        private static Dictionary<string, string[]> SourceLabels { get; set; }
        private static string[] SearchPatterns => CommonStaticLiterals.SourceFileExtensions.Split('|');
        private static readonly string[] TargetLabels = new string[] { CommonStaticLiterals.CodeCopyLabel };

        private static void Main(/*string[] args*/)
        {
            string input;
            string queryMsg = "Copy [y|n]?: ";

            PrintHeader();
            Console.Write(queryMsg);
            input = Console.ReadLine();
            while (input.Equals("y", StringComparison.CurrentCultureIgnoreCase))
            {
                PrintHeader();
                PrintBusyProgress();

                foreach (var path in Paths)
                {
                    // Delete all QnSCopyCode files
                    foreach (var targetPath in path.Value)
                    {
                        foreach (var searchPattern in SearchPatterns)
                        {
                            var targetCodeFiles = GetSourceCodeFiles(targetPath, searchPattern, TargetLabels);
                            foreach (var targetCodeFile in targetCodeFiles)
                            {
                                File.Delete(targetCodeFile);
                            }
                        }
                    }
                    // Copy all QnSBasCode files
                    foreach (var searchPattern in SearchPatterns)
                    {
                        var sourceLabels = SourceLabels[path.Key];
                        var sourceCodeFiles = GetSourceCodeFiles(path.Key, searchPattern, sourceLabels);

                        foreach (var targetPath in path.Value)
                        {
                            foreach (var sourceCodeFile in sourceCodeFiles)
                            {
                                SynchronizeSourceCodeFile(path.Key, sourceCodeFile, targetPath, sourceLabels, TargetLabels);
                            }
                        }
                    }
                }
                runBusyProgress = false;
                PrintHeader();
                Console.Write(queryMsg);
                input = Console.ReadLine();
            }
        }

        private static bool runBusyProgress = false;
        private static void PrintBusyProgress()
        {
            Console.WriteLine();
            runBusyProgress = true;
            Task.Factory.StartNew(async () =>
            {
                while (runBusyProgress)
                {
                    Console.Write(".");
                    await Task.Delay(250).ConfigureAwait(false);
                }
            });
        }

        private static void PrintHeader()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"{nameof(SolutionCodeComparsion)}:");
            Console.WriteLine("==========================================");
            Console.WriteLine();
            foreach (var path in Paths)
            {
                Console.WriteLine($"Source: {path.Key}");
                Console.WriteLine();
                foreach (var target in path.Value)
                {
                    Console.WriteLine($"\t -> {target}");
                }
            }
            Console.WriteLine();
        }
        private static IEnumerable<string> GetSourceCodeFiles(string path, string searchPattern, string[] labels)
        {
            var result = new List<string>();
            var files = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).OrderBy(i => i);

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file, Encoding.Default);

                if (lines.Any() && labels.Any(l => lines.First().Contains(l)))
                {
                    result.Add(file);
                }
                System.Diagnostics.Debug.WriteLine($"{file}");
            }
            return result;
        }
        private static bool SynchronizeSourceCodeFile(string sourcePath, string sourceFilePath, string targetPath, string[] sourceLabels, string[] targetLabels)
        {
            var result = false;
            var canCopy = true;
            var sourceSolutionName = GetSolutionNameFromPath(sourcePath);
            var sourceProjectName = GetProjectNameFromFilePath(sourceFilePath, sourceSolutionName);
            var sourceSubFilePath = sourceFilePath.Replace(sourcePath, string.Empty)
                                                  .Replace(sourceProjectName, string.Empty)
                                                  .Replace("\\\\", string.Empty)
                                                  .Replace("//", string.Empty);
            var sourceSubFilePath2 = sourceFilePath.Replace(sourcePath, string.Empty)
                                                  .Replace(sourceProjectName, string.Empty)
                                                  .Replace("\\", "#")
                                                  .Replace("//", "#")
                                                  .Split('#', StringSplitOptions.RemoveEmptyEntries);

            var targetSolutionName = GetSolutionNameFromPath(targetPath);
            var targetProjectName = sourceProjectName.Replace(sourceSolutionName, targetSolutionName);
            var targetFilePath = Path.Combine(targetPath, targetProjectName, string.Join("\\", sourceSubFilePath2));
            var targetFileFolder = Path.GetDirectoryName(targetFilePath);

            var tFilePath = Path.Combine(targetPath, targetProjectName);
            var a1 = sourceFilePath.Replace(sourcePath, targetPath);
            var a2 = a1.Replace(sourceProjectName, targetProjectName);
            var a3 = a2.Replace(sourceSolutionName, targetSolutionName);

            if (Directory.Exists(targetFileFolder) == false)
            {
                Directory.CreateDirectory(targetFileFolder);
            }
            if (File.Exists(targetFilePath))
            {
                var lines = File.ReadAllLines(targetFilePath, Encoding.Default);

                canCopy = false;
                if (lines.Any() && targetLabels.Any(l => lines.First().Contains(l)))
                {
                    canCopy = true;
                }
            }
            if (canCopy)
            {
                var cpyLines = new List<string>();
                var srcLines = File.ReadAllLines(sourceFilePath, Encoding.Default)
                                   .Select(i => i.Replace(sourceSolutionName, targetSolutionName));
                var srcFirst = srcLines.FirstOrDefault();

                if (srcFirst != null)
                {
                    var label = sourceLabels.FirstOrDefault(l => srcFirst.Contains(l));

                    cpyLines.Add(srcFirst.Replace(label ?? string.Empty, CommonStaticLiterals.CodeCopyLabel));
                }
                cpyLines.AddRange(File.ReadAllLines(sourceFilePath, Encoding.Default)
                                   .Skip(1)
                                   .Select(i => i.Replace(sourceSolutionName, targetSolutionName)));
                File.WriteAllLines(targetFilePath, cpyLines.ToArray(), Encoding.UTF8);
            }
            return result;
        }
        private static string GetSolutionNameFromPath(string path)
        {
            var result = string.Empty;
            var data = path.Split("\\", StringSplitOptions.RemoveEmptyEntries);

            if (data.Any())
            {
                result = data.Last();
            }
            return result;
        }
        private static string GetProjectNameFromFilePath(string filePath, string solutionName)
        {
            var result = string.Empty;
            var data = filePath.Split("\\", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length && result == string.Empty; i++)
            {
                for (int j = 0; j < CommonStaticLiterals.SolutionProjects.Length; j++)
                {
                    if (data[i].Equals(CommonStaticLiterals.SolutionProjects[j]))
                    {
                        result = data[i];
                    }
                }
                for (int j = 0; j < CommonStaticLiterals.GeneratorProjects.Length; j++)
                {
                    if (data[i].Equals(CommonStaticLiterals.GeneratorProjects[j]))
                    {
                        result = data[i];
                    }
                }
                for (int j = 0; j < CommonStaticLiterals.SolutionToolProjects.Length; j++)
                {
                    if (data[i].Equals(CommonStaticLiterals.SolutionToolProjects[j]))
                    {
                        result = data[i];
                    }
                }
                if (string.IsNullOrEmpty(result))
                {
                    for (int j = 0; j < CommonStaticLiterals.ProjectExtensions.Length; j++)
                    {
                        if (data[i].Equals($"{solutionName}{CommonStaticLiterals.ProjectExtensions[j]}"))
                        {
                            result = data[i];
                        }
                    }
                }
            }
            return result;
        }
        private static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf($"{nameof(SolutionCodeComparsion)}", StringComparison.CurrentCultureIgnoreCase);

            return AppContext.BaseDirectory.Substring(0, endPos);
        }
    }
}
//MdEnd