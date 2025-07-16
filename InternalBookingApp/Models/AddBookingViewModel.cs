namespace InternalBookingApp.Models
{
    public class AddBookingViewModel
    {
      
        public required Guid ResourceId { get; set; }

       
        public required DateTime StartTime { get; set; }

    
        public required DateTime EndTime { get; set; }

        
        public required string BookedBy { get; set; }

       
        public required string Purpose { get; set; }
    }
}
