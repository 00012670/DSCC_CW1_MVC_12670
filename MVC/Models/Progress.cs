using System.ComponentModel.DataAnnotations;
using System;

namespace MVC.Models
{
    public class Progress
    {
        public Habit ?Habit { get; set; }
        public int ID { get; set; }
        public int HabitProgress { get; set; }
        public bool IsCompleted { get; set; }
        [Required(ErrorMessage = "The note field is required")]
        public string ?Note { get; set; }
        public DateTime EndDate { get; set; }
    }
}
