using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitnessChallenge.Pages.Challenges
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ChallengeViewModel> Challenges { get; set; }
        [BindProperty(SupportsGet = true)]
        public SearchModel Search { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Challenges
                .Include(c => c.Reviews)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Search.Keyword))
            {
                query = query.Where(c => c.Title.Contains(Search.Keyword) || c.Description.Contains(Search.Keyword));
            }

            if (Search.Difficulty != null)
            {
                query = query.Where(c => c.DifficultyLevel == Search.Difficulty);
            }

            if (Search.Category != null)
            {
                query = query.Where(c => c.Category == Search.Category);
            }

            if (Search.StartDate != null)
            {
                query = query.Where(c => c.StartDate >= Search.StartDate);
            }

            if (Search.EndDate != null)
            {
                query = query.Where(c => c.EndDate <= Search.EndDate);
            }

            var challenges = await query.ToListAsync();

            Challenges = challenges.Select(c => new ChallengeViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                DifficultyLevel = c.DifficultyLevel,
                Category = c.Category,
                AverageRating = c.Reviews.Any() ? c.Reviews.Average(r => r.Rating) : 0
            }).ToList();
        }

        public class SearchModel
        {
            public string Keyword { get; set; }
            public Challenge.Difficulty_Level? Difficulty { get; set; }
            public Challenge.CategoryType? Category { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        public class ChallengeViewModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public Challenge.Difficulty_Level DifficultyLevel { get; set; }
            public Challenge.CategoryType Category { get; set; }
            public double AverageRating { get; set; }
        }
    }
}
