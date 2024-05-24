using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace fitnessChallenge.Pages.Challenges
{
    public class ReviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; }
        
        public Challenge Challenge { get; set; }
        public List<Review> ExistingReviews { get; set; }
        public Review UserReview { get; set; }
        public bool HasUserReviewed { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            Challenge = await _context.Challenges.FindAsync(id);
            if (Challenge == null)
            {
                return NotFound();
            }

            ExistingReviews = await _context.Reviews.Where(r => r.ChallengeId == id).ToListAsync();

            if (userId != null)
            {
                UserReview = await _context.Reviews.FirstOrDefaultAsync(r => r.ChallengeId == id && r.UserId == userId);
                HasUserReviewed = UserReview != null;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = User.Identity.Name;
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (user == null || userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not logged in.");
                return Page();
            }

            if (await _context.Reviews.AnyAsync(r => r.ChallengeId == id && r.UserId == userId))
            {
                ModelState.AddModelError(string.Empty, "You have already reviewed this challenge.");
                return Page();
            }

            Review.ChallengeId = id;
            Review.UserName = user;
            Review.UserId = userId;

            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Review submitted successfully.";

           
            Challenge = await _context.Challenges.FindAsync(id);
            ExistingReviews = await _context.Reviews.Where(r => r.ChallengeId == id).ToListAsync();
            HasUserReviewed = true;

            return RedirectToPage(new { id = id });
        }
    }
}
