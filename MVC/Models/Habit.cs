using System.ComponentModel.DataAnnotations; 

namespace MVC.Models
{
    public class Habit
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Frequency is required")]
        public int Frequency { get; set; }

        public TargerFrequency Repeat { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        public enum TargerFrequency
        {
            Daily,
            Weekly,
            Monthly
        }
    }
}
