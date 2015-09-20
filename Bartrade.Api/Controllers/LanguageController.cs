using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Utils;
using Services.Interfaces;

namespace Api.Controllers
{
    /// <summary>
    /// API to obtain different items and lists related to the Languages, e.g. Countries, Languages, etc.
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/languages")]
    public class LanguagesController : ApiController
    {
        private readonly ILanguagesService _languageServices;
        private readonly IGeoLocationService _geoLocationServices;

        /// <summary>
        /// Constructor of the LanguagesController
        /// </summary>
        /// <param name="languageServices">To inject the languagesServices object</param>
        /// <param name="geoLocationServices">To inject the geoLocationServices object</param>
        public LanguagesController(ILanguagesService languageServices, IGeoLocationService geoLocationServices)
        {
            _languageServices = languageServices;
            _geoLocationServices = geoLocationServices;
        }

        // GET api/languages/language 
        /// <summary>
        /// Obtain a LanguageDto object given its id
        /// </summary>
        /// <param name="id">The id of the language</param>
        /// <returns>A LanguageDto object</returns>
        [Route("Country/IP/{id}")]
        [HttpGet]
        public string Get(string id)
        {
            var result = _geoLocationServices.GetCountryIdByIp(id);
            return result;
        }

        // GET api/languages/language 
        /// <summary>
        /// Obtain a LanguageDto object given its id
        /// </summary>
        /// <param name="id">The id of the language</param>
        /// <returns>A LanguageDto object</returns>
        [Route("language/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLanguage(string id)
        {
            var result = await _languageServices.GetLanguage(id);
            return Ok(result);
        }

        // GET api/languages/combo/ISOCode 
        /// <summary>
        /// Obtain a list of LanguageDto objects with the translations in the given iso language code
        /// </summary>
        /// <param name="id">The language to obtain the translations. E.g. en-US</param>
        /// <returns>A list of LanguageDto objects</returns>
        [Route("combo/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCombo(string id)
        {
            var result = await _languageServices.GetLanguagesComboItems(id);
            return Ok(result);
        }

        // GET api/languages/initialdata/en-EN - TESTED
        /// <summary>
        /// Obtain the initial data: a list of Languages and a list of Countries
        /// </summary>
        /// <param name="id">The language to obtain the translations. E.g. en-US</param>
        /// <returns>A list of Languages and Countries objects</returns>
        [Route("initialdata/{id}")]
        [HttpGet]
        public HttpResponseMessage GetInitialData(string id)
        {
            var language = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(x => new
            {
                x.Name,
                x.EnglishName
            });
            var countries = Misc.GetISO3166CountryList().Select(x => new
            {
                x.Region.Name,
                x.Region.EnglishName
            });

            var pack = new { countries, language };
            
            //var result = new InitialData
            //{
            //    Countries = await _languageServices.GetCountriesComboItems(id),
            //    Language = await _languageServices.GetLanguagesComboItems(id)
            //};

            var response = Request.CreateResponse(HttpStatusCode.OK, pack);
            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromDays(5),
                MustRevalidate = true,
                Private = false
            };
            return response;
        }

        // GET api/languages/combo/ISOCode
        /// <summary>
        /// Obtain a list of languages given its CharacterSetId
        /// </summary>
        /// <param name="id">The character set id </param>
        /// <returns>A list of Language objects</returns>
        [Route("allbycharacterset/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllLanguagesByCharacterId(string id)
        {
            var result = await _languageServices.GetLanguagesByCharacterSetId(id);
            return Ok(result);
        }

        // GET api/languages/countries/ISOCode 
        /// <summary>
        /// Obtain a list of CountryDto objects with the translations in the given iso language code
        /// </summary>
        /// <param name="id">The language to obtain the translations. E.g. en-US</param>
        /// <returns>A list of CountryDto objects</returns>
        [Route("countries/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCountriesCombo(string id)
        {
            var result = await _languageServices.GetCountriesComboItems(id);
            return Ok(result);
        }

        // GET api/languages/language 
        /// <summary>
        /// Obtain a CountryDto object given its id
        /// </summary>
        /// <param name="id">The Id of the Country to obtain. E.g. 134</param>
        /// <returns>A CountryDto object</returns>
        [Route("country/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCountry(string id)
        {
            var result = await _languageServices.GetCountryById(id);
            return Ok(result);
        }

        // GET api/languages/encodings 
        /// <summary>
        /// Obtain a queryable list of all encodings
        /// </summary>
        /// <returns>A list of CharacterSet objects</returns>
        [Route("encodings")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEncodings()
        {
            var result = await _languageServices.GetAllCharacterSets();
            return Ok(result);
        }

        // GET api/languages/encoding/id 
        /// <summary>
        /// Obtain a CharacterSet object given its id
        /// </summary>
        /// <param name="id">The id of the CharacterSet. E.g. 106</param>
        /// <returns></returns>
        [Route("encoding/id/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEncoding(string id)
        {
            var result = await _languageServices.GetCharacterSetById(id);
            return Ok(result);
        }

        // GET api/languages/encoding/id 
        /// <summary>
        /// Obtain a CharacterSet object given its code
        /// </summary>
        /// <param name="id">The code of the CharacterSet. E.g. iso-8859-1</param>
        /// <returns></returns>
        [Route("encoding/code/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEncodingByCode(string id)
        {
            var result = await _languageServices.GetCharacterSetByCode(id);
            return Ok(result);
        }

    }
}
