namespace fitnessChallenge.Models
{
    public class Challenge
    {
        public int ChallengeId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public CategoryType Category { get; set; }

        public enum DifficultyLevel
        {
            VeryEasy, Easy, Medium, Hard, VeryHard
        }

        public enum CategoryType
        {
            Yoga, Powerlifting, Bodybuilding, Pilates, Casual
        }
    }
}
