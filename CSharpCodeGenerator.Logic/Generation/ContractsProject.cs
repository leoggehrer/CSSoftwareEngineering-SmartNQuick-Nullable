//@BaseCode
//MdStart
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace CSharpCodeGenerator.Logic.Generation
{
    internal partial class ContractsProject
    {
        public SolutionProperties SolutionProperties { get; private set; }
        public string ProjectName => $"{SolutionProperties.SolutionName}{SolutionProperties.ContractsPostfix}";
        public string ProjectPath => Path.Combine(SolutionProperties.SolutionPath, ProjectName);

        private ContractsProject()
        {

        }
        public static ContractsProject Create(SolutionProperties solutionProperties)
        {
            solutionProperties.CheckArgument(nameof(solutionProperties));

            ContractsProject result = new()
            {
                SolutionProperties = solutionProperties
            };
            return result;
        }

        private IEnumerable<Type> assemblyTypes = null;
        public IEnumerable<Type> AssemblyTypes
        {
            get
            {
                if (assemblyTypes == null)
                {
                    if (SolutionProperties.ContractsFilePath.HasContent())
                    {
                        assemblyTypes = AssemblyLoadContext.Default
                                                           .LoadFromAssemblyPath(SolutionProperties.ContractsFilePath)
                                                           .GetTypes();
                    }

                }
                return assemblyTypes ?? Array.Empty<Type>();
            }
        }

        public IEnumerable<Type> EnumTypes => AssemblyTypes.Where(t => t.IsEnum);
        public IEnumerable<Type> InterfaceTypes => AssemblyTypes.Where(t => t.IsInterface);
        public IEnumerable<Type> RootTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.EndsWith($"{StaticLiterals.RootSubName}{t.Name}"));
            }
        }
        public IEnumerable<Type> ClientTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(StaticLiterals.ClientSubName));
            }
        }
        public IEnumerable<Type> BusinessTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(StaticLiterals.BusinessSubName));
            }
        }
        public IEnumerable<Type> ModuleTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(StaticLiterals.ModulesSubName));
            }
        }

        public IEnumerable<Type> PersistenceTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(StaticLiterals.PersistenceSubName));
            }
        }
        public IEnumerable<Type> ShadowTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(StaticLiterals.ShadowSubName));
            }
        }
        public IEnumerable<Type> ThirdPartyTypes
        {
            get
            {
                return InterfaceTypes.Where(t => t.IsInterface
                                              && t.FullName.Contains(StaticLiterals.ThirdPartySubName));
            }
        }
    }
}
//MdEnd