//@BaseCode
//MdStart
using CommonBase.Modules.Configuration;
using SmartNQuick.AspMvc.Models.ThirdParty;
using SmartNQuick.AspMvc.Modules.Handler;
using SmartNQuick.Contracts.Modules.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Modules.ThirdPartyServices

{
    public partial class HtmlStoreService
    {
        private static DateTime? lastErrorTime;
        private static bool canServiceAcessed = true;

        private static DateTime? LastErrorTime
        {
            get => lastErrorTime;
            set
            {
                lastErrorTime = value;
                canServiceAcessed = value == null;
                ServiceIsAccessible();
            }
        }
        private static bool CanServiceAcessed => canServiceAcessed;

        private static bool ServiceIsAccessible()
        {
            var htmlStoreServer = AppSettings.Configuration[StaticLiterals.EnvironmentHtmlStoreServerKey];

            if (htmlStoreServer.HasContent() && canServiceAcessed == false)
            {
                var ctrl = Adapters.Factory.CreateThridParty<Contracts.ThirdParty.IStaticPage>(htmlStoreServer);

                Task.Run(() =>
                {
                    while (canServiceAcessed == false)
                    {
                        try
                        {
                            var maxSize = ctrl.MaxPageSize;

                            canServiceAcessed = true;
                        }
                        catch
                        {
                            Task.Delay(60_000).Wait();
                        }
                    }
                });
            }
            return canServiceAcessed;
        }
        public static StaticPage GetPageContent(string pageName)
        {
            return GetPageContent(nameof(SmartNQuick), pageName);
        }
        public static StaticPage GetPageContent(string appName, string pageName)
        {
            var result = default(StaticPage);
            var htmlStoreServer = AppSettings.Configuration[StaticLiterals.EnvironmentHtmlStoreServerKey];

            if (htmlStoreServer.HasContent() && CanServiceAcessed)
            {
                var ctrl = Adapters.Factory.CreateThridParty<Contracts.ThirdParty.IStaticPage>(htmlStoreServer);
                var predicate = $"{nameof(StaticPage.AppName)} == \"{appName}\" AND {nameof(StaticPage.Key)} == \"{pageName}\" AND {nameof(StaticPage.State)} == \"{State.Active}\"";

                try
                {
                    var qry = Task.Run(async () =>
                    {
                        return await ctrl.QueryAllAsync(predicate).ConfigureAwait(false);
                    }).Result;

                    if (qry.Any())
                    {
                        result = StaticPage.Create(qry.ElementAt(0));
                    }
                }
                catch (Exception ex)
                {
                    LastErrorTime = DateTime.UtcNow;
                    ErrorHandler.LastLogicError = $"{System.Reflection.MethodBase.GetCurrentMethod()?.Name}: {ex.GetError()}";
                    System.Diagnostics.Debug.WriteLine(ErrorHandler.LastLogicError);
                }
            }
            return result;
        }

        public static string GetHtmlElement(string key, string defaultContent)
        {
            return GetHtmlElement(nameof(SmartNQuick), key, defaultContent);
        }
        public static string GetHtmlElement(string appName, string key, string defaultContent)
        {
            var result = defaultContent;
            var htmlStoreServer = AppSettings.Configuration[StaticLiterals.EnvironmentHtmlStoreServerKey];

            if (htmlStoreServer.HasContent() && CanServiceAcessed)
            {
                var ctrl = Adapters.Factory.CreateThridParty<Contracts.ThirdParty.IHtmlElement>(htmlStoreServer);
                var predicate = $"{nameof(HtmlElement.AppName)} == \"{appName}\" AND {nameof(HtmlElement.Key)} == \"{key}\" AND {nameof(HtmlElement.State)} == \"{State.Active}\"";

                try
                {
                    var qry = Task.Run(async () =>
                    {
                        return await ctrl.QueryAllAsync(predicate).ConfigureAwait(false);
                    }).Result;

                    if (qry.Any())
                    {
                        result = qry.ElementAt(0).Content;
                    }
                }
                catch (Exception ex)
                {
                    LastErrorTime = DateTime.UtcNow;
                    ErrorHandler.LastLogicError = $"{System.Reflection.MethodBase.GetCurrentMethod()?.Name}: {ex.GetError()}";
                    System.Diagnostics.Debug.WriteLine(ErrorHandler.LastLogicError);
                }
            }
            return result;
        }

        public static string GetHtmlAttribute(string key, string defaultContent)
        {
            return GetHtmlAttribute(nameof(SmartNQuick), key, defaultContent);
        }
        public static string GetHtmlAttribute(string appName, string key, string defaultContent)
        {
            var result = defaultContent;
            var htmlStoreServer = AppSettings.Configuration[StaticLiterals.EnvironmentHtmlStoreServerKey];

            if (htmlStoreServer.HasContent() && CanServiceAcessed)
            {
                var ctrl = Adapters.Factory.CreateThridParty<Contracts.ThirdParty.IHtmlAttribute>(htmlStoreServer);
                var predicate = $"{nameof(HtmlAttribute.AppName)} == \"{appName}\" AND {nameof(HtmlAttribute.Key)} == \"{key}\" AND {nameof(HtmlAttribute.State)} == \"{State.Active}\"";

                try
                {
                    var qry = Task.Run(async () =>
                    {
                        return await ctrl.QueryAllAsync(predicate).ConfigureAwait(false);
                    }).Result;

                    if (qry.Any())
                    {
                        result = qry.ElementAt(0).Content;
                    }
                }
                catch (Exception ex)
                {
                    LastErrorTime = DateTime.UtcNow;
                    ErrorHandler.LastLogicError = $"{System.Reflection.MethodBase.GetCurrentMethod()?.Name}: {ex.GetError()}";
                    System.Diagnostics.Debug.WriteLine(ErrorHandler.LastLogicError);
                }
            }
            return result;
        }
    }
}
//MdEnd