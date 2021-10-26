//@BaseCode
//MdStart

using CommonBase.Extensions;
using System.Collections.Generic;
using System.Linq;

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

        protected List<Models.ThirdParty.Translation> translations = new ();
        protected virtual void LoadTranslations()
        {

        }
        protected virtual string Translate(string key)
        {
            return Translate(key, key);
        }
        protected virtual string Translate(string key, string defaultValue)
        {
            key.CheckArgument(nameof(key));

            var result = defaultValue;
            var translation = translations.FirstOrDefault(e => e.Key.Equals(key));

            if (translation != null)
            {
                result = translation.Value;
            }
            else
            {
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