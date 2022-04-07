//@BaseCode
namespace SmartNQuick.BlazorServerApp
{
    public sealed partial class StaticLiterals
    {
        static StaticLiterals()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        #region Session
        public static string AppStartedTimeKey => nameof(AppStartedTimeKey);
        public static string AuthorizationSessionKey => nameof(AuthorizationSessionKey);
        public static string SessionHistoryKey => nameof(SessionHistoryKey);
        public static string BeforeLoginPageKey => nameof(BeforeLoginPageKey);
        #endregion

        #region Pages
        public static string LoginPage => "Login";
        #endregion Pages
    }
}
