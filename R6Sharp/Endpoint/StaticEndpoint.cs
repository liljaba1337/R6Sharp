using R6Sharp.Response.Static;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp.Endpoint
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

    public class StaticEndpoint
    {
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
        public async Task<Dictionary<string, string>> GetLocaleAsync(Language language)
        {
            // Transform input language to ones Ubisoft uses for localization
            Language lang;
            switch (language)
            {
                case Language.AustralianEnglish:
                case Language.BritishEnglish:
                case Language.NordicEnglish:
                    lang = Language.AmericanEnglish;
                    break;
                case Language.CanadianFrench:
                    lang = Language.EuropeanFrench;
                    break;
                default:
                    lang = language;
                    break;
            }

            var formatted = string.Format(Endpoints.Locales, FromLanguage(lang), GetLocaleHash(lang));
            var results = await ApiHelper.GetDataAsync(formatted, null, null, null).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(results);
        }

        /// <summary>
        /// Get overall seasons data (season 2 and onwards).
        /// </summary>
        /// <returns>
        /// List of seasons with their respective details like ranks and divisions.
        /// </returns>
        public async Task<List<SeasonDetail>> GetSeasonDetailsAsync()
        {
            // Fetch ranked static data
            var fetch = await ApiHelper.GetDataAsync(Endpoints.Ranks, null, null, null).ConfigureAwait(false);
            var deserialized = JsonSerializer.Deserialize<RankFetch>(fetch);
            // Prefix R6 base URL to the images URL so it is ready to use as it is
            foreach (var season in deserialized.Seasons)
            {
                foreach (var rank in season.Ranks)
                {
                    rank.Images.Default = string.Join('/', Endpoints.Base, rank.Images.Default);
                    rank.Images.Hd = string.Join('/', Endpoints.Base, rank.Images.Hd);
                }
            }
            return deserialized.Seasons;
        }

        /// <summary>
        /// Get information based on past and current seasons.
        /// </summary>
        /// <returns>
        /// Latest season and dictionary of seasons with their respective names and URL to season background.
        /// </returns>
        public async Task<SeasonsInfo> GetSeasonsInfoAsync()
        {
            var results = await ApiHelper.GetDataAsync(Endpoints.Seasons, null, null, null).ConfigureAwait(false);
            return JsonSerializer.Deserialize<SeasonsInfo>(results);
        }

        /// <summary>
        /// Get information specific to a season.
        /// </summary>
        /// <param name="id">
        /// The season number, such as 18 for Steel Wave, or -1 for latest.
        /// </param>
        /// <returns>
        /// Season details.
        /// </returns>
        public async Task<Season> GetSeasonAsync(int id)
        {
            var info = await GetSeasonsInfoAsync().ConfigureAwait(false);
            var season = info.Seasons[id < 0 ? info.LatestSeason.ToString() : id.ToString()];
            season.Id = id < 0 ? info.LatestSeason : id;
            return season;
        }

        /// <inheritdoc/>
        public async Task<Season> GetSeasonAsync()
        {
            return await GetSeasonAsync(-1).ConfigureAwait(false);
        }
    }
}
