using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace fitnessChallenge.Pages.Challenges
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Challenge> Challenges { get; set; }
        public IList<FavoriteChallenge> FavoriteChallenges { get; set; }
        [BindProperty]
        public SearchModel Search { get; set; }

        public class SearchModel
        {
            public string Keyword { get; set; }
            public Challenge.Difficulty_Level? Difficulty { get; set; }
            public Challenge.CategoryType? Category { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        public async Task OnGetAsync()
        {
            Search = new SearchModel();
            var query = _context.Challenges.AsQueryable();

            if (!string.IsNullOrEmpty(Search?.Keyword))
            {
                query = query.Where(c => c.Title.Contains(Search.Keyword) || c.Description.Contains(Search.Keyword));
            }
            if (Search?.Difficulty.HasValue == true)
            {
                query = query.Where(c => c.DifficultyLevel == Search.Difficulty.Value);
            }
            if (Search?.Category.HasValue == true)
            {
                query = query.Where(c => c.Category == Search.Category.Value);
            }
            if (Search?.StartDate.HasValue == true)
            {
                query = query.Where(c => c.StartDate >= Search.StartDate.Value);
            }
            if (Search?.EndDate.HasValue == true)
            {
                query = query.Where(c => c.EndDate <= Search.EndDate.Value);
            }

            Challenges = await query.ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                FavoriteChallenges = await _context.FavoriteChallenges
                    .Where(fc => fc.UserId == user.Id)
                    .ToListAsync();
            }
            else
            {
                FavoriteChallenges = new List<FavoriteChallenge>();
            }
        }

        public async Task<IActionResult> OnPostFavoriteAsync(int challengeId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var existingFavorite = await _context.FavoriteChallenges
                .FirstOrDefaultAsync(f => f.UserId == user.Id && f.ChallengeId == challengeId);

            if (existingFavorite == null)
            {
                var favorite = new FavoriteChallenge
                {
                    UserId = user.Id,
                    ChallengeId = challengeId,
                };

                _context.FavoriteChallenges.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
