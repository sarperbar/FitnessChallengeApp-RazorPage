namespace fitnessChallenge.Models
{
    public class SearchModel
    {
        public string? Keyword { get; set; }
        public Challenge.Difficulty_Level? Difficulty { get; set; }
        public Challenge.CategoryType? Category { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
