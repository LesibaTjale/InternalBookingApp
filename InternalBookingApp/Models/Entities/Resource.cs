namespace InternalBookingApp.Models.Entities
{
    public class Resource
    {

        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }

   //     public ICollection<Booking>? Bookings { get; set; }

    }
}
