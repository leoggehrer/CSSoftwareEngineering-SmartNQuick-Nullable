using System;
using System.Linq;
using System.Threading;

namespace SolutionCopier.ConApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("SolutionCopier");

			var sourceSolutionName = "SmartNQuick";
			var sourceProjects = StaticLiterals.SnQCommonProjects
											   .Concat(StaticLiterals.SnQProjectExtensions
																	 .Select(e => $"{sourceSolutionName}{e}"));
			var sourcePath = GetCurrentSolutionPath();
            var targetPath = @"C:\Users\ggehr\source\repos\SnQTemplate";

            Console.WriteLine("Solution copier!");
            Console.WriteLine("================");
            Console.WriteLine();
            Console.WriteLine($"Copy from: {sourcePath}");
            Console.WriteLine($"Copy to:   {targetPath}");
            Console.WriteLine();

            Thread t = new Thread(() =>
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

            var sc = new Copier();

            sc.Copy(sourcePath, targetPath, sourceProjects);
        }

        private static string GetCurrentSolutionPath()
		{
			int endPos = AppContext.BaseDirectory
								   .IndexOf($"{nameof(SolutionCopier)}", StringComparison.CurrentCultureIgnoreCase);

			return AppContext.BaseDirectory.Substring(0, endPos);
		}

	}
}
