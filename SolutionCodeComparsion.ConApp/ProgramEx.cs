using System.IO;
using System.Linq;

namespace SolutionCodeComparsion.ConApp
{
    partial class Program
    {
        static partial void ClassConstructed()
        {
            var parentDirectory = @"C:\Users\g.gehrer\source\repos\ISO";
            var qtDirectories = Directory.GetDirectories(parentDirectory, "SnQ*", SearchOption.AllDirectories)
                                         .Where(d => d.Replace(UserPath, string.Empty).Contains('.') == false)
                                         .ToList();
            TargetPaths = qtDirectories.ToArray();
        }
    }
}
