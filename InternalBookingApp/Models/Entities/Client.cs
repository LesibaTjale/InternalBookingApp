namespace InternalBookingApp.Models.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        required
        public string FullName { get; set; }
        required
        public string Email { get; set; }
        required
        public string Phone { get; set; }
    }
}
