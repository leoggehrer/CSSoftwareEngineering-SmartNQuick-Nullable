using System;
using System.Threading;

namespace SolutionGeneratedCodeDeleter.ConApp
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
            SolutionPath = GetCurrentSolutionPath();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        private static string HomePath { get; set; }
        private static string UserPath { get; set; }
        private static string SolutionPath { get; set; }

        private static void Main(/*string[] args*/)
        {
            Console.WriteLine(nameof(SolutionGeneratedCodeDeleter));
            RunProgress();

            CSharpCodeGenerator.Logic.Generator.DeleteGenerationFiles(SolutionPath);
        }

        private static void RunProgress()
        {
            var t = new Thread(() =>
            {
                bool hpos = true;
                int top = Console.CursorTop + 1;

                while (true)
                {
                    Console.SetCursorPosition(0, top);
                    if (hpos)
                    {
                        Console.Write("---");
                    }
                    else
                    {
                        Console.Write(" | ");
                    }
                    hpos = !hpos;
                    Thread.Sleep(500);
                }
            })
            {
                IsBackground = true
            };
            t.Start();
        }
        private static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf($"{nameof(SolutionGeneratedCodeDeleter)}", StringComparison.CurrentCultureIgnoreCase);

            return AppContext.BaseDirectory.Substring(0, endPos);
        }
    }
}
