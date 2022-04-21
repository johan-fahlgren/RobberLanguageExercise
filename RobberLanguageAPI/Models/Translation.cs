namespace RobberLanguageAPI.Models
{
    public class Translation
    {
        public int Id { get; set; }
        public string? OriginalSentance { get; set; }
        public string? TranslatedSentance { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

    }
}
