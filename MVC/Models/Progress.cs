using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Progress
    {
        [Required(ErrorMessage = "Habit is required")]
        public Habit? Habit { get; set; }

        public int ID { get; set; }

        [Required(ErrorMessage = "Habit Progress is required")]
        public int HabitProgress { get; set; }

        [Display(Name = "Is Completed")]
        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "The note field is required")]
        public string? Note { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
    }
}
