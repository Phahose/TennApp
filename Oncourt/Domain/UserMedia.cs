namespace OnCourt.Domain
{
    public class UserMedia
    {
        private int MediaId { get; set; }
        private int UserId { get; set; }
        private string FilePath { get; set; } = string.Empty;
        private string MediaType { get; set; } = string.Empty;
        private DateTime CreatedDate { get; set; }
    }
}
