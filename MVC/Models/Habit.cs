using System;

namespace MVC.Models
{
    public class Habit
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int Frequency { get; set; }
        public TargerFrequency Repeat { get; set; }
        public DateTime StartDate { get; set; }

        public enum TargerFrequency
        {
            Daily,
            Weekly,
            Monthly
        }
    }
}
