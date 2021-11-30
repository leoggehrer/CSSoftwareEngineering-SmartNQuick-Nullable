//@BaseCode
//MdStart
namespace SmartNQuick.AspMvc
{
    public static partial class StaticLiterals
	{
		public static string RedirectControllerKey => nameof(RedirectControllerKey);
		public static string EnvironmentTranslationServerKey => "ASPNETCORE_TRANSLATIONSERVER";
		public static string EnvironmentStaticPageServerKey => "ASPNETCORE_STATICPAGESERVER";

        public static int[] DefaultPageSizes { get; private set; } = new int[] { 25, 50, 100, 200 };
        public static string SearchFilterKeyPrefix => "SFILK";
        public static string FilterPredicateKeyPrefix => "FPREDK";

        public static string FilterValuesKey => "FVK";
        public static string PageSizesKeyPrefix => "PSIZSK";
        public static string PageCountKeyPrefix => "PCNTK";
        public static string PageIndexKeyPrefix => "PIDXK";
        public static string PageSizeKeyPrefix => "PSIZK";

        #region Filter
        public static string TypeOperationPostfix => "TO";
        public static string FieldOperationPostfix => "FO";
        public static string OperationEquals => "Equals";
        public static string OperationNumEquals => "Equals";
        public static string OperationNotEquals => "NotEquals";
        public static string OperationContains => "Contains";
        public static string OperationStartsWith => "StartsWith";
        public static string OperationEndsWith => "EndsWith";
        public static string OperationNumIsGreater => "IsGreater";
        public static string OperationNumIsLess => "IsLess";
        #endregion Filter
    }
}
//MdEnd