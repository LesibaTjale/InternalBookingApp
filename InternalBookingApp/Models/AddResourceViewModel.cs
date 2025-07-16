namespace InternalBookingApp.Models
{
    public class AddResourceViewModel
    {

        public required string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }

    }
}
