namespace CBT.API.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Coupon { get; set; }
        public string IsValid { get; set; }
    }
}
