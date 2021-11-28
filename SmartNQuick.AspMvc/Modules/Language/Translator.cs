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

        private bool Reload { get; set; } = false;
        private string appName = nameof(SmartNQuick);
        private LanguageCode keyLanguage = LanguageCode.En;
        private LanguageCode valueLanguage = LanguageCode.De;

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
        public string AppName 
        {
            get => appName; 
            set
            {
                Reload = appName != value;
                appName = value;
            }
        }
        public LanguageCode KeyLanguage 
        {
            get => keyLanguage;
            set
            {
                Reload = keyLanguage != value;
                keyLanguage = value;
            }
        }
        public LanguageCode ValueLanguage 
        {
            get => valueLanguage;
            set
            {
                Reload = valueLanguage != value;
                valueLanguage = value;
            }
        }
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
                AppendNoTranslation(key, result);

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
                        AppendNoTranslation(splitKey[1], result);
                    }
                }
            }
            return result;
        }

        protected void AppendNoTranslation(string key, string value)
        {
            var item = noTranslations.SingleOrDefault(t => t.Key.Equals(key) && t.KeyLanguage == KeyLanguage);

            if (item == null)
            {
                noTranslations.Add(new Translation
                {
                    AppName = AppName,
                    KeyLanguage = KeyLanguage,
                    Key = key,
                    ValueLanguage = ValueLanguage,
                    Value = value
                });
            }
        }

        public static Translator Create()
        {
            return new Translator();
        }
        public static void ChangeAppName(string appName)
        {
            Instance.AppName = appName;
        }
        public static void ChangeKeyLanguage(LanguageCode languageCode)
        {
            Instance.KeyLanguage = languageCode;
        }
        public static void ChangeValueLanguage(LanguageCode languageCode)
        {
            Instance.ValueLanguage = languageCode;
        }

        public static string TranslateIt(string key) => Instance.Translate(key);
        public static string TranslateIt(string key, string defaultValue) => Instance.Translate(key, defaultValue);
    }
}
//MdEnd