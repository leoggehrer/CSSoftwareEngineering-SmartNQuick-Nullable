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

        public static string PageSizesKeyPrefix => "PSIZSK";
        public static string PageCountKeyPrefix => "PCNTK";
        public static string PageIndexKeyPrefix => "PIDXK";
        public static string PageSizeKeyPrefix => "PSIZK";
    }
}
//MdEnd