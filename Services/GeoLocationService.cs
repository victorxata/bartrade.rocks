using Common.GeoLocation;
using Domain.Data.Models.Languages;
using MongoDB.Driver;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly ICountriesRepository _countriesRepository;

        public GeoLocationService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public string GetCountryIdByIp(string ip)
        {
            var result = CountryOriginHelper.RetrieveCountryId(ip);

            if (result == null) return null;

            var country = _countriesRepository.Collection.Find(x => x.Name == result).FirstOrDefaultAsync().Result;

            if (country != null)
                return country.Id;

            return null;

        }

        public string GetCountryCodeByIp(string ip)
        {
            var result = CountryOriginHelper.RetrieveCountryId(ip);
            return result;
        }

        public CountryDto GetCountryDtoByIp(string ip)
        {
            if (ip.Length < 7) // that is e.g. ::1
                return null;
            var result = CountryOriginHelper.RetrieveCountryId(ip);

            if (result == null) return null;

            var country = _countriesRepository.Collection.Find(x => x.Name == result).FirstOrDefaultAsync().Result;

            if (country != null)
                return new CountryDto
                {
                    CountryId = country.Id,
                    EnglishName = country.EnglishName,
                    LCID = country.LCID,
                    Iso31622 = country.Iso31662
                };

            return null;
        }
    }
}
