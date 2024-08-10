namespace OnCourt.Domain
{
    public class Connections
    {
        private int ConnectionID { get; set; }
        public int UserId1 { get; set;}
        public int UserId2 { get; set;}
        public DateTime ConnectedAt { get; set; }
    }
}
