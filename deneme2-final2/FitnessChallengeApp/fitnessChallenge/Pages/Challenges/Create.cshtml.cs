using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Threading.Tasks;

namespace fitnessChallenge.Pages.Challenges
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Challenge Challenge { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Error");
            }

            Challenge.UserId = user.Id;  
            Challenge.UserName = user.UserName;  

            _context.Challenges.Add(Challenge);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
