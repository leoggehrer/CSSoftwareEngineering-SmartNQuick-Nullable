//@BaseCode
//MdStart
using System;
using System.IO;
using System.Linq;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace CSharpCodeGenerator.Logic
{
    public static partial class SolutionAccessor
    {
        public static string GetCurrentSolutionPath(string solutionProjectName)
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf($"{solutionProjectName}", StringComparison.CurrentCultureIgnoreCase);

            return AppContext.BaseDirectory.Substring(0, endPos);
        }
        public static string GetCurrentSolutionName(string solutionProjectName)
        {
            var solutionPath = GetCurrentSolutionPath(solutionProjectName);

            return GetSolutionNameByFile(solutionPath);
        }
        public static string GetSolutionNameByFile(string solutionPath)
        {
            var fileInfo = new DirectoryInfo(solutionPath).GetFiles()
                                                          .SingleOrDefault(f => f.Extension.Equals(CommonStaticLiterals.SolutionFileExtension, StringComparison.CurrentCultureIgnoreCase));

            return fileInfo != null ? Path.GetFileNameWithoutExtension(fileInfo.Name) : string.Empty;
        }
        public static string GetContractsFilePath(string solutionPath)
        {
            var result = default(string);
            var solutionName = GetSolutionNameByFile(solutionPath);
            var projectName = $"{solutionName}{StaticLiterals.ContractsExtension}";
            var binPath = Path.Combine(solutionPath, projectName, "bin");

            if (Directory.Exists(binPath))
            {
                var fileName = $"{projectName}.dll";
                var fileInfos = new DirectoryInfo(binPath).GetFiles(fileName, SearchOption.AllDirectories)
                                                          .Where(f => f.FullName.EndsWith(fileName))
                                                          .OrderByDescending(f => f.LastWriteTime);

                var fileInfo = fileInfos.Where(f => f.FullName.ToLower().Contains("\\ref\\") == false)
                                        .FirstOrDefault();

                if (fileInfo != null)
                {
                    result = fileInfo.FullName;
                }
            }
            return result;
        }
    }
}
//MdEnd