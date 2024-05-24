namespace fitnessChallenge.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Difficulty_Level DifficultyLevel { get; set; }
        public CategoryType Category { get; set; }

        public ICollection<Review> Reviews { get; set; } 

        public enum Difficulty_Level
        {
            VeryEasy = 0, Easy = 1, Medium = 2, Hard = 3, VeryHard = 4
        }

        public enum CategoryType
        {
            Yoga = 0, Powerlifting = 1, Bodybuilding = 2, Pilates = 3, Casual = 4
        }
    }
}
