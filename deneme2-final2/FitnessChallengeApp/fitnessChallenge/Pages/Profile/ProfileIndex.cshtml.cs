using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Security.Claims;

namespace fitnessChallenge.Pages.Profile
{
    public class ProfileIndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileIndexModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public string ProfilePictureUrl { get; set; }
        public string Bio { get; set; }
        public IList<FavoriteChallenge> FavoriteChallenges { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            ProfilePictureUrl = claims.FirstOrDefault(c => c.Type == "ProfilePictureUrl")?.Value;
            Bio = claims.FirstOrDefault(c => c.Type == "Bio")?.Value;

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
