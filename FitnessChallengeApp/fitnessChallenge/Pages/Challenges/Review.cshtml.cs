using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Threading.Tasks;

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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Challenge = await _context.Challenges.FindAsync(id);
            if (Challenge == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            /*
            if (!ModelState.IsValid)
            {
                return Page();
            }
*/
            var user = User.Identity.Name;
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (user == null || userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not logged in.");
                return Page();
            }

            
            Review.ChallengeId = id;
            Review.UserName = user;
            Review.UserId = userId;

            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
