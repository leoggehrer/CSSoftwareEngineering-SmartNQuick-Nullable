//@BaseCode
//MdStart
using CommonBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpCodeGenerator.Logic.Generation
{
    internal partial class DataContextGenerator : GeneratorObject, Contracts.IDataContextGenerator
    {
        protected DataContextGenerator(SolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }

        public static DataContextGenerator Create(SolutionProperties solutionProperties)
        {
            return new DataContextGenerator(solutionProperties);
        }

        public string DataContextNameSpace => $"{SolutionProperties.LogicProjectName}.{SolutionProperties.DataContextFolder}";

        private bool CanCreateDoModelCreating()
        {
            bool create = true;

            CanCreateDoModelCreating(ref create);
            return create;
        }
        partial void CanCreateDoModelCreating(ref bool canCreating);
        private bool CanEntityModelBuild(Type type)
        {
            bool create = true;

            CanEntityModelBuild(type, ref create);
            return create;
        }
        partial void CanEntityModelBuild(Type type, ref bool canCreating);
        private bool CanEntityModelConfigure(Type type)
        {
            bool create = true;

            CanEntityModelConfigure(type, ref create);
            return create;
        }
        partial void CanEntityModelConfigure(Type type, ref bool canCreating);

        private string CreateDbNameSpace()
        {
            return $"{DataContextNameSpace}";
        }
        public Contracts.IGeneratedItem CreateDbContext()
        {
            return CreateDbContext(CreateDbNameSpace());
        }
        private Models.GeneratedItem CreateDbContext(string nameSpace)
        {
            var contractsProject = ContractsProject.Create(SolutionProperties);
            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.DbContext)
            {
                FullName = $"{nameSpace}.{SolutionProperties.SolutionName}DbContext",
                FileExtension = StaticLiterals.CSharpFileExtension,
            };
            result.SubFilePath = $"{result.FullName}PartA{result.FileExtension}";
            result.Add($"namespace {nameSpace}");
            result.Add("{");
            result.Add("using Microsoft.EntityFrameworkCore;");
            result.Add("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
            result.Add($"partial class {SolutionProperties.SolutionName}DbContext");
            result.Add("{");

            foreach (var type in contractsProject.PersistenceTypes)
            {
                string entityName = CreateEntityNameFromInterface(type);
                string subNameSpace = CreateSubNamespaceFromType(type);
                string entityNameSet = $"{entityName}Set";

                result.Add($"protected DbSet<Entities.{subNameSpace}.{entityName}> {entityNameSet}" + " { get; set; }");
            }

            result.AddRange(CreateSetMethode());
            result.AddRange(CreateDoModelCreating());

            result.Add("}");
            if (nameSpace.HasContent())
            {
                result.Add("}");
            }
            result.AddRange(result.Source.Eject().FormatCSharpCode());
            return result;
        }
        private IEnumerable<string> CreateSetMethode()
        {
            var first = true;
            var result = new List<string>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            #region Generate DbSet<E> Set<I, E>()
            result.Add("partial void GetDbSet<I, E>(ref DbSet<E> dbSet) where E : class");
            result.Add("{");

            foreach (var type in contractsProject.PersistenceTypes)
            {
                string entityName = CreateEntityNameFromInterface(type);
                string entityNameSet = $"{entityName}Set";

                if (first)
                {
                    result.Add($"if (typeof(I) == typeof({type.FullName}))");
                }
                else
                {
                    result.Add($"else if (typeof(I) == typeof({type.FullName}))");
                }
                result.Add("{");
                result.Add($"dbSet = {entityNameSet} as DbSet<E>;");
                result.Add("}");
                first = false;
            }
            result.Add("}");
            #endregion Generate DbSet<E> Set<I, E>()
            return result;
        }
        private IEnumerable<string> CreateDoModelCreating()
        {
            var result = new List<string>();
            var contractsProject = ContractsProject.Create(SolutionProperties);

            #region CanCreateDoModelCreating()
            if (CanCreateDoModelCreating())
            {
                result.Add("static partial void DoModelCreating(ModelBuilder modelBuilder)");
                result.Add("{");
                foreach (var type in contractsProject.PersistenceTypes.Where(t => CanEntityModelBuild(t)))
                {
                    var contractHelper = new Helpers.ContractHelper(type);
                    var builder = $"{contractHelper.EntityFieldName}Builder";

                    result.Add($"var {builder} = modelBuilder.Entity<{contractHelper.EntityType}>();");
                    if (contractHelper.ContextType == CommonBase.Attributes.ContextType.View)
                    {
                        result.Add($"{builder}.ToView(\"{contractHelper.ContextName}\", \"{contractHelper.SchemaName}\");");
                        result.AddRange(CreateEntityConfigure(type));
                    }
                    else if (contractHelper.ContextType == CommonBase.Attributes.ContextType.Table)
                    {
                        result.Add($"{builder}.ToTable(\"{contractHelper.ContextName}\", \"{contractHelper.SchemaName}\")");
                        if (Helpers.ContractHelper.HasPersistenceBaseInterface(type) == false)
                        {
                            if (contractHelper.IsIdentifiable)
                            {
                                result.Add($".HasKey(\"{contractHelper.KeyName}\");");
                            }
                            if (contractHelper.IsVersionable)
                            {
                                result.Add($"modelBuilder.Entity<{contractHelper.EntityType}>().Property(p => p.RowVersion).IsRowVersion();");
                            }
                        }
                        else
                        {
                            result[^1] = $"{result[^1]};";
                        }
                        result.AddRange(CreateEntityConfigure(type));
                    }
                    result.Add($"ConfigureEntityType({builder});");
                }
                result.Add("}");
                foreach (var type in contractsProject.PersistenceTypes.Where(t => CanEntityModelConfigure(t)))
                {
                    var contractHelper = new Helpers.ContractHelper(type);

                    result.Add($"static partial void ConfigureEntityType(EntityTypeBuilder<{contractHelper.EntityType}> entityTypeBuilder);");
                }
            }
            #endregion CanCreateDoModelCreating()
            return result;
        }

        private static IEnumerable<string> CreateEntityConfigure(Type type)
        {
            var result = new List<string>();
            var contractHelper = new Helpers.ContractHelper(type);
            var properties = contractHelper.GetAllProperties();
            var builder = $"{contractHelper.EntityFieldName}Builder";

            foreach (var item in Helpers.ContractHelper.FilterPropertiesForGeneration(properties))
            {
                var contractPropertyHelper = new Helpers.ContractPropertyHelper(item);

                if (contractPropertyHelper.NotMapped)
                {
                    result.Add($"{builder}");
                    result.Add($".Ignore(c => c.{contractPropertyHelper.PropertyName});");
                }
                else if (contractPropertyHelper.IsUnique)
                {
                    result.Add($"{builder}");
                    result.Add($".HasIndex(c => c.{contractPropertyHelper.PropertyName})");
                    result.Add($".IsUnique();");
                }
                else if (contractPropertyHelper.HasIndex)
                {
                    result.Add($"{builder}");
                    result.Add($".HasIndex(c => c.{contractPropertyHelper.PropertyName});");
                }

                if (contractPropertyHelper.NotMapped == false)
                {
                    var innerResult = new List<string>();

                    if (contractPropertyHelper.IsRequired == true)
                    {
                        innerResult.Add($".IsRequired()");
                    }
                    if (contractPropertyHelper.MaxLength > 0)
                    {
                        innerResult.Add($".HasMaxLength({contractPropertyHelper.MaxLength})");
                    }
                    if (contractPropertyHelper.IsFixedLength)
                    {
                        innerResult.Add($".IsFixedLength()");
                    }
                    if (contractPropertyHelper.ColumnName.HasContent())
                    {
                        innerResult.Add($".HasColumnName(\"{contractPropertyHelper.ColumnName}\")");
                    }
                    if (contractPropertyHelper.DefaultValueSql.HasContent())
                    {
                        innerResult.Add($".HasDefaulValueSql(\"{contractPropertyHelper.DefaultValueSql}\")");
                    }

                    if (innerResult.Count > 0)
                    {
                        innerResult.Insert(0, $"{builder}.Property(p => p.{contractPropertyHelper.PropertyName})");
                        innerResult[^1] = innerResult[^1] + ";";
                        result.AddRange(innerResult);
                    }
                }
            }
            #region Create multicolumn index
            var indexQueries = properties.Select(pi => new Helpers.ContractPropertyHelper(pi))
                                         .Where(cph => cph.NotMapped == false
                                                    && string.IsNullOrEmpty(cph.IndexName) == false)
                                         .GroupBy(cph => cph.IndexName);

            foreach (var index in indexQueries)
            {
                var colIdx = 0;

                result.Add($"{builder}");
                result.Add(".HasIndex(c => new {");
                foreach (var column in index.OrderBy(i => i.IndexColumnOrder))
                {
                    result.Add(colIdx++ == 0 ? $"  c.{column.PropertyName}" : $", c.{column.PropertyName}");
                }
                if (index.Select(i => i.HasUniqueIndexWithName ? 1 : 0).Sum() > 0)
                {
                    result.Add("})");
                    result.Add(".IsUnique();");
                }
                else
                {
                    result.Add("});");
                }
            }
            #endregion Create multicolumn index
            return result;
        }
    }
}
//MdEnd