namespace OnCourt.Domain
{
    public class UserMedia
    {
        public int MediaId { get; set; }
        public int UserId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
