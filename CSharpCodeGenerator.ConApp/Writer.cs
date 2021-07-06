//@QnSBaseCode
//MdStart

using CommonBase.Extensions;
using CSharpCodeGenerator.Logic;
using CSharpCodeGenerator.Logic.Common;
using CSharpCodeGenerator.Logic.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCodeGenerator.ConApp
{
    class Writer
    {
        public static void WriteAll(string solutionPath, ISolutionProperties solutionProperties, IEnumerable<IGeneratedItem> generatedItems)
        {
            solutionPath.CheckNotNullOrEmpty(nameof(solutionPath));
            solutionProperties.CheckArgument(nameof(solutionProperties));
            generatedItems.CheckArgument(nameof(generatedItems));

            var tasks = new List<Task>();

            #region WriteLogicCode
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.BusinessEntity);

                Console.WriteLine("Write Logic-Business-Entities...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.EntitiesBusinessFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.ModuleEntity);

                Console.WriteLine("Write Logic-Modules-Entities...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.EntitiesModulesFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.PersistenceEntity);

                Console.WriteLine("Write Logic-Persistence-Entities...");
                //WriteGeneratedCodeFile(solutionPath, solutionProperties.EntitiesPersistenceFileSubPath, writeItems);
                WriteCodeFiles(writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.ShadowEntity);

                Console.WriteLine("Write Logic-Shadow-Entities...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.EntitiesShadowFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItem = generatedItems.Single(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.DbContext);

                Console.WriteLine("Write Logic-DataContext-DbContext...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.DataContextPersistenceFileSubPath, writeItem);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.BusinessController);

                Console.WriteLine("Write logic-Business-Controllers...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.ControllersBusinessFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.PersistenceController);

                Console.WriteLine("Write Logic-Persistence-Controllers...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.ControllersPersistenceFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.ShadowController);

                Console.WriteLine("Write Logic-Shadow-Controllers...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.ControllersShadowFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItem = generatedItems.Single(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.Factory);

                Console.WriteLine("Write Logic-Factory...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.LogicFactoryFileSubPath, writeItem);
            }));
            #endregion WriteLogicCode

            #region WriteTransfer
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Transfer && e.ItemType == ItemType.BusinessModel);

                Console.WriteLine("Write Transfer-Business-Models...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.TransferBusinessFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Transfer && e.ItemType == ItemType.ModuleModel);

                Console.WriteLine("Write Transfer-Modules-Models...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.TransferModulesFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Transfer && e.ItemType == ItemType.PersistenceModel);

                Console.WriteLine("Write Transfer-Persistence-Models...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.TransferPersistenceFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Transfer && e.ItemType == ItemType.ShadowModel);

                Console.WriteLine("Write Transfer-Modules-Models...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.TransferShadowFileSubPath, writeItems);
            }));
            #endregion WriteTansfer

            #region WriteWebApi
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.WebApi && e.ItemType == ItemType.WebApiController);

                Console.WriteLine("Write WebApi-Controllers...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.WebApiControllersFileSubPath, writeItems);
            }));
            #endregion WriteWebApi

            #region AspMvc
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && e.ItemType == ItemType.BusinessModel);

                Console.WriteLine("Write AspMvc-Business-Models...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.AspMvcBusinessFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && e.ItemType == ItemType.ModuleModel);

                Console.WriteLine("Write AspMvc-Modules-Models...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.AspMvcModulesFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && e.ItemType == ItemType.PersistenceModel);

                Console.WriteLine("Write AspMvc-Persistence-Models...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.AspMvcPersistenceFileSubPath, writeItems);
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && e.ItemType == ItemType.ShadowModel);

                Console.WriteLine("Write AspMvc-Shadow-Models...");
                WriteGeneratedCodeFile(solutionPath, solutionProperties.AspMvcShadowFileSubPath, writeItems);
            }));
            #endregion AspMvc

            Task.WaitAll(tasks.ToArray());
        }

        #region Create translations
        private record Translation(string AppName, string KeyLanguage, string Key, string ValueLanguage, string Value);
        private static Translation ToTranslation(string line, string separator)
        {
            var data = line.Split(separator);
            return new Translation(data[0], data[1], data[2], data[3], data[4]);
        }
        private static string ToCsv(Translation translation, string separator)
        {
            return $"{translation.AppName}{separator}{translation.KeyLanguage}{separator}{translation.Key}{separator}{translation.ValueLanguage}{separator}{translation.Value}";
        }
        public static void WriteTranslations(string solutionPath, IGeneratedItem generatedItem)
        {
            generatedItem.CheckArgument(nameof(generatedItem));

            var separator = ";";
            var fileName = $"{generatedItem.FullName}{generatedItem.FileExtension}";
            var filePath = Path.Combine(solutionPath, fileName);
            var resultItems = new List<Translation>();

            if (File.Exists(filePath))
            {
                resultItems.AddRange(File.ReadAllLines(filePath).Select(l => ToTranslation(l, separator)));
            }
            foreach (var line in generatedItem.SourceCode)
            {
                var entry = ToTranslation(line, separator);
                var existEntry = resultItems.FirstOrDefault(e => e.AppName.Equals(entry.AppName)
                                                              && e.KeyLanguage.Equals(entry.KeyLanguage)
                                                              && e.Key.Equals(entry.Key));

                if (existEntry == null)
                {
                    resultItems.Add(entry);
                }
            }
            File.WriteAllLines(filePath, resultItems.Select(t => ToCsv(t, separator)), Encoding.Default);
        }
        #endregion Create translations

        #region Create properties
        private record Property(string AppName, string EntityName, string PropertyName, string Attribute, string Value);
        private static string ToCsv(Property property, string separator)
        {
            return $"{property.AppName}{separator}{property.EntityName}{separator}{property.PropertyName}{separator}{property.Attribute}{separator}{property.Value}";
        }
        static Property ToProperty(string line, string separator)
        {
            var data = line.Split(separator);
            return new Property(data[0], data[1], data[2], data[3], data[4]);
        }
        public static void WriteProperties(string solutionPath, IGeneratedItem generatedItem)
        {
            generatedItem.CheckArgument(nameof(generatedItem));

            var separator = ";";
            var fileName = $"{generatedItem.FullName}{generatedItem.FileExtension}";
            var filePath = Path.Combine(solutionPath, fileName);
            var resultItems = new List<Property>();

            if (File.Exists(filePath))
            {
                resultItems.AddRange(File.ReadAllLines(filePath).Select(l => ToProperty(l, separator)));
            }

            foreach (var line in generatedItem.SourceCode)
            {
                var entry = ToProperty(line, separator);
                var existEntry = resultItems.FirstOrDefault(e => e.AppName.Equals(entry.AppName)
                                                              && e.EntityName.Equals(entry.EntityName)
                                                              && e.PropertyName.Equals(entry.PropertyName)
                                                              && e.Attribute.Equals(entry.Attribute));

                if (existEntry == null)
                {
                    resultItems.Add(entry);
                }
            }
            File.WriteAllLines(filePath, resultItems.Select(p => ToCsv(p, separator)), Encoding.Default);
        }
        #endregion Create properties
        public static void WriteGeneratedItem(string solutionPath, IGeneratedItem generatedItem)
        {
            generatedItem.CheckArgument(nameof(generatedItem));

            var sourceLines = new List<string>(generatedItem.SourceCode);
            var filePath = Path.Combine(solutionPath, generatedItem.SubFilePath);

            if (sourceLines.Any())
            {
                var extension = Path.GetExtension(generatedItem.SubFilePath).ToLower();

                if (StaticLiterals.SourceFileHeaders.ContainsKey(extension))
                {
                    sourceLines.Insert(0, StaticLiterals.SourceFileHeaders[extension]);
                }
            }
            WriteCodeFile(filePath, sourceLines);
        }
        public static void WriteGeneratedItem(string solutionPath, IEnumerable<IGeneratedItem> generatedItems)
        {
            generatedItems.CheckArgument(nameof(generatedItems));

            foreach (var item in generatedItems)
            {
                WriteGeneratedItem(solutionPath, item);
            }
        }
        public static void WriteGeneratedCodeFile(string solutionPath, string filePath, IGeneratedItem generatedItem)
        {
            WriteGeneratedCodeFile(solutionPath, filePath, new[] { generatedItem });
        }
        public static void WriteGeneratedCodeFile(string solutionPath, string filePath, IEnumerable<IGeneratedItem> generatedItems)
        {
            var lines = new List<string>();
            var fullFilePath = Path.Combine(solutionPath, filePath);
            var directory = Path.GetDirectoryName(fullFilePath);

            foreach (var item in generatedItems)
            {
                lines.AddRange(item.SourceCode);
            }

            if (lines.Any() == false)
            {
                if (File.Exists(fullFilePath))
                {
                    File.Delete(fullFilePath);
                }
            }
            else if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            if (lines.Any())
            {
                var sourceLines = new List<string>(lines);

                sourceLines.Insert(0, $"//{StaticLiterals.GeneratedCodeLabel}");
                File.WriteAllLines(fullFilePath, sourceLines);
            }
        }
        public static void WriteCodeFiles(IEnumerable<IGeneratedItem> generatedItems)
        {
            generatedItems.CheckArgument(nameof(generatedItems));

            foreach (var item in generatedItems)
            {
                WriteCodeFile($"{item.FullName}{item.FileExtension}", item.SourceCode);
            }
        }

        public static void WriteCodeFile(string filePath, IEnumerable<string> source)
        {
            source.CheckArgument(nameof(source));

            var canCreate = true;
            var path = Path.GetDirectoryName(filePath);
            var generatedCode = StaticLiterals.GeneratedCodeLabel;

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                var header = lines.FirstOrDefault(l => l.Contains(StaticLiterals.GeneratedCodeLabel)
                                  || l.Contains(StaticLiterals.CustomizedAndGeneratedCodeLabel));

                if (header != null)
                {
                    File.Delete(filePath);
                }
                else
                {
                    canCreate = false;
                }
            }
            else if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            if (canCreate && source.Any())
            {
                File.WriteAllLines(filePath, source);
            }
        }
    }
}
//MdEnd