//@BaseCode
//MdStart
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SolutionDockerBuilder.ConApp
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
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        private static string HomePath { get; set; }
        private static string UserPath { get; set; }
        private static string SourcePath { get; set; }

        private static void Main(/*string[] args*/)
        {
            bool running = false;

            do
            {
                var input = string.Empty;
                var dockerfiles = PrintHeader();

                Console.Write($"Build [1..{dockerfiles.Count() + 1}|X]?: ");
                input = Console.ReadLine().ToLower();

                running = input.Equals("x") == false;
                if (running)
                {
                    var numbers = input.Split(',').Where(s => Int32.TryParse(s, out int n))
                                       .Select(s => Int32.Parse(s))
                                       .ToArray();

                    foreach (var number in numbers)
                    {
                        if (number == dockerfiles.Count() + 1)
                        {
                            var solutionPath = GetCurrentSolutionPath();

                            BuildDockerfiles(solutionPath);
                        }
                        else if (number > 0 && number <= dockerfiles.Count())
                        {
                            BuildDockerfile(dockerfiles.ElementAt(number - 1), true);
                        }
                    }
                }
                if (ErrorHandler.HasError)
                {
                    Console.Clear();
                    Console.WriteLine("*** ERROR(S) ***");
                    ErrorHandler.GetErrors().ToList().ForEach(e => Console.WriteLine(e));
                    ErrorHandler.Clear();
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            } while (running);
        }

        private static IEnumerable<string> PrintHeader()
        {
            int index = 0;
            var result = new List<string>();
            var solutionPath = GetCurrentSolutionPath();

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"{nameof(SolutionDockerBuilder)}:");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            foreach (var dockerfile in GetDockerfiles(solutionPath))
            {
                var dockerfileInfo = new FileInfo(dockerfile);
                var directoryName = dockerfileInfo.Directory.Name;

                result.Add(dockerfileInfo.FullName);
                Console.WriteLine($"Build docker image for: [{++index,2}] {directoryName}");
            }
            Console.WriteLine($"Build docker image for: [{++index,2}] ALL");
            Console.WriteLine();
            return result;
        }
        private static void BuildDockerfile(string dockerfile, bool buildContracts)
        {
            var maxWaiting = 10 * 60 * 1000;    // 10 minutes
            var arguments = string.Empty;       // arguments for process start
            var slnPath = Directory.GetParent(Path.GetDirectoryName(dockerfile)).FullName;
            var contractsCsproj = GetContractProjectFileFromDockerfile(dockerfile);
            var codeGeneratorCsproj = GetCSharpCodeGeneratorProjectFileFromDockerfile(dockerfile);
            var contractsCsprojLines = default(string[]);

            if (string.IsNullOrEmpty(contractsCsproj) == false)
            {
                try
                {
                    contractsCsprojLines = File.ReadAllLines(contractsCsproj, Encoding.Default);
                    File.WriteAllLines(contractsCsproj, contractsCsprojLines.Select(l => l.Replace("Condition=\"True\"", "Condition=\"False\"")), Encoding.Default);

                    if (buildContracts)
                    {
                        arguments = $"build \"{contractsCsproj}\" -c Release";
                        Console.WriteLine(arguments);
                        Debug.WriteLine($"dotnet.exe {arguments}");
                        var csprojStartInfo = new ProcessStartInfo("dotnet.exe")
                        {
                            Arguments = arguments,
                            //WorkingDirectory = projectPath,
                            UseShellExecute = false
                        };
                        Process.Start(csprojStartInfo).WaitForExit(maxWaiting);

                        if (string.IsNullOrEmpty(codeGeneratorCsproj) == false)
                        {
                            arguments = $"run --project \"{codeGeneratorCsproj}\" -c Release";
                            Console.WriteLine(arguments);
                            Debug.WriteLine($"dotnet.exe {arguments}");
                            csprojStartInfo = new ProcessStartInfo("dotnet.exe")
                            {
                                Arguments = arguments,
                                //WorkingDirectory = projectPath,
                                UseShellExecute = false
                            };
                            Process.Start(csprojStartInfo).WaitForExit(maxWaiting);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorHandler.LastError = e.GetFullError();
                    Debug.WriteLine($"Error: {ErrorHandler.LastError}");
                }
            }
            var dockerfileInfo = new FileInfo(dockerfile);
            var directoryName = dockerfileInfo.Directory.Name;
            var directoryFullName = dockerfileInfo.Directory.FullName;
            var tagLabel = $"{directoryName.Replace(".", string.Empty).ToLower()}";

            try
            {
                arguments = $"build -f \"{dockerfile}\" --force-rm -t {tagLabel} --label \"com.microsoft.created-by=visual-studio\" --label \"com.microsoft.visual-studio.project-name={directoryName}\" \"{slnPath}\"";
                Console.WriteLine(arguments);
                Debug.WriteLine($"Docker {arguments}");
                var buildStartInfo = new ProcessStartInfo("docker")
                {
                    Arguments = arguments,
                    WorkingDirectory = directoryFullName,
                    UseShellExecute = false
                };
                Process.Start(buildStartInfo).WaitForExit(maxWaiting);

                //arguments = $"scan {tagLabel}";
                //Console.WriteLine(arguments);
                //Debug.WriteLine($"Docker {arguments}");
                //buildStartInfo = new ProcessStartInfo("docker")
                //{
                //    Arguments = arguments,
                //    WorkingDirectory = directoryFullName,
                //    UseShellExecute = false
                //};
                //Process.Start(buildStartInfo).WaitForExit(maxWaiting);
            }
            catch (Exception e)
            {
                ErrorHandler.LastError = e.GetFullError();
                Debug.WriteLine($"Error: {ErrorHandler.LastError}");
            }
            if (contractsCsprojLines != null)
            {
                File.WriteAllLines(contractsCsproj, contractsCsprojLines, Encoding.Default);
            }
        }
        private static void BuildDockerfiles(string solutionPath)
        {
            int counter = 0;

            foreach (var dockerfile in GetDockerfiles(solutionPath))
            {
                BuildDockerfile(dockerfile, counter++ == 0);
            }
        }

        private static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf($"{nameof(SolutionDockerBuilder)}", StringComparison.CurrentCultureIgnoreCase);

            return AppContext.BaseDirectory.Substring(0, endPos);
        }
        private static string GetContractProjectFileFromSolutionPath(string path)
        {
            return Directory.GetFiles(path, "*.Contracts.*proj", SearchOption.AllDirectories).FirstOrDefault();
        }
        private static string GetContractProjectFileFromDockerfile(string dockerfile)
        {
            var path = Path.GetDirectoryName(dockerfile);
            var dirInfo = Directory.GetParent(path);
            var fileInfo = dirInfo.GetFiles("*.Contracts.*proj", SearchOption.AllDirectories).FirstOrDefault();

            return fileInfo?.FullName;
        }
        private static string GetCSharpCodeGeneratorProjectFileFromDockerfile(string dockerfile)
        {
            var path = Path.GetDirectoryName(dockerfile);
            var dirInfo = Directory.GetParent(path);
            var fileInfo = dirInfo.GetFiles("CSharpCodeGenerator.ConApp.csproj", SearchOption.AllDirectories).FirstOrDefault();

            return fileInfo?.FullName;
        }
        private static IEnumerable<string> GetDockerfiles(string path)
        {
            var result = new List<string>();

            foreach (var item in Directory.GetFiles(path, "Dockerfile", SearchOption.AllDirectories))
            {
                result.Add(item);
            }
            return result;
        }
    }
}
//MdEnd