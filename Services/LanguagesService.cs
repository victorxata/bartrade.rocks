using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Data.Models.Languages;
using MongoDB.Bson;
using MongoDB.Driver;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class LanguagesService : ILanguagesService
    {
        #region Private

        private readonly ILanguagesRepository _languagesRepository;
        private readonly ICharacterSetsRepository _characterSetsRepository;
        private readonly ICountriesRepository _countriesRepository;

        #endregion

        #region Constructor

        public LanguagesService(ILanguagesRepository languagesRepository, ICharacterSetsRepository characterSetsRepository, ICountriesRepository countriesRepository)
        {
            _languagesRepository = languagesRepository;
            _characterSetsRepository = characterSetsRepository;
            _countriesRepository = countriesRepository;
        }

        #endregion

        #region Languages

        public async Task<LanguageDto> GetLanguageByIsoCode(string isoCode)
        {
            var lang = await _languagesRepository.Collection.Find(x => x.IsoCode == isoCode).FirstOrDefaultAsync();
            if (lang != null)
            {
                var translation =
                    lang.Translations.FirstOrDefault(xx => (xx.IsoCode == isoCode) || (xx.IsoCode == "en-US"));
                var result = new LanguageDto
                {
                    LanguageId = lang.Id,
                    EnglishName = lang.EnglishName,
                    IsoCode = lang.IsoCode,
                    CustomCode = lang.CustomCode,
                    TranslatedName = translation?.TranslationText
                };
                return result;
            }
            return null;
        }

        public async Task<LanguageDto> GetLanguage(string languageId)
        {
            var lang = await _languagesRepository.Collection.Find(x => x.Id == languageId).FirstOrDefaultAsync();

            if (lang == null) return null;

            var result = new LanguageDto
            {
                LanguageId = lang.Id,
                EnglishName = lang.EnglishName,
                IsoCode = lang.IsoCode,
                CustomCode = lang.CustomCode
            };

            var translatedList = lang.Translations;
            var translatedItem = translatedList?.FirstOrDefault(xx => xx.IsoCode == lang.IsoCode);
            if (translatedItem != null)
                result.TranslatedName = translatedItem.TranslationText;
            return result;
        }

        public async Task<List<Language>> GetLanguagesByCharacterSetId(string characterSetId)
        {
            var filter = Builders<Language>.Filter;
            var result =
                await
                    _languagesRepository.Collection.Find(filter.Eq(x => x.CharacterSet.Id, characterSetId))
                        .ToListAsync();

            return result;
        }

        public async Task<List<Language>> GetAllLanguages()
        {
            var result = await _languagesRepository.Collection.Find(new BsonDocument()).ToListAsync();

            return result;
        }

        public async Task<List<LanguageDto>> GetLanguagesComboItems(string isoCode)
        {
            var projection =
                Builders<Language>.Projection.Include(x => x.Id).Include(x => x.IsoCode).Include(x => x.EnglishName);

            var result = await _languagesRepository
                .Collection
                .Find(new BsonDocument())
                .Project<BsonDocument>(projection)
                .Sort(Builders<Language>.Sort.Ascending(x => x.EnglishName))
                .ToListAsync();

            return result.Select(language => new LanguageDto
            {
                LanguageId = language["_id"].ToString(),
                EnglishName = language["EnglishName"].ToString(),
                IsoCode = language["IsoCode"].ToString(),
                TranslatedName = language["EnglishName"].ToString()
            }).ToList();
        }

        #endregion

        #region Countries

        public async Task<CountryDto> GetCountryById(string countryId)
        {
            var country = await _countriesRepository.Collection.Find(x => x.Id == countryId).FirstOrDefaultAsync();
            if (country != null)
                return await GetCountryDto(country);
            return null;
        }

        private async Task<CountryDto> GetCountryDto(Country country, string isoCode = "en-us")
        {
            await Task.Run(() =>
            {
                LanguageTranslation firstOrDefault = null;

                    if (country.Translations != null)
                    {
                        firstOrDefault = country.Translations.FirstOrDefault(x => x.IsoCode.ToLower() == isoCode);
                    }
                    var translated = firstOrDefault == null ? string.Empty : firstOrDefault.TranslationText;

                    return new CountryDto
                    {
                        LCID = country.LCID,
                        CountryId = country.Id,
                        EnglishName = country.EnglishName,
                        TranslatedName = translated
                    };
            });
            return null;
        }

        public async Task<CountryDto> GetCountryByLCID(int lcid)
        {
            var country = await _countriesRepository.Collection.Find(x => x.LCID == lcid).FirstOrDefaultAsync();
            if (country != null)
                return await GetCountryDto(country);
            return null;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            var result = await _countriesRepository.Collection.Find(new BsonDocument()).ToListAsync();

            return result;
        }

        public async Task<List<CountryDto>> GetCountriesComboItems(string isoCode)
        {
            var projection =
                Builders<Country>.Projection.Include(x => x.Id).Include(x => x.LCID).Include(x => x.EnglishName);

            var result = await _countriesRepository
                .Collection.Find(new BsonDocument())
                .Project<BsonDocument>(projection)
                .Sort(Builders<Country>.Sort.Ascending(x => x.EnglishName))
                .ToListAsync();

            return result.Select(country => new CountryDto
            {
                CountryId = country["_id"].ToString(),
                EnglishName = country["EnglishName"].ToString(),
                LCID = Convert.ToInt32(country["LCID"])
            }).ToList();
        }

        #endregion

        #region CharacterSets

        public async Task<CharacterSet> GetCharacterSetByCode(string csCode)
        {
            return await _characterSetsRepository.Collection.Find(x => x.Code == csCode).FirstOrDefaultAsync();
        }

        public async Task<CharacterSet> GetCharacterSetById(string csId)
        {
            return await _characterSetsRepository.Collection.Find(x => x.Id == csId).FirstOrDefaultAsync();
        }

        public async Task<List<CharacterSet>> GetAllCharacterSets()
        {
            return await _characterSetsRepository.Collection.Find(new BsonDocument()).ToListAsync();
        }
        
        #endregion
    }
}
