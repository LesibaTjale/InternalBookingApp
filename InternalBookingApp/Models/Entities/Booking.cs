using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalBookingApp.Models.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public Guid ResourceId { get; set; }

        [ForeignKey("ResourceId")]
        public Resource Resource { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string BookedBy { get; set; }

        [Required]
        public string Purpose { get; set; }

    }
}
