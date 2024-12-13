using System.ComponentModel.DataAnnotations;

namespace PlantCareBackEnd.Models
{
    public class Plant
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; } // "succulent", "tropical", "herb", "cacti"
        public int? WateringFrequencyDays { get; set; }
        public DateTime? LastWateredDate { get; set; }
        public string? Location { get; set; }
    }
}
