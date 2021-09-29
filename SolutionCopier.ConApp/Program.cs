//@BaseCode
using System;
using System.Linq;
using System.Threading;
using System.IO;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace SolutionCopier.ConApp
{
    internal class Program
	{
        private static void Main(/*string[] args*/)
		{
			Console.WriteLine("SolutionCopier");

			var sourceSolutionName = "SmartNQuick";
			var sourceProjects = CommonStaticLiterals.CommonProjects
                                               .Concat(CommonStaticLiterals.GeneratorProjects
                                                                     .Select(e => $"{e}"))
											   .Concat(CommonStaticLiterals.ProjectExtensions
																	 .Select(e => $"{sourceSolutionName}{e}"));
            
            var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var sourcePath = GetCurrentSolutionPath();
            var targetPath = Path.Combine(userPath, @"source\repos\HtlLeo\SnQProjectC");
            Console.WriteLine("Solution copier!");
            Console.WriteLine("================");
            Console.WriteLine();
            Console.WriteLine($"Copy from: {sourcePath}");
            Console.WriteLine($"Copy to:   {targetPath}");
            Console.WriteLine();

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
