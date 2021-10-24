//MdStart
using System;
using System.IO;
using System.Linq;
using System.Threading;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace SolutionCopier.ConApp
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
			TargetPath = Path.Combine(UserPath, @"source\repos\HtlLeo\AustroSoftAG\SnQAustroSoftProject");
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();

		private static string HomePath { get; set; }
		private static string UserPath { get; set; }
		private static string SourcePath { get; set; }
		private static string TargetPath { get; set; }
		private static void Main(/*string[] args*/)
		{
			Console.WriteLine(nameof(SolutionCopier));

			var sourceSolutionName = "SmartNQuick";
			var sourceProjects = CommonStaticLiterals.SolutionProjects
											   .Concat(CommonStaticLiterals.GeneratorProjects
																	 .Select(e => $"{e}"))
											   .Concat(CommonStaticLiterals.ProjectExtensions
																	 .Select(e => $"{sourceSolutionName}{e}"));

			Console.WriteLine("Solution copier!");
			Console.WriteLine("================");
			Console.WriteLine();
			Console.WriteLine($"Copy from: {SourcePath}");
			Console.WriteLine($"Copy to:   {TargetPath}");
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

			sc.Copy(SourcePath, TargetPath, sourceProjects);
		}

		private static string GetCurrentSolutionPath()
		{
			int endPos = AppContext.BaseDirectory
								   .IndexOf($"{nameof(SolutionCopier)}", StringComparison.CurrentCultureIgnoreCase);

			return AppContext.BaseDirectory.Substring(0, endPos);
		}
	}
}
//MdEnd