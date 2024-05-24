using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitnessChallenge.Pages.Profile
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

        public IList<FavoriteChallenge> FavoriteChallenges { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            FavoriteChallenges = await _context.FavoriteChallenges
                .Include(fc => fc.Challenge)
                .Where(fc => fc.UserId == user.Id)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFavoriteAsync(int favoriteId)
        {
            var favoriteChallenge = await _context.FavoriteChallenges.FindAsync(favoriteId);
            if (favoriteChallenge != null)
            {
                _context.FavoriteChallenges.Remove(favoriteChallenge);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
