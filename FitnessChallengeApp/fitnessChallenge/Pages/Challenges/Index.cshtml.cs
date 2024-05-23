using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IList<Challenge> Challenges { get; set; }

        [BindProperty(SupportsGet = true)]
        public SearchModel Search { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Challenges.AsQueryable();

            if (!string.IsNullOrEmpty(Search.Keyword))
            {
                query = query.Where(c => c.Title.Contains(Search.Keyword) || c.Description.Contains(Search.Keyword));
            }

            if (Search.Difficulty.HasValue)
            {
                query = query.Where(c => c.DifficultyLevel == Search.Difficulty.Value);
            }

            if (Search.Category.HasValue)
            {
                query = query.Where(c => c.Category == Search.Category.Value);
            }

            if (Search.StartDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= Search.StartDate.Value);
            }

            if (Search.EndDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= Search.EndDate.Value);
            }

            Challenges = await query.ToListAsync();
        }
    }
}
