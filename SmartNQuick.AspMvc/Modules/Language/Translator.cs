//@BaseCode
//MdStart

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

        private bool Reload { get; set; } = false;

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

        protected List<Translation> translations = new();
        protected List<Translation> noTranslations = new();

        public bool HasLoaded => LastLoad.HasValue;
        public DateTime? LastLoad { get; private set; }
        public string AppName { get; private set; } = nameof(SmartNQuick);
        public LanguageCode KeyLanguage { get; private set; } = LanguageCode.En;
        public LanguageCode ValueLanguage { get; private set; } = LanguageCode.De;

        public IEnumerable<Translation> NoTranslations => noTranslations;

        protected virtual void LoadTranslations()
        {
            bool LoadTranslationsFromServer(List<Translation> translations)
            {
                var result = false;
                var translationServer = AppSettings.Configuration[StaticLiterals.EnvironmentTranslationServerKey];

                noTranslations.Clear();
                if (translationServer.HasContent())
                {
                    var ctrl = Adapters.Factory.CreateThridParty<Contracts.ThirdParty.ITranslation>(translationServer);
                    var predicate = $"{nameof(Translation.AppName)} == \"{AppName}\" AND {nameof(Translation.KeyLanguage)} == \"{KeyLanguage}\" AND {nameof(Translation.ValueLanguage)} == \"{ValueLanguage}\"";

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
                        ErrorHandler.LastLogicError = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.GetError()}";
                        System.Diagnostics.Debug.WriteLine(ErrorHandler.LastLogicError);
                    }
                }
                return result;
            };

            if (Reload)
            {
                Reload = false;
                LoadTranslationsFromServer(translations);
                LastLoad = DateTime.Now;
            }
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
        public virtual void ReloadTranslations()
        {
            Reload = true;
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
                AppendNoTranslation(key);

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
                        AppendNoTranslation(splitKey[1]);
                    }
                }
            }
            return result;
        }

        protected void AppendNoTranslation(string key)
        {
            var item = noTranslations.SingleOrDefault(t => t.Key.Equals(key) && t.KeyLanguage == KeyLanguage);

            if (item == null)
            {
                noTranslations.Add(new Translation
                {
                    AppName = nameof(SmartNQuick),
                    KeyLanguage = KeyLanguage,
                    Key = key,
                    ValueLanguage = ValueLanguage,
                    Value = default
                });
            }
        }

        public static void ChangeAppName(string appName)
        {
            if (Instance.AppName != appName)
            {
                Instance.Reload = true;
                Instance.AppName = appName;
            }
        }
        public static void ChangeKeyLanguage(LanguageCode languageCode)
        {
            if (Instance.KeyLanguage != languageCode)
            {
                Instance.Reload = true;
                Instance.KeyLanguage = languageCode;
            }
        }
        public static void ChangeValueLanguage(LanguageCode languageCode)
        {
            if (Instance.ValueLanguage != languageCode)
            {
                Instance.Reload = true;
                Instance.ValueLanguage = languageCode;
            }
        }

        public static string TranslateIt(string key) => Instance.Translate(key);
        public static string TranslateIt(string key, string defaultValue) => Instance.Translate(key, defaultValue);
    }
}
//MdEnd