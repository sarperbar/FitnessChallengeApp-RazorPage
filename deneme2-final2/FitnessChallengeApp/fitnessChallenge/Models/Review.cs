namespace fitnessChallenge.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Challenge Challenge { get; set; } 
    }
}
