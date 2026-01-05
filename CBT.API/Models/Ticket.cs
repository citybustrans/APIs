namespace CBT.API.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Coupon { get; set; }
        public string IsValid { get; set; } = string.Empty;
    }
}
