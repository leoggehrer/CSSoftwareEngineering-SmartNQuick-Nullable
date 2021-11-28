//@BaseCode
//MdStart
using CommonBase.Extensions;
using CommonBase.Modules.Configuration;
using SmartNQuick.AspMvc.Models.ThirdParty;
using SmartNQuick.AspMvc.Modules.Handler;
using SmartNQuick.Contracts.Modules.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Modules.StaticPage
{
    public class StaticPageService
    {
        public static HtmlItem GetPageContent(string pageName)
        {
            return GetPageContent(nameof(SmartNQuick), pageName);
        }
        public static HtmlItem GetPageContent(string appName, string pageName)
        {
            var result = default(HtmlItem);
            var staticPageServer = AppSettings.Configuration[StaticLiterals.EnvironmentStaticPageServerKey];

            if (staticPageServer.HasContent())
            {
                var ctrl = Adapters.Factory.CreateThridParty<Contracts.ThirdParty.IHtmlItem>(staticPageServer);
                var predicate = $"{nameof(HtmlItem.AppName)} == \"{appName}\" AND {nameof(HtmlItem.Key)} == \"{pageName}\" AND {nameof(HtmlItem.State)} == \"{State.Active}\"";

                try
                {
                    var qry = Task.Run(async () =>
                    {
                        return await ctrl.QueryAllAsync(predicate).ConfigureAwait(false);
                    }).Result;

                    if (qry.Any())
                    {
                        result = HtmlItem.Create(qry.ElementAt(0));
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.LastLogicError = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.GetError()}";
                    System.Diagnostics.Debug.WriteLine(ErrorHandler.LastLogicError);
                }
            }
            return result;
        }

        public static string GetPartialContent(string key, string defaultContent)
        {
            return GetPartialContent(nameof(SmartNQuick), key, defaultContent);
        }
        public static string GetPartialContent(string appName, string key, string defaultContent)
        {
            var result = defaultContent;
            var staticPageServer = AppSettings.Configuration[StaticLiterals.EnvironmentStaticPageServerKey];

            if (staticPageServer.HasContent())
            {
                var ctrl = Adapters.Factory.CreateThridParty<Contracts.ThirdParty.IHtmlItem>(staticPageServer);
                var predicate = $"{nameof(HtmlItem.AppName)} == \"{appName}\" AND {nameof(HtmlItem.Key)} == \"{key}\" AND {nameof(HtmlItem.State)} == \"{State.Active}\"";

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
                    ErrorHandler.LastLogicError = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.GetError()}";
                    System.Diagnostics.Debug.WriteLine(ErrorHandler.LastLogicError);
                }
            }
            return result;
        }
    }
}
//MdEnd