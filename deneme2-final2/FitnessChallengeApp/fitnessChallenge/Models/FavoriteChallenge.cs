using System.ComponentModel.DataAnnotations;

namespace fitnessChallenge.Models
{
    public class FavoriteChallenge
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ChallengeId { get; set; }

        public Challenge Challenge { get; set; }
    }
}
