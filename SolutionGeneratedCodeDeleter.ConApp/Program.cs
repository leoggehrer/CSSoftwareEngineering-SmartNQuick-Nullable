using System;
using System.Threading;

namespace SolutionGeneratedCodeDeleter.ConApp
{
    internal class Program
    {
        private static void Main(/*string[] args*/)
        {
            var solutionPath = GetCurrentSolutionPath();

            Console.WriteLine(nameof(SolutionGeneratedCodeDeleter));
            RunProgress();

            CSharpCodeGenerator.Logic.Generator.DeleteGenerationFiles(solutionPath);
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
