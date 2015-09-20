using Domain.Data.Models.Languages;

namespace Services.Interfaces
{
    public interface IGeoLocationService
    {
        string GetCountryIdByIp(string ip);
        string GetCountryCodeByIp(string ip);
        CountryDto GetCountryDtoByIp(string ip);
    }
}
