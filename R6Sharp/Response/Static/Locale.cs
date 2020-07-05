using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace R6Sharp.Response.Static
{
    public class Locale
    {
        public enum Language
        {
            AmericanEnglish,
            AustralianEnglish,
            BrazilianPortuguese,
            BritishEnglish,
            CanadianFrench,
            Czech,
            Dutch,
            EuropeanFrench,
            EuropeanSpanish,
            German,
            Italian,
            Japanese,
            Korean,
            LatinSpanish,
            NordicEnglish,
            Polish,
            Russian,
            SimplifiedChinese,
            TraditionalChinese
        }

        /// <summary>
        /// Get string representation of Ubisoft available locales.
        /// </summary>
        private static string FromLanguage(Language language)
        {
            return language switch
            {
                Language.AmericanEnglish => "en-us",
                Language.AustralianEnglish => "en-au",
                Language.BrazilianPortuguese => "pt-br",
                Language.BritishEnglish => "en-gb",
                Language.CanadianFrench => "fr-ca",
                Language.Czech => "cs-cz",
                Language.Dutch => "nl-nl",
                Language.EuropeanFrench => "fr-fr",
                Language.EuropeanSpanish => "es-es",
                Language.German => "de-de",
                Language.Italian => "it-it",
                Language.Japanese => "ja-jp",
                Language.Korean => "ko-kr",
                Language.LatinSpanish => "es-mx",
                Language.NordicEnglish => "en-nordic",
                Language.Polish => "pl-pl",
                Language.Russian => "ru-ru",
                Language.SimplifiedChinese => "zh-cn",
                Language.TraditionalChinese => "zh-tw",
                _ => default,
            };
        }

        /// <summary>
        /// Get hash of the locale file that is present on Ubisoft's servers.
        /// </summary>
        private static string GetLocaleHash(Language language)
        {
            return language switch
            {
                Language.AmericanEnglish => "6eb4f5cd",
                Language.BrazilianPortuguese => "7ad95128",
                Language.EuropeanFrench => "4f78c986",
                Language.Czech => "dc66e300",
                Language.Dutch => "80a0b37a",
                Language.EuropeanSpanish => "5e27d9fa",
                Language.German => "47a861dd",
                Language.Italian => "4ac66a00",
                Language.Japanese => "30330c03",
                Language.Korean => "31170d10",
                Language.LatinSpanish => "979289ed",
                Language.Polish => "845c3d39",
                Language.Russian => "22e559b7",
                Language.SimplifiedChinese => "23f064e8",
                Language.TraditionalChinese => "52f0c5ec",
                _ => default,
            };
        }

        /// <summary>
        /// Get locale dictionary based on requested language.
        /// </summary>
        /// <returns>
        /// Dictionary with list of items (oasisId and value) that contains localization items.
        /// </returns>
        public static async Task<Dictionary<string, string>> GetAllAsync(Language language)
        {
            switch (language)
            {
                case Language.AustralianEnglish:
                case Language.BritishEnglish:
                case Language.NordicEnglish:
                    language = Language.AmericanEnglish;
                    break;
                case Language.CanadianFrench:
                    language = Language.EuropeanFrench;
                    break;
                default:
                    break;
            }

            var formatted = string.Format(Endpoint.Locales, FromLanguage(language), GetLocaleHash(language));
            return await ApiHelper.GetDataAsync<Dictionary<string, string>>(formatted, null, null, null).ConfigureAwait(false);
        }
    }
}
