using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Threading.Tasks;

namespace fitnessChallenge.Pages.Challenges
{
    public class JoinModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public JoinModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Katılma işlemi için gerekli kodlar burada olacak

            return RedirectToPage("./Index");
        }
    }
}
