using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using fitnessChallenge.Data;
using fitnessChallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fitnessChallenge.Pages.Challenges
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Challenge> Challenges { get; set; }

        public async Task OnGetAsync()
        {
            Challenges = await _context.Challenges.ToListAsync();
        }
    }
}
