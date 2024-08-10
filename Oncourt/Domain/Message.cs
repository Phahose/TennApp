namespace OnCourt.Domain
{
    public class Message
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int RecieverID { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
