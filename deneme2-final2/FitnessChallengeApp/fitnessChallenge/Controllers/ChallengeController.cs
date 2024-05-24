// Controllers/ChallengeController.cs
using Microsoft.AspNetCore.Mvc;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace fitnessChallenge.Controllers
{
    [Authorize]
    public class ChallengeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ChallengeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Favorite(int challengeId)
        {
            var userId = _userManager.GetUserId(User);
            var favorite = new FavoriteChallenge { UserId = userId, ChallengeId = challengeId };
            _context.FavoriteChallenges.Add(favorite);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Challenges");
        }
    }
}
