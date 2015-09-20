namespace Domain.Data.Models.Languages
{
    public class SelectedLanguageDto
    {

        public SelectedLanguageDto()
        {
        }

        public SelectedLanguageDto(Language language)
        {
            LanguageId= language.Id;
            IsoCode = language.IsoCode;
            CustomCode = language.CustomCode;
        }

        public string LanguageId { get; set; }
        public string IsoCode { get; set; }
        public string CustomCode { get; set; }
    }
}
