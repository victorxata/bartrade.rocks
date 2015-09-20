using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Data.Models.Languages;

namespace Services.Interfaces
{
    public interface ILanguagesService
    {
        Task<LanguageDto> GetLanguageByIsoCode(string isoCode);
        Task<LanguageDto> GetLanguage(string languageId);
        Task<List<Language>> GetLanguagesByCharacterSetId(string characterSetId);
        Task<List<Language>> GetAllLanguages();
        Task<List<LanguageDto>> GetLanguagesComboItems(string isoCode);

        Task<CountryDto> GetCountryById(string countryId);
        Task<CountryDto> GetCountryByLCID(int lcid);
        Task<List<Country>> GetAllCountries();
        Task<List<CountryDto>> GetCountriesComboItems(string isoCode);

        Task<CharacterSet> GetCharacterSetByCode(string csCode);
        Task<CharacterSet> GetCharacterSetById(string csId);
        Task<List<CharacterSet>> GetAllCharacterSets();
    }
}
