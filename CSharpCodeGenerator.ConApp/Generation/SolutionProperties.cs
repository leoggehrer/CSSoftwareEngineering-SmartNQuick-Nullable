//@BaseCode
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCodeGenerator.ConApp.Generation
{
	partial class SolutionProperties
	{
		public static string GeneratedCodeFileName => "_GeneratedCode.cs";
        #region Project-postfixes
        public static string ContractsPostfix => ".Contracts";
        public static string LogicPostfix => ".Logic";
        public static string TransferPostfix => ".Transfer";
        public static string WebApiPostfix => ".WebApi";
        public static string AspMvcPostfix => ".AspMvc";
        #endregion Project-postfixes

        static SolutionProperties()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public SolutionProperties()
        {
            Constructing();

            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
    }
}
