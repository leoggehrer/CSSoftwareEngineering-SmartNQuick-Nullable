//@BaseCode
//MdStart

using CommonBase.Extensions;
using CommonBase.Modules.Configuration;
using SmartNQuick.AspMvc.Models.ThirdParty;
using SmartNQuick.AspMvc.Modules.Handler;
using SmartNQuick.Contracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.AspMvc.Modules.Language
{
    public partial class Translator
    {
        static Translator()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        private Translator()
        {
            Constructing();
            LoadTranslations();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        private static Translator instance = null;
        public static Translator Instance => instance ??= new Translator();

        public bool HasLoaded => LastLoad.HasValue;
        public DateTime? LastLoad { get; private set; }
        public LanguageCode KeyLanguage => LanguageCode.En;
        public LanguageCode ValueLanguage => LanguageCode.De;

        protected List<Models.ThirdParty.Translation> translations = new();
        protected virtual void LoadTranslations()
        {
            bool LoadTranslationsFromServer(List<Translation> translations)
            {
                var result = false;
                var translationServer = AppSettings.Configuration[StaticLiterals.EnvironmentTranslationServerKey];

                if (translationServer.HasContent())
                {
                    var ctrl = Adapters.Factory.CreateThridParty<Contracts.ThirdParty.ITranslation>(translationServer);
                    var predicate = $"{nameof(Translation.AppName)} == \"{nameof(SmartNQuick)}\" AND {nameof(Translation.KeyLanguage)} == \"{KeyLanguage}\" AND {nameof(Translation.ValueLanguage)} == \"{ValueLanguage}\"";

                    try
                    {
                        var qry = Task.Run(async () =>
                        {
                            return await ctrl.QueryAllAsync(predicate).ConfigureAwait(false);
                        }).Result;

                        translations.Clear();
                        translations.AddRange(qry.Select(e => Translation.Create(e)));
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LastError = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.GetError()}";
                        System.Diagnostics.Debug.WriteLine(ErrorHandler.LastError);
                    }
                }
                return result;
            };

            if (LastLoad.HasValue == false)
            {
                LoadTranslationsFromServer(translations);
                LastLoad = DateTime.Now;
            }
            else
            {
                if ((DateTime.Now - LastLoad.Value).TotalMinutes > 60)
                {
                    LoadTranslationsFromServer(translations);
                    LastLoad = DateTime.Now;
                }
            }
        }
        public virtual void ReloadTranslation()
        {
            LastLoad = null;
            LoadTranslations();
        }
        protected virtual string Translate(string key)
        {
            return Translate(key, key);
        }
        protected virtual string Translate(string key, string defaultValue)
        {
            key.CheckArgument(nameof(key));

            LoadTranslations();
            var result = defaultValue;
            var translation = translations.FirstOrDefault(e => e.Key.Equals(key));

            if (translation != null)
            {
                result = translation.Value;
            }
            else
            {
#if DEBUG
                var csvTranslation = $"Translation;{nameof(SmartNQuick)};{KeyLanguage};{key};{ValueLanguage};";

                System.Diagnostics.Debug.WriteLine(csvTranslation);
#endif
                var splitKey = key.Split(".");

                if (splitKey.Length == 2)
                {
                    translation = translations.FirstOrDefault(e => e.Key.Equals(splitKey[1]));

                    if (translation != null)
                    {
                        result = translation.Value;
                    }
                    else if (defaultValue == key)
                    {
                        result = splitKey[1];
                    }
                }
            }
            return result;
        }

        public static string TranslateIt(string key) => Instance.Translate(key);
        public static string TranslateIt(string key, string defaultValue) => Instance.Translate(key, defaultValue);
    }
}
//MdEnd