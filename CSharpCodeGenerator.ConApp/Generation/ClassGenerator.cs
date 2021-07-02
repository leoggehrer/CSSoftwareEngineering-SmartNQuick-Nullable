//@BaseCode
using CommonBase.Extensions;
using CSharpCodeGenerator.ConApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpCodeGenerator.ConApp.Generation
{
	partial class ClassGenerator : GeneratorObject
	{
        static ClassGenerator()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public ClassGenerator()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        #region Create constructors
        public static IEnumerable<string> CreatePartialStaticConstrutor(string className, IEnumerable<string> initStatements = null)
        {
            var lines = new List<string>
            {
                $"static {className}()",
                "{",
                "ClassConstructing();"
            };
            if (initStatements != null)
            {
                foreach (var item in initStatements)
                {
                    lines.Add($"{item}");
                }
            }
            lines.Add($"ClassConstructed();");
            lines.Add("}");
            lines.Add("static partial void ClassConstructing();");
            lines.Add("static partial void ClassConstructed();");
            lines.Add(string.Empty);
            return lines;
        }
        public static IEnumerable<string> CreatePartialConstrutor(string visibility, string className, string argumentList = null, string baseConstructor = null, IEnumerable<string> initStatements = null, bool withPartials = true)
        {
            var lines = new List<string>
            {
                $"{visibility} {className}({argumentList})"
            };

            if (string.IsNullOrEmpty(baseConstructor) == false)
                lines.Add($":{baseConstructor}");

            lines.Add("{");
            lines.Add("Constructing();");
            if (initStatements != null)
            {
                foreach (var item in initStatements)
                {
                    lines.Add($"{item}");
                }
            }
            else
            {
                lines.Add(string.Empty);
            }
            lines.Add($"Constructed();");
            lines.Add("}");
            if (withPartials)
            {
                lines.Add("partial void Constructing();");
                lines.Add("partial void Constructed();");
            }
            return lines;
        }
        #endregion Create constructors
        #region Create factory methode
        public static IEnumerable<string> CreateFactoryMethods(Type type, bool newPrefix)
        {
            type.CheckArgument(nameof(type));

            var result = new List<string>();
            var entityType = CreateEntityTypeFromInterface(type);

            result.Add($"public{(newPrefix ? " new " : " ")}static {entityType} Create()");
            result.Add("{");
            result.Add("BeforeCreate();");
            result.Add($"var result = new {entityType}();");
            result.Add("AfterCreate(result);");
            result.Add("return result;");
            result.Add("}");

            result.Add($"public{(newPrefix ? " new " : " ")}static {entityType} Create(object other)");
            result.Add("{");
            result.Add("BeforeCreate(other);");
            result.Add("CommonBase.Extensions.ObjectExtensions.CheckArgument(other, nameof(other));");
            result.Add($"var result = new {entityType}();");
            result.Add("CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);");
            result.Add("AfterCreate(result, other);");
            result.Add("return result;");
            result.Add("}");

            result.Add($"public static {entityType} Create({type.FullName} other)");
            result.Add("{");
            result.Add("BeforeCreate(other);");
            result.Add($"var result = new {entityType}();");
            result.Add("result.CopyProperties(other);");
            result.Add("AfterCreate(result, other);");
            result.Add("return result;");
            result.Add("}");

            result.Add("static partial void BeforeCreate();");
            result.Add($"static partial void AfterCreate({entityType} instance);");

            result.Add("static partial void BeforeCreate(object other);");
            result.Add($"static partial void AfterCreate({entityType} instance, object other);");

            result.Add($"static partial void BeforeCreate({type.FullName} other);");
            result.Add($"static partial void AfterCreate({entityType} instance, {type.FullName} other);");
            return result;
        }
        #endregion Create factory methode

        #region Create partial model
        public static IEnumerable<string> CreateModelFromInterface(Type type,
                                                                   Action<Type, List<string>> createAttributes = null,
                                                                   Action<Type, PropertyInfo, List<string>> createPropertyAttributes = null)
        {
            type.CheckArgument(nameof(type));

            var result = new List<string>();
            var entityName = CreateEntityNameFromInterface(type);
            var properties = ContractHelper.GetAllProperties(type);

            createAttributes?.Invoke(type, result);
            result.Add($"public partial class {entityName} : {type.FullName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(entityName));
            result.AddRange(CreatePartialConstrutor("public", entityName));
            foreach (var item in ContractHelper.FilterPropertiesForGeneration(properties))
            {
                createPropertyAttributes?.Invoke(type, item, result);
                result.AddRange(CreateProperty(item));
            }
            result.AddRange(CreateCopyProperties(type));
            result.AddRange(CreateFactoryMethods(type, false));
            result.Add("}");
            return result;
        }
        #endregion Create partial model

        #region Create partial property
        static partial void SetPropertyAttributes(Type type, PropertyInfo propertyInfo, List<string> codeLines);
        static partial void SetPropertyGetAttributes(Type type, PropertyInfo propertyInfo, List<string> codeLines);
        static partial void SetPropertySetAttributes(Type type, PropertyInfo propertyInfo, List<string> codeLines);
        static partial void GetPropertyDefaultValue(Type type, PropertyInfo propertyInfo, ref string defaultValue);

        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Eigenschaft (Auto-Property oder Partial-Full-Property).
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <returns>Die Eigenschaft als Text.</returns>
        public static IEnumerable<string> CreateProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            IEnumerable<string> result;
            var contractPropertyHelper = new ContractPropertyHelper(propertyInfo);


            if (contractPropertyHelper.IsAutoProperty)
            {
                result = CreateAutoProperty(propertyInfo);
            }
            else
            {
                result = CreatePartialProperty(propertyInfo);
            }
            return result;
        }

        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Eigenschaft (Auto-Property).
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <returns>Die Eigenschaft als Text.</returns>
        public static IEnumerable<string> CreateAutoProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var result = new List<string>();
            var contractPropertyHelper = new ContractPropertyHelper(propertyInfo);
            var defaultValue = contractPropertyHelper.DefaultValue;
            var fieldType = contractPropertyHelper.PropertyFieldType;

            result.Add(string.Empty);
            SetPropertyAttributes(propertyInfo.DeclaringType, propertyInfo, result);
            result.Add($"public {fieldType} {propertyInfo.Name}");
            result.Add(string.IsNullOrEmpty(defaultValue)
                ? "{ get; set; }"
                : "{ get; set; }" + $" = {defaultValue};");

            GetPropertyDefaultValue(propertyInfo.DeclaringType, propertyInfo, ref defaultValue);
            return result;
        }
        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Eigenschaft (Partial-Full-Property).
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <returns>Die Eigenschaft als Text.</returns>
        public static IEnumerable<string> CreatePartialProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var result = new List<string>();
            var contractPropertyHelper = new ContractPropertyHelper(propertyInfo);
            var defaultValue = contractPropertyHelper.DefaultValue;
            var fieldType = contractPropertyHelper.PropertyFieldType;
            var fieldName = contractPropertyHelper.PropertyFieldName;
            var paramName = contractPropertyHelper.PropertyFieldName;

            result.Add(string.Empty);
            SetPropertyAttributes(propertyInfo.DeclaringType, propertyInfo, result);
            result.Add($"public {fieldType} {propertyInfo.Name}");
            result.Add("{");
            result.AddRange(CreatePartialGetProperty(propertyInfo));
            result.AddRange(CreatePartialSetProperty(propertyInfo));
            result.Add("}");

            GetPropertyDefaultValue(propertyInfo.DeclaringType, propertyInfo, ref defaultValue);
            result.Add(string.IsNullOrEmpty(defaultValue)
                ? $"private {fieldType} {fieldName};"
                : $"private {fieldType} {fieldName} = {defaultValue};");

            result.Add($"partial void On{propertyInfo.Name}Reading();");
            result.Add($"partial void On{propertyInfo.Name}Changing(ref bool handled, ref {fieldType} {paramName});");
            result.Add($"partial void On{propertyInfo.Name}Changed();");
            return result;
        }
        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Getter-Eigenschaft aus dem Eigenschaftsinfo-Objekt.
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <param name="generationType">The type of the generation that should be performed</param>
        /// <returns>Die Getter-Eigenschaft als Text.</returns>
        public static IEnumerable<string> CreatePartialGetProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var result = new List<string>();
            var fieldName = CreateFieldName(propertyInfo, "_");

            SetPropertyGetAttributes(propertyInfo.DeclaringType, propertyInfo, result);
            result.Add("get");
            result.Add("{");
            result.Add($"On{propertyInfo.Name}Reading();");
            result.Add($"return {fieldName};");
            result.Add("}");
            return result;
        }
        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Setter-Eigenschaft aus dem Eigenschaftsinfo-Objekt.
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <returns>Die Setter-Eigenschaft als Text.</returns>
        public static IEnumerable<string> CreatePartialSetProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.CheckArgument(nameof(propertyInfo));

            var result = new List<string>();
            var propName = propertyInfo.Name;
            var fieldName = CreateFieldName(propertyInfo, "_");

            SetPropertySetAttributes(propertyInfo.DeclaringType, propertyInfo, result);

            result.Add("set");
            result.Add("{");
            result.Add("bool handled = false;");
            result.Add($"On{propName}Changing(ref handled, ref {fieldName});");
            result.Add("if (handled == false)");
            result.Add("{");
            result.Add($"{fieldName} = value;");
            result.Add("}");
            result.Add($"On{propName}Changed();");
            result.Add("}");
            return result.ToArray();
        }
        #endregion Create partial properties

        #region CopyProperties
        public static IEnumerable<string> CreateCopyProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            return CreateCopyProperties(type, type.FullName);
        }
        public static IEnumerable<string> CreateCopyProperties(Type type, string copyType)
        {
            type.CheckArgument(nameof(type));
            copyType.CheckArgument(nameof(copyType));

            var result = new List<string>
            {
                $"public void CopyProperties({copyType} other)",
                "{",
                "if (other == null)",
                "{",
                "throw new System.ArgumentNullException(nameof(other));",
                "}",
                string.Empty,
                "bool handled = false;",
                "BeforeCopyProperties(other, ref handled);",
                "if (handled == false)",
                "{"
            };

            foreach (var item in ContractHelper.GetAllProperties(type))
            {
                var contractPropertyHelper = new ContractPropertyHelper(item);

                if (contractPropertyHelper.HasImplementation == false)
                {
                    if (item.Name.Equals(StaticLiterals.ConnectorItemName) && item.DeclaringType.Name.Equals(StaticLiterals.ICompositeName))
                    {
                        result.Add($"{StaticLiterals.ConnectorItemName}.CopyProperties(other.{StaticLiterals.ConnectorItemName});");
                    }
                    else if (item.Name.Equals(StaticLiterals.OneItemName) && item.DeclaringType.Name.Equals(StaticLiterals.ICompositeName))
                    {
                        result.Add($"{StaticLiterals.OneItemName}.CopyProperties(other.{StaticLiterals.OneItemName});");
                    }
                    else if (item.Name.Equals(StaticLiterals.AnotherItemName) && item.DeclaringType.Name.Equals(StaticLiterals.ICompositeName))
                    {
                        result.Add($"{StaticLiterals.AnotherItemName}.CopyProperties(other.{StaticLiterals.AnotherItemName});");
                    }
                    else if (item.Name.Equals(StaticLiterals.OneItemName) && item.DeclaringType.Name.Equals(StaticLiterals.IOneToAnotherName))
                    {
                        result.Add($"{StaticLiterals.OneItemName}.CopyProperties(other.{StaticLiterals.OneItemName});");
                    }
                    else if (item.Name.Equals(StaticLiterals.AnotherItemName) && item.DeclaringType.Name.Equals(StaticLiterals.IOneToAnotherName))
                    {
                        result.Add($"{StaticLiterals.AnotherItemName}.CopyProperties(other.{StaticLiterals.AnotherItemName});");
                    }
                    else if (item.Name.Equals(StaticLiterals.OneItemName) && item.DeclaringType.Name.Equals(StaticLiterals.IOneToManyName))
                    {
                        result.Add($"{StaticLiterals.OneItemName}.CopyProperties(other.{StaticLiterals.OneItemName});");
                    }
                    else if (item.Name.Equals(StaticLiterals.ManyItemsName) && item.DeclaringType.Name.Equals(StaticLiterals.IOneToManyName))
                    {
                        result.Add($"Clear{StaticLiterals.ManyItemsName}();");
                        result.Add($"foreach (var item in other.{StaticLiterals.ManyItemsName})");
                        result.Add("{");
                        result.Add($"Add{StaticLiterals.ManyItemName}(item);");
                        result.Add("}");
                    }
                    else if (item.CanRead)
                    {
                        result.Add($"{item.Name} = other.{item.Name};");
                    }
                }
            }
            result.Add("}");
            result.Add("AfterCopyProperties(other);");
            result.Add("}");

            result.Add($"partial void BeforeCopyProperties({copyType} other, ref bool handled);");
            result.Add($"partial void AfterCopyProperties({copyType} other);");

            return result;
        }
        public static IEnumerable<string> CreateDelegateCopyProperties(Type type)
        {
            type.CheckArgument(nameof(type));

            return CreateDelegateCopyProperties(type, type.FullName);
        }
        public static IEnumerable<string> CreateDelegateCopyProperties(Type type, string copyType)
        {
            type.CheckArgument(nameof(type));

            var result = new List<string>
            {
                $"public void CopyProperties({copyType} other)",
                "{",
                "if (other == null)",
                "{",
                "throw new System.ArgumentNullException(nameof(other));",
                "}",
                string.Empty,
                "bool handled = false;",
                "BeforeCopyProperties(other, ref handled);",
                "if (handled == false)",
                "{",
            };
            result.Add($"{StaticLiterals.DelegatePropertyName}.CopyProperties(other as {type.FullName});");
            result.Add("}");
            result.Add("AfterCopyProperties(other);");
            result.Add("}");

            result.Add($"partial void BeforeCopyProperties({copyType} other, ref bool handled);");
            result.Add($"partial void AfterCopyProperties({copyType} other);");

            return result;
        }
        #endregion CopyProperties

        /// <summary>
        /// Diese Methode erstellt den Programmcode fuer das Vergleichen der Eigenschaften.
        /// </summary>
        /// <param name="type">Die Schnittstellen-Typ Information.</param>
        /// <returns>Die Equals-Methode als Text.</returns>
        public static IEnumerable<string> CreateEquals(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new List<string>();
            var counter = 0;
            var properties = ContractHelper.GetAllProperties(type);
            var filteredProperties = ContractHelper.FilterPropertiesForGeneration(properties);

            if (filteredProperties.Any())
            {
                result.Add($"public override bool Equals(object obj)");
                result.Add("{");
                result.Add($"if (obj is not {type.FullName} instance)");
                result.Add("{");
                result.Add("return false;");
                result.Add("}");
                result.Add("return base.Equals(instance) && Equals(instance);");
                result.Add("}");
                result.Add(string.Empty);
                result.Add($"protected bool Equals({type.FullName} other)");
                result.Add("{");
                result.Add("if (other == null)");
                result.Add("{");
                result.Add("return false;");
                result.Add("}");

                foreach (var pi in filteredProperties)
                {
                    if (pi.CanRead)
                    {
                        var codeLine = counter == 0 ? "return " : "       && ";

                        if (pi.PropertyType.IsValueType)
                        {
                            codeLine += $"{pi.Name} == other.{pi.Name}";
                        }
                        else
                        {
                            codeLine += $"IsEqualsWith({pi.Name}, other.{pi.Name})";
                        }
                        result.Add(codeLine);
                        counter++;
                    }
                }
                if (counter > 0)
                {
                    result[^1] = $"{result[^1]};";
                }
                else
                {
                    result.Add("return true;");
                }
                result.Add("}");
            }
            return result;
        }
        /// <summary>
        /// Diese Methode erstellt den Programmcode fuer die Berechnung des Hash-Codes.
        /// </summary>
        /// <param name="type">Die Schnittstellen-Typ Information.</param>
        /// <returns>Die GetHashCode-Methode als Text.</returns>
        public static IEnumerable<string> CreateGetHashCode(Type type)
        {
            type.CheckArgument(nameof(type));

            var result = new List<string>();

            var braces = 0;
            var counter = 0;
            var codeLine = string.Empty;
            var properties = ContractHelper.GetAllProperties(type);
            var filteredProperties = ContractHelper.FilterPropertiesForGeneration(properties);

            if (filteredProperties.Any())
            {
                result.Add($"public override int GetHashCode()");
                result.Add("{");

                foreach (var pi in filteredProperties)
                {
                    if (pi.CanRead)
                    {
                        if (counter == 0)
                        {
                            braces++;
                            codeLine = "HashCode.Combine(";
                        }
                        else if (counter % 6 == 0)
                        {
                            braces++;
                            codeLine += ", HashCode.Combine(";
                        }
                        else
                        {
                            codeLine += ", ";
                        }
                        codeLine += pi.Name;
                        counter++;
                    }
                }
                for (int i = 0; i < braces; i++)
                {
                    codeLine += ")";
                }

                if (counter > 0)
                {
                    result.Add($"return {codeLine};");
                }
                else
                {
                    result.Add($"return base.GetHashCode();");
                }
                result.Add("}");
            }
            return result;
        }

    }
}
