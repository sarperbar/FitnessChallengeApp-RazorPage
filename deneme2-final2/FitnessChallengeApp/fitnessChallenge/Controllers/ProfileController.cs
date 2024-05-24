// Controllers/ProfileController.cs
using Microsoft.AspNetCore.Mvc;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace fitnessChallenge.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Favorites()
        {
            var userId = _userManager.GetUserId(User);
            var favorites = await _context.FavoriteChallenges
                .Include(fc => fc.Challenge)
                .Where(fc => fc.UserId == userId)
                .ToListAsync();
            return View(favorites);
        }
    }
}
