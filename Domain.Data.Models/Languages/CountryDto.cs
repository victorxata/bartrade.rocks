namespace Domain.Data.Models.Languages
{
    public class CountryDto
    {
        public string CountryId { get; set; }
        public int LCID { get; set; }
        public string EnglishName { get; set; }
        public string TranslatedName { get; set; }
        public string Iso31622 { get; set; }
    }
}
