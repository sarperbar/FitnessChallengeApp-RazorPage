using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitnessChallenge.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Challenge> FavoriteChallenges { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            FavoriteChallenges = await _context.FavoriteChallenges
                .Where(fc => fc.UserId == userId)
                .Select(fc => fc.Challenge)
                .ToListAsync();
        }
    }
}
